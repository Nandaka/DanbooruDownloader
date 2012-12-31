using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using DanbooruDownloader3.Entity;
using System.ComponentModel;
using DanbooruDownloader3.DAO;

namespace DanbooruDownloader3.Engine
{
    public class SankakuComplexParser : IEngine
    {
        public static DanbooruPost ParsePost(DanbooruPost post, string postHtml)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(postHtml);
            string file_url = "";
            string sample_url = "";

            // Flash Game or bmp
            if (post.PreviewUrl == "http://chan.sankakucomplex.com/download-preview.png")
            {
                var links = doc.DocumentNode.SelectNodes("//a");
                foreach (var link in links)
                {
                    // flash
                    if (link.InnerText == "Save this flash (right click and save)")
                    {
                        file_url = link.GetAttributeValue("href", "");
                        break;
                    }
                    // bmp
                    if (link.InnerText == "Download")
                    {
                        file_url = link.GetAttributeValue("href", "");
                        break;
                    }
                }
            }
            else
            {
                var lowresElement = doc.DocumentNode.SelectSingleNode("//a[@id='lowres']");
                if (lowresElement != null)
                {
                    sample_url = lowresElement.GetAttributeValue("href", "");
                }
                var highresElement = doc.DocumentNode.SelectSingleNode("//a[@id='highres']");
                if (highresElement != null)
                {
                    file_url = highresElement.GetAttributeValue("href", "");
                }                
            }

            post.FileUrl = file_url;
            if (!string.IsNullOrWhiteSpace(file_url) && string.IsNullOrWhiteSpace(sample_url))
                sample_url = file_url;
            post.SampleUrl = sample_url;
            return post;
        }

        public BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam query)
        {
            RawData = data;

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

                        post.Provider = query.Provider.Name;
                        post.SearchTags = query.Tag;
                        post.Query = GenerateQueryString(query);

                        var a = thumb.ChildNodes[0];
                        post.Referer = query.Provider.Url + a.GetAttributeValue("href", "");

                        var img = a.ChildNodes[0];
                        var title = img.GetAttributeValue("title", "");
                        var title2 = title.ToString();
                        post.Tags = title.Substring(0, title.LastIndexOf("rating:")).Trim();
                        post.Tags = Helper.DecodeEncodedNonAsciiCharacters(post.Tags);
                        post.TagsEntity = DanbooruTagsDao.Instance.ParseTagsString(post.Tags);

                        post.PreviewUrl = img.GetAttributeValue("src", "");
                        post.PreviewHeight = img.GetAttributeValue("height", 0);
                        post.PreviewWidth = img.GetAttributeValue("width", 0);

                        post.Source = "";
                        post.Score = title.Substring(title.LastIndexOf("score:") + 6);
                        post.Score = post.Score.Substring(0, post.Score.LastIndexOf(" ")).Trim();

                        title2 = title2.Substring(title2.LastIndexOf("rating:"));
                        post.Rating = title2.Substring(7, 1).ToLower();

                        post.Status = "";
                        
                        posts.Add(post);
                    }
                }
            }


            return posts;
        }


        public int? TotalPost
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

        public int? Offset
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
    }
}
