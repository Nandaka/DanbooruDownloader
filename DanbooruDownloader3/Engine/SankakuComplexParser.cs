using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;
using HtmlAgilityPack;

namespace DanbooruDownloader3.Engine
{
    public class SankakuComplexParser : IEngine
    {
        public static DanbooruPost ParsePost(DanbooruPost post, string postHtml)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(postHtml);
                string file_url = "";
                string sample_url = "";

                // Flash Game or bmp
                if (post.PreviewUrl.EndsWith("download-preview.png"))
                {
                    var links = doc.DocumentNode.SelectNodes("//a");
                    foreach (var link in links)
                    {
                        // flash
                        if (link.InnerText == "Save this file (right click and save as)")
                        {
                            file_url = Helper.FixUrl(link.GetAttributeValue("href", ""));
                            // http://cs.sankakucomplex.com/data/f6/23/f623ea7559ef39d96ebb0ca7530586b8.swf
                            post.MD5 = file_url.Substring(file_url.LastIndexOf("/") + 1);
                            post.MD5 = post.MD5.Substring(0, post.MD5.Length - 4);

                            break;
                        }
                        // bmp
                        if (link.InnerText == "Download")
                        {
                            file_url = Helper.FixUrl(link.GetAttributeValue("href", ""));
                            break;
                        }
                    }
                }
                else
                {
                    var lowresElement = doc.DocumentNode.SelectSingleNode("//a[@id='lowres']");
                    if (lowresElement != null)
                    {
                        sample_url = Helper.FixUrl(lowresElement.GetAttributeValue("href", ""));
                    }
                    var highresElement = doc.DocumentNode.SelectSingleNode("//a[@id='highres']");
                    if (highresElement != null)
                    {
                        file_url = Helper.FixUrl(highresElement.GetAttributeValue("href", ""));
                    }
                }

                post.FileUrl = file_url;
                if (!string.IsNullOrWhiteSpace(file_url) && string.IsNullOrWhiteSpace(sample_url))
                    sample_url = file_url;
                post.SampleUrl = sample_url;
                return post;
            }
            catch (Exception ex)
            {
                string filename = "Dump for Post " + post.Id + post.Provider.Name + " Query " + post.Query + ".txt";
                bool result = Helper.DumpRawData(postHtml, filename);
                if (!result) Program.Logger.Error("Failed to dump rawdata to: " + filename, ex);
                throw;
            }
        }

        public BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam searchParam)
        {
            try
            {
                this.SearchParam = searchParam;
                this.RawData = data;

                BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(data);

                // remove popular preview
                var popular = doc.DocumentNode.SelectSingleNode("//div[@id='popular-preview']");
                if (popular != null)
                {
                    popular.Remove();
                }

                // get all thumbs
                var thumbs = doc.DocumentNode.SelectNodes("//span");
                if (thumbs != null && thumbs.Count > 0)
                {
                    foreach (var thumb in thumbs)
                    {
                        if (thumb.GetAttributeValue("class", "").Contains("thumb"))
                        {
                            DanbooruPost post = new DanbooruPost();
                            post.Id = thumb.GetAttributeValue("id", "-1").Substring(1);

                            post.Provider = searchParam.Provider;
                            post.SearchTags = searchParam.Tag;
                            post.Query = GenerateQueryString(searchParam);

                            int i = 0;
                            // get the image link
                            for (; i < thumb.ChildNodes.Count; ++i)
                            {
                                if (thumb.ChildNodes[i].Name == "a") break;
                            }
                            var a = thumb.ChildNodes[i];
                            post.Referer = Helper.FixUrl(searchParam.Provider.Url + a.GetAttributeValue("href", ""));

                            var img = a.ChildNodes[i];
                            var title = img.GetAttributeValue("title", "");
                            post.Tags = title.Substring(0, title.LastIndexOf("Rating:")).Trim();
                            post.Tags = Helper.DecodeEncodedNonAsciiCharacters(post.Tags);
                            post.TagsEntity = Helper.ParseTags(post.Tags, SearchParam.Provider);

                            post.Hidden = Helper.CheckBlacklistedTag(post, searchParam.Option);

                            post.PreviewUrl = Helper.FixUrl(img.GetAttributeValue("src", ""));
                            post.PreviewHeight = img.GetAttributeValue("height", 0);
                            post.PreviewWidth = img.GetAttributeValue("width", 0);

                            // Rating:Explicit Score:4.5 Size:1080x1800 User:System
                            post.Source = "";
                            post.Score = title.Substring(title.LastIndexOf("Score:") + 6);
                            post.Score = post.Score.Substring(0, post.Score.IndexOf(" ")).Trim();

                            string resolution = title.Substring(title.LastIndexOf("Size:") + 5);
                            resolution = resolution.Substring(0, resolution.IndexOf(" ")).Trim();
                            string[] resArr = resolution.Split('x');
                            post.Width = Int32.Parse(resArr[0]);
                            post.Height = Int32.Parse(resArr[1]);

                            string rating = title.Substring(title.LastIndexOf("Rating:") + 7, 1);
                            //rating = rating.Substring(0, rating.IndexOf(" ")).Trim();
                            post.Rating = rating.ToLower();

                            post.CreatorId = title.Substring(title.LastIndexOf("User:") + 5);

                            post.Status = "";

                            post.MD5 = post.PreviewUrl.Substring(post.PreviewUrl.LastIndexOf("/") + 1);
                            post.MD5 = post.MD5.Substring(0, post.MD5.LastIndexOf("."));

                            posts.Add(post);
                        }
                    }
                }

                TotalPost = posts.Count;
                if (!SearchParam.Page.HasValue) SearchParam.Page = 1;
                Offset = TotalPost * SearchParam.Page;
                return posts;
            }
            catch (Exception ex)
            {
                var filename = Helper.SanitizeFilename("Dump for Sankaku Image List - " + searchParam.Tag + " - page " + searchParam.Page + ".txt");
                var result = Helper.DumpRawData(data, filename);
                if (!result) Program.Logger.Error("Failed to dump rawdata to: " + filename, ex);
                throw;
            }
        }

        public int? TotalPost { get; set; }

        public int? Offset { get; set; }

        public string RawData { get; set; }

        public string ResponseMessage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Success
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string GenerateQueryString(DanbooruSearchParam query)
        {
            string tmp = "";

            if (!String.IsNullOrWhiteSpace(query.Tag))
            {
                // convert spaces into '_'
                tmp += query.Tag.Replace(' ', '_');
            }
            if (!String.IsNullOrWhiteSpace(query.Source))
            {
                if (!string.IsNullOrWhiteSpace(tmp))
                {
                    tmp += "+";
                }
                tmp += "source:" + query.Source;
            }
            if (!String.IsNullOrWhiteSpace(query.OrderBy))
            {
                if (!string.IsNullOrWhiteSpace(tmp))
                {
                    tmp += "+";
                }
                tmp += "order:" + query.OrderBy;
            }
            if (!String.IsNullOrWhiteSpace(query.Rating))
            {
                if (!string.IsNullOrWhiteSpace(tmp))
                {
                    tmp += "+";
                }
                tmp += "rating:" + query.Rating;
            }
            if (!string.IsNullOrWhiteSpace(tmp))
            {
                tmp = "tags=" + tmp;
            }

            // page
            if (query.Page.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(tmp))
                {
                    tmp += "&";
                }
                tmp += "page=" + query.Page.Value.ToString();
            }

            // limit
            if (query.Limit.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(tmp))
                {
                    tmp += "&";
                }
                tmp += "limit=" + query.Limit.Value.ToString();
            }
            return tmp;
        }

        public DanbooruSearchParam SearchParam { get; set; }

        public int GetNextPage()
        {
            if (!SearchParam.Page.HasValue) SearchParam.Page = 1;
            return SearchParam.Page.Value + 1;
        }

        public int GetPrevPage()
        {
            if (!SearchParam.Page.HasValue) SearchParam.Page = 1;
            var temp = SearchParam.Page.Value - 1;
            if (temp > 0) return temp;
            else return 1;
        }

        public DanbooruTagCollection parseTagsPage(string data, int page)
        {
            DanbooruTagCollection tagCol = new DanbooruTagCollection();
            List<DanbooruTag> tags = new List<DanbooruTag>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);

            int index = 1 + ((page - 1) * 50);
            // select all tags
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class,'highlightable')]");
            foreach (var table in tables)
            {
                if (!(table.Attributes["class"].Value == "highlightable"))
                {
                    table.Remove();
                    continue;
                }

                var rows = table.SelectNodes("//table[contains(@class,'highlightable')]//tr");
                int countIndex = 1, nameIndex = 3, typeIndex = 9;
                foreach (var row in rows)
                {
                    //if (row.ChildNodes.Count != 11 && row.ChildNodes.Count != 7) continue;
                    var cols = row.ChildNodes;
                    if (cols[1].Name == "th")
                    {
                        for (int i = 0; i < cols.Count; ++i)
                        {
                            if (cols[i].Name == "th")
                            {
                                if (cols[i].InnerText.Replace("\n", "") == "Posts")
                                {
                                    countIndex = i;
                                    continue;
                                }
                                if (cols[i].InnerText.Replace("\n", "") == "Name")
                                {
                                    nameIndex = i;
                                    continue;
                                }
                                if (cols[i].InnerText.Replace("\n", "") == "Type")
                                {
                                    typeIndex = i;
                                    continue;
                                }
                            }
                        }
                        continue;
                    }
                    if (cols[1].Name != "td") continue;

                    DanbooruTag tag = new DanbooruTag();
                    tag.Id = index.ToString();
                    tag.Count = Int32.Parse(cols[countIndex].InnerText);
                    tag.Name = Helper.RemoveControlCharacters(System.Net.WebUtility.HtmlDecode(cols[nameIndex].ChildNodes[3].InnerText.Replace("\n", "")));

                    string tagType = cols[typeIndex].InnerText.Replace("\n", "");
                    if (tagType.EndsWith("(edit)")) tagType = tagType.Substring(0, tagType.Length - 6);
                    tagType = tagType.ToLowerInvariant();
                    if (tagType == "general")
                        tag.Type = DanbooruTagType.General;
                    else if (tagType == "character")
                        tag.Type = DanbooruTagType.Character;
                    else if (tagType == "artist")
                        tag.Type = DanbooruTagType.Artist;
                    else if (tagType == "copyright")
                        tag.Type = DanbooruTagType.Copyright;
                    else if (tagType == "idol")
                        tag.Type = DanbooruTagType.Artist;
                    else if (tagType == "photo_set")
                        tag.Type = DanbooruTagType.Circle;
                    else
                        tag.Type = DanbooruTagType.Faults;

                    tags.Add(tag);
                    ++index;
                }
            }

            tagCol.Tag = tags.ToArray();
            return tagCol;
        }
    }
}