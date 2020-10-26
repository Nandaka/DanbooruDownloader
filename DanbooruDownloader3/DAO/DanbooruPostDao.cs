using DanbooruDownloader3.Engine;
using DanbooruDownloader3.Entity;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace DanbooruDownloader3.DAO
{
    public class DanbooruPostDao
    {
        #region ctor

        /// <summary>
        /// parse xml/json list file
        /// </summary>
        /// <param name="option"></param>
        public DanbooruPostDao(DanbooruPostDaoOption option)
        {
            this.Option = option;
            if (option.Url.ToLower().EndsWith(".xml"))
                ReadXML(option.Url);
            else
                ReadJSON(option.Url);
        }

        /// <summary>
        /// parse xml/json list stream and close it.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="option"></param>
        public DanbooruPostDao(Stream input, DanbooruPostDaoOption option, int? currentPage)
        {
            string rawData = "";
            this.Option = option;
            try
            {
                using (StreamReader reader = new StreamReader(input))
                {
                    rawData = reader.ReadToEnd();
                }
                this.Option = option;
                switch (option.Provider.Preferred)
                {
                    case PreferredMethod.Xml:
                        ReadXML(rawData, option);
                        break;

                    case PreferredMethod.Json:
                        ReadJSON(rawData, option);
                        break;

                    case PreferredMethod.Html:
                        DanbooruSearchParam param = new DanbooruSearchParam()
                        {
                            Provider = option.Provider,
                            Tag = option.SearchTags,
                            Page = currentPage,
                            Option = option
                        };
                        if (option.Provider.BoardType == BoardType.Danbooru)
                        {
                            SankakuComplexParser parser = new SankakuComplexParser();
                            posts = parser.Parse(rawData, param);
                            NextId = param.NextKey;
                            if (parser.TotalPost.HasValue)
                                postCount = parser.TotalPost.Value;
                        }
                        else if (option.Provider.BoardType == BoardType.Gelbooru)
                        {
                            GelbooruHtmlParser parser = new GelbooruHtmlParser();
                            posts = parser.Parse(rawData, param);
                        }
                        else
                        {
                            throw new NotImplementedException("No HTML Parser for: " + option.Provider.Name);
                        }
                        break;
                }
            }
            catch (Exception)
            {
                Helper.DumpRawData(rawData, Option.Provider, option.Query);
                throw;
            }
        }

        #endregion ctor

        #region property

        // used by sankaku
        public string NextId { get; set; }

        public DanbooruPostDaoOption Option { get; set; }

        private int postCount;

        public int PostCount
        {
            get { return postCount; }
        }

        private int offset;

        public int Offset
        {
            get { return offset; }
        }

        private int actualCount;

        public int ActualCount
        {
            get { return actualCount; }
        }

        private BindingList<DanbooruPost> posts;

        public BindingList<DanbooruPost> Posts
        {
            get { return posts; }
        }

        public String RawData { get; set; }

        public bool Success { get; set; }

        public string ResponseMessage { get; set; }

        #endregion property

        private void ReadXML(string filename)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;
            StreamReader reader = null;
            try
            {
                try
                {
                    reader = File.OpenText(filename);
                }
                catch (Exception)
                {
                    reader = new StreamReader(System.Net.WebRequest.Create(filename).GetResponse().GetResponseStream());
                }

                string rawData = reader.ReadToEnd();
                ProcessXML(rawData);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private void ReadXML(string rawData, DanbooruPostDaoOption option)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;

            // Issue #60
            // modify xml to insert html entity
            rawData = Regex.Replace(rawData, @"(<\?xml.*\?>)", "$1<!DOCTYPE document SYSTEM \"xhtml.ent\">");

            ProcessXML(rawData);
        }

        private void ProcessXML(string rawData)
        {
            RawData = rawData;
            using (StringReader strReader = new StringReader(rawData))
            {
                using (XmlTextReader reader = new XmlTextReader(strReader))
                {
                    if (Option.Provider.BoardType == BoardType.Shimmie2)
                    {
                        posts = Engine.ShimmieEngine.ParseRSS(rawData, Option);
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element: // The node is an element.
                                    var nodeName = reader.Name.ToLowerInvariant();
                                    if (nodeName.Equals("posts"))
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.ToLowerInvariant().Equals("count"))    // Posts Count
                                            {
                                                postCount = int.Parse(reader.Value);
                                            }
                                            else if (reader.Name.ToLowerInvariant().Equals("offset")) // Post Offset
                                            {
                                                offset = int.Parse(reader.Value);
                                            }
                                        }
                                    }
                                    else if (nodeName.Equals("response"))
                                    {
                                        Success = true;
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.ToLowerInvariant().Equals("reason"))    // Posts Count
                                            {
                                                ResponseMessage = reader.Value;
                                            }
                                            else if (reader.Name.ToLowerInvariant().Equals("success"))    // Posts Count
                                            {
                                                Success = bool.Parse(reader.Value);
                                            }
                                        }
                                    }
                                    else if (nodeName.Equals("result"))
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.ToLowerInvariant().Equals("success"))
                                            {
                                                Success = bool.Parse(reader.Value);
                                                ResponseMessage = reader.ReadString();
                                            }
                                        }
                                    }
                                    else if (nodeName.Equals("post"))
                                    {
                                        DanbooruPost post = new DanbooruPost();
                                        ParsePostAttributes(reader, post);

                                        post.Hidden = Helper.CheckBlacklistedTag(post, Option);
                                        post.Provider = Option.Provider;
                                        post.Query = Option.Query;
                                        post.SearchTags = Option.SearchTags;
                                        if (Option.Provider.BoardType == BoardType.Danbooru || Option.Provider.BoardType == BoardType.Shimmie2)
                                        {
                                            post.Referer = Option.Provider.Url + @"/post/show/" + post.Id;
                                        }
                                        else if (Option.Provider.BoardType == BoardType.Gelbooru)
                                        {
                                            post.Referer = Option.Provider.Url + @"/index.php?page=post&s=view&id=" + post.Id;
                                        }
                                        posts.Add(post);
                                        actualCount++;
                                    }
                                    break;

                                case XmlNodeType.EndElement: //Display the end of the element.
                                    //txtResult.AppendText("END");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void ParsePostAttributes(XmlTextReader reader, DanbooruPost post)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name.ToLowerInvariant())
                {
                    case "id": post.Id = reader.Value; break;
                    case "tags":
                        post.Tags = reader.Value;
                        post.TagsEntity = Helper.ParseTags(post.Tags, Option.Provider);
                        break;

                    case "source": post.Source = reader.Value; break;
                    case "creator_id": post.CreatorId = reader.Value; break;
                    case "file_url": post.FileUrl = AppendHttp(reader.Value); break;
                    case "width":
                        post.Width = -1;
                        try
                        {
                            post.Width = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "height":
                        post.Height = -1;
                        try
                        {
                            post.Height = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "change": post.Change = reader.Value; break;
                    case "score": post.Score = reader.Value; break;
                    case "rating": post.Rating = reader.Value; break;
                    case "status": post.Status = reader.Value; break;
                    case "has_children":
                        int num;
                        bool isNum = Int32.TryParse(reader.Value, out num);
                        if (isNum)
                        {
                            post.HasChildren = Convert.ToBoolean(num);
                        }
                        else
                        {
                            post.HasChildren = Convert.ToBoolean(reader.Value);
                        }
                        break;

                    case "created_at":
                        post.CreatedAt = reader.Value;
                        post.CreatedAtDateTime = ParseDateTime(post.CreatedAt, Option.Provider);
                        break;

                    case "md5": post.MD5 = reader.Value; break;
                    case "preview_url": post.PreviewUrl = AppendHttp(reader.Value); break;
                    case "preview_width":
                        post.PreviewWidth = -1;
                        try
                        {
                            post.PreviewWidth = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "preview_height":
                        post.PreviewHeight = -1;
                        try
                        {
                            post.PreviewHeight = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "parent_id": post.ParentId = reader.Value; break;
                    case "sample_url": post.SampleUrl = AppendHttp(reader.Value); break;
                    case "sample_width":
                        post.SampleWidth = -1;
                        try
                        {
                            post.SampleWidth = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "sample_height":
                        post.SampleHeight = -1;
                        try
                        {
                            post.SampleHeight = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "jpeg_url": post.JpegUrl = AppendHttp(reader.Value); break;
                    case "jpeg_width":
                        post.JpegWidth = -1;
                        try
                        {
                            post.JpegWidth = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;

                    case "jpeg_height":
                        post.JpegHeight = -1;
                        try
                        {
                            post.JpegHeight = Int32.Parse(reader.Value);
                        }
                        catch (Exception)
                        {
#if DEBUG
                            throw;
#endif
                        }
                        break;
                }
            }
        }

        public static DateTime ParseDateTime(string dtStr, DanbooruProvider provider)
        {
            DateTime dt = DateTime.MinValue;
            if (!String.IsNullOrWhiteSpace(dtStr) &&
                provider.DateTimeFormat != DanbooruProviderDao.DATETIME_FORMAT_NA)
            {
                if (provider.DateTimeFormat == DanbooruProviderDao.DATETIME_FORMAT_UNIX)
                {
                    double timestamp = 0;
                    double.TryParse(dtStr, out timestamp);
                    dt = new DateTime(1970, 1, 1).AddSeconds(timestamp);
                }
                else
                {
                    var result = DateTime.TryParseExact(dtStr,
                                           provider.DateTimeFormat,
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out dt);
                    if (!result)
                    {
                        Program.Logger.WarnFormat("Invalid format: {1} for {2} ==> {0}.", dt, provider.DateTimeFormat, provider.Name);
                    }
                }
            }
            return dt;
        }

        public void ReadJSON(string filename)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;

            StreamReader reader = null;
            try
            {
                try
                {
                    reader = File.OpenText(filename);
                }
                catch (Exception)
                {
                    reader = new StreamReader(System.Net.WebRequest.Create(filename).GetResponse().GetResponseStream());
                }

                string rawData = reader.ReadToEnd();

                ProcessJson(rawData);
                RawData = rawData;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public void ReadJSON(string rawData, DanbooruPostDaoOption option)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;

            using (StringReader reader = new StringReader(rawData))
            {
                ProcessJson(rawData);
                //RawData = json;
                RawData = rawData;
            }
        }

        private void ProcessJson(String json)
        {
            if (json.Length < 4) return;

            if (json.StartsWith("{"))
            {
                json = json.Substring(1, json.Length - 2);
                string[] splitter = { "\"," };
                string[] node = json.Split(splitter, StringSplitOptions.None);
                foreach (string str2 in node)
                {
                    string[] val = str2.Split(':');
                    switch (val[0].ToLowerInvariant())
                    {
                        case "\"reason\"":
                            ResponseMessage = val[1].Replace("\"", "");
                            break;

                        case "\"success\"":
                            Success = bool.Parse(val[1].Replace("\"", ""));
                            break;
                    }
                }
            }
            else
            {
                json = json.Substring(2, json.Length - 4);
                string[] splitter = { "},{" };
                string[] split = json.Split(splitter, StringSplitOptions.None);

                foreach (string str in split)
                {
                    DanbooruPost post = new DanbooruPost();
                    string[] node = str.Split(',');
                    foreach (string str2 in node)
                    {
                        char[] splitter2 = { ':' };
                        string[] val = str2.Split(splitter2, 2);
                        switch (val[0].ToLowerInvariant())
                        {
                            case "\"id\"":
                                post.Id = val[1].Replace("\"", "");
                                break;

                            case "\"tags\"":
                                post.Tags = val[1].Replace("\"", "");
                                post.Tags = Helper.DecodeEncodedNonAsciiCharacters(post.Tags);
                                post.TagsEntity = Helper.ParseTags(post.Tags, Option.Provider);
                                break;

                            case "\"source\"":
                                post.Source = val[1].Replace("\"", "");
                                break;

                            case "\"creator_id\"":
                                post.CreatorId = val[1].Replace("\"", "");
                                break;

                            case "\"file_url\"":
                                post.FileUrl = AppendHttp(val[1].Replace("\"", ""));
                                break;

                            case "\"width\"":
                                post.Width = -1;
                                try
                                {
                                    post.Width = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"height\"":
                                post.Height = -1;
                                try
                                {
                                    post.Height = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"score\"":
                                post.Score = val[1];
                                break;

                            case "\"rating\"":
                                post.Rating = val[1].Replace("\"", "");
                                break;

                            case "\"md5\"":
                                post.MD5 = val[1].Replace("\"", "");
                                break;

                            case "\"preview_url\"":
                                post.PreviewUrl = AppendHttp(val[1].Replace("\"", ""));
                                break;

                            case "\"preview_width\"":
                                post.PreviewWidth = -1;
                                try
                                {
                                    post.PreviewWidth = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"preview_height\"":
                                post.PreviewHeight = -1;
                                try
                                {
                                    post.PreviewHeight = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"file_size\"":
                                post.Filesize = -1;
                                try
                                {
                                    post.Filesize = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"parent_id\"":
                                post.ParentId = val[1];
                                break;

                            case "\"status\"":
                                post.Status = val[1];
                                break;

                            case "\"created_at\"":
                                post.CreatedAt = val[1];
                                post.CreatedAtDateTime = ParseDateTime(post.CreatedAt, Option.Provider);
                                break;

                            case "\"has_children\"":
                                post.HasChildren = false;
                                try
                                {
                                    post.HasChildren = Convert.ToBoolean(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"sample_width\"":
                                post.SampleWidth = -1;
                                try
                                {
                                    post.SampleWidth = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"sample_height\"":
                                post.SampleHeight = -1;
                                try
                                {
                                    post.SampleHeight = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"sample_url\"":
                                post.SampleUrl = AppendHttp(val[1].Replace("\"", ""));
                                break;

                            case "\"jpeg_width\"":
                                post.JpegWidth = -1;
                                try
                                {
                                    post.JpegWidth = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"jpeg_height\"":
                                post.JpegHeight = -1;
                                try
                                {
                                    post.JpegHeight = Convert.ToInt32(val[1]);
                                }
                                catch (Exception)
                                {
#if DEBUG
                                    throw;
#endif
                                }
                                break;

                            case "\"jpeg_url\"":
                                post.JpegUrl = AppendHttp(val[1].Replace("\"", ""));
                                break;

                            default: break;
                        }
                    }
                    post.Hidden = Helper.CheckBlacklistedTag(post, Option);
                    post.Provider = Option.Provider;
                    post.Query = Option.Query;
                    post.SearchTags = Option.SearchTags;
                    post.Referer = Option.Provider.Url + @"/post/show/" + post.Id;
                    this.posts.Add(post);
                    actualCount++;
                }
            }
        }

        private string AppendHttp(string url)
        {
            url = Helper.FixUrl(url, hostname: Option.Provider.Url);
            return url.Replace(@"\", "");
        }
    }
}