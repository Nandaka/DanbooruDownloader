using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DanbooruDownloader3.Entity;
using System.IO;
using System.ComponentModel;

namespace DanbooruDownloader3.DAO
{
    public class DanbooruPostDao
    {
        public static DanbooruTagsDao tagsDao = new DanbooruTagsDao("tags.xml");

        #region ctor
        public DanbooruPostDao(string url, DanbooruProvider provider)
        {
            this.Provider = provider;
            this.Query = url.Split('\\').Last();
            this.SearchTags = "";
            this.Referer = provider.Url;

            if (url.ToLower().EndsWith(".xml"))
                ReadXML(url);
            else 
                ReadJSON(url);
        }

        public DanbooruPostDao(Stream input, DanbooruProvider provider, string query, string searchTags, string referer, Boolean isXMl)
        {
            this.Provider = provider;
            this.Query = query;
            this.SearchTags = searchTags;
            this.Referer = referer;

            if (isXMl)
            {
                ReadXML(input);
            }
            else ReadJSON(input);
        }
        #endregion
        
        #region property
        public DanbooruProvider Provider { get; set; }

        public string Query { get; set; }

        public string SearchTags { get; set; }

        public string Referer { get; set; }

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
        #endregion
        
        private void ReadXML(string filename)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                ProcessXML(reader);
            }
        }

        private void ReadXML(Stream input)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;

            using (XmlTextReader reader = new XmlTextReader(input))
            {
                ProcessXML(reader);
            }
                       
        }

        private void ProcessXML(XmlTextReader reader)
        {
            RawData = "";
            if (Provider.BoardType == BoardType.Shimmie2)
            {
                posts =  Engine.ShimmieEngine.ParseRSS(reader, Provider, Query, SearchTags);
                foreach (var item in posts)
                {
                    RawData += item.Id + ":" + item.FileUrl + ", ";
                    item.TagsEntity = DanbooruPostDao.tagsDao.ParseTagsString(item.Tags);
                }
            }
            else
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("posts"))
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name.Equals("Count"))    // Posts Count
                                    {
                                        postCount = int.Parse(reader.Value);
                                        RawData += "postCount:" + postCount;
                                    }
                                    else if (reader.Name.Equals("offset")) // Post Offset
                                    {
                                        offset = int.Parse(reader.Value);
                                        RawData += ", offset:" + offset;
                                    }
                                }
                            }
                            else if (reader.Name.Equals("post"))
                            {
                                DanbooruPost post = new DanbooruPost();
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "id": post.Id = reader.Value; RawData += ", id:" + reader.Value; break;
                                        case "tags": 
                                            post.Tags = reader.Value;
                                            post.TagsEntity =  DanbooruPostDao.tagsDao.ParseTagsString(post.Tags);
                                            break;
                                        case "source": post.Source = reader.Value; break;
                                        case "creator_id": post.CreatorId = reader.Value; break;
                                        case "file_url": post.FileUrl = AppendHttp(reader.Value); break;
                                        case "width": post.Width = -1;
                                            try
                                            {
                                                post.Width = Int32.Parse(reader.Value);
                                            }
                                            catch (Exception) { if (FormMain.Debug) throw; }
                                            break;
                                        case "height": post.Height = -1;
                                            try
                                            {
                                                post.Height = Int32.Parse(reader.Value);
                                            }
                                            catch (Exception) { if (FormMain.Debug) throw; }
                                            break;
                                        case "change": post.Change = reader.Value; break;
                                        case "score": post.Score = reader.Value; break;
                                        case "rating": post.Rating = reader.Value; break;
                                        case "status": post.Status = reader.Value; break;
                                        case "has_children": post.HasChildren = Boolean.Parse(reader.Value); break;
                                        case "created_at": post.CreatedAt = reader.Value; break;
                                        case "md5": post.MD5 = reader.Value; break;
                                        case "preview_url": post.PreviewUrl = AppendHttp(reader.Value); break;
                                        case "preview_width": post.PreviewWidth = -1;
                                            try
                                            {
                                                post.PreviewWidth = Int32.Parse(reader.Value);
                                            }
                                            catch (Exception) { if (FormMain.Debug) throw; }
                                            break;
                                        case "preview_height": post.PreviewHeight = -1;
                                            try
                                            {
                                                post.PreviewHeight = Int32.Parse(reader.Value);
                                            }
                                            catch (Exception) { if (FormMain.Debug) throw; }
                                            break;
                                        case "parent_id": post.ParentId = reader.Value; break;
                                        case "sample_url": post.SampleUrl = AppendHttp(reader.Value); break;
                                        case "sample_width": post.SampleWidth = -1;
                                            try
                                            {
                                                post.SampleWidth = Int32.Parse(reader.Value);
                                            }
                                            catch (Exception) { if (FormMain.Debug) throw; }
                                            break;
                                        case "sample_height": post.SampleHeight = -1;
                                            try
                                            {
                                                post.SampleHeight = Int32.Parse(reader.Value);
                                            }
                                            catch (Exception) { if (FormMain.Debug) throw; }
                                            break;
                                    }
                                }
                                post.Provider = this.Provider.Name;
                                post.Query = this.Query;
                                post.SearchTags = this.SearchTags;
                                if (Provider.BoardType == BoardType.Danbooru || Provider.BoardType == BoardType.Shimmie2)
                                {
                                    post.Referer = this.Referer + @"/post/show/" + post.Id;
                                }
                                else if(Provider.BoardType == BoardType.Gelbooru)
                                {
                                    post.Referer = this.Referer + @"/index.php?page=post&s=view&id=" + post.Id;
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

        public void ReadJSON(string filename)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;
            DanbooruPost post = null;
            String json = "";
            String tmp;


            StreamReader reader;
            try
            {
                reader = File.OpenText(filename);
            }
            catch (Exception)
            {
                reader = new StreamReader(System.Net.WebRequest.Create(filename).GetResponse().GetResponseStream());
            }

            ProcessJson(ref post, ref json, reader, out tmp);

            RawData = json;
        }

        public void ReadJSON(Stream input)
        {
            posts = new BindingList<DanbooruPost>();
            actualCount = 0;
            DanbooruPost post = null;
            String json = "";
            String tmp;

            StreamReader reader = new StreamReader(input);

            ProcessJson(ref post, ref json, reader, out tmp);
            RawData = json;
        }

        private void ProcessJson(ref DanbooruPost post, ref String json, StreamReader reader, out String tmp)
        {
            while ((tmp = reader.ReadLine()) != null)
            {
                json += tmp;
            }
            reader.Close();

            if (json.Length < 4) return;

            json = json.Substring(2, json.Length - 4);
            string[] splitter = { "},{" };
            string[] split = json.Split(splitter, StringSplitOptions.None);

            foreach (string str in split)
            {
                post = new DanbooruPost();
                string[] node = str.Split(',');
                foreach (string str2 in node)
                {

                    string[] val = str2.Split(':');
                    switch (val[0])
                    {
                        case "\"id\"":
                            post.Id = val[1].Replace("\"", "");
                            break;
                        case "\"tags\"":
                            post.Tags = val[1].Replace("\"", "");
                            post.TagsEntity = DanbooruPostDao.tagsDao.ParseTagsString(post.Tags);
                            break;
                        case "\"source\"":
                            post.Source = val[1].Replace("\"", "");
                            break;
                        case "\"creator_id\"":
                            post.CreatorId = val[1].Replace("\"", "");
                            break;
                        case "\"file_url\"":
                            post.FileUrl = "http:" + val[2].Replace("\\", "").Replace("\"", "");
                            break;
                        case "\"width\"":
                            post.Width = -1;
                            try
                            {
                                post.Width = Convert.ToInt32(val[1]);
                            }
                            catch (Exception) { if (FormMain.Debug) throw; }
                            break;
                        case "\"height\"":
                            post.Height = -1;
                            try
                            {
                                post.Height = Convert.ToInt32(val[1]);
                            }
                            catch (Exception) { if (FormMain.Debug) throw; }
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
                            if (val.Length > 2)
                            {
                                post.PreviewUrl = "http:" + val[2].Replace("\\", "").Replace("\"", "");
                            }
                            else
                            {
                                post.PreviewUrl = Provider.Url + val[1].Replace("\"", "");
                            }
                            break;
                        case "\"preview_width\"":
                            post.PreviewWidth = -1;
                            try
                            {
                                post.PreviewWidth = Convert.ToInt32(val[1]);
                            }
                            catch (Exception) { if (FormMain.Debug) throw; }
                            break;
                        case "\"preview_height\"":
                            post.PreviewHeight = -1;
                            try
                            {
                                post.PreviewHeight = Convert.ToInt32(val[1]);
                            }
                            catch (Exception) { if (FormMain.Debug) throw; }
                            break;
                        default: break;
                    }
                }
                post.Provider = this.Provider.Name;
                post.Query = this.Query;
                post.SearchTags = this.SearchTags;
                post.Referer = this.Referer + @"/post/show/" + post.Id;
                this.posts.Add(post);
                actualCount++;
            }
        }

        private string AppendHttp(string url)
        {
            if (!url.StartsWith("http"))
            {
                return Provider.Url + url;
            }
            return url;
        }
    }
}
