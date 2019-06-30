using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DanbooruDownloader3.Engine
{
    public class GelbooruHtmlParser : IEngine
    {
        public static DanbooruPost ParsePost(DanbooruPost post, string postHtml)
        {
            HtmlDocument doc = new HtmlDocument();
            if (String.IsNullOrEmpty(postHtml)) throw new Exception("No post html!");

            doc.LoadHtml(postHtml);
            string file_url = "";
            string sample_url = "";

            // Flash Game or bmp
            // TODO: need to change the preview url
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
                var image = doc.DocumentNode.SelectSingleNode("//img[@id='image']");
                if (image != null)
                {
                    sample_url = image.GetAttributeValue("src", "");
                }

                var links = doc.DocumentNode.SelectNodes("//a");
                foreach (var link in links)
                {
                    if (link.InnerText == "Original image")
                    {
                        file_url = link.GetAttributeValue("href", "");
                        break;
                    }
                }
                
                // Some Gelbooru sites (*.booru.org) serve full resolution in the img tag
                if (file_url == "")
                {
                    file_url = sample_url;
                }
            }

            post.FileUrl = file_url;
            if (!string.IsNullOrWhiteSpace(file_url) && string.IsNullOrWhiteSpace(sample_url))
                sample_url = file_url;
            post.SampleUrl = sample_url;

            // parse datetime
            // TODO: untested
            var statsLIs = doc.DocumentNode.SelectNodes("//div[@id='stats']//li");
            if (statsLIs != null)
            {
                foreach (var item in statsLIs)
                {
                    if (item.InnerHtml.StartsWith("Posted: "))
                    {
                        post.CreatedAt = System.Text.RegularExpressions.Regex.Match(item.InnerHtml, "Posted: (.*)<").Value;
                        post.CreatedAtDateTime = DanbooruPostDao.ParseDateTime(post.CreatedAt, post.Provider);
                        break;
                    }
                }
            }

            return post;
        }

        public BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam searchParam)
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
                        post.Id = thumb.GetAttributeValue("id", "_N/A").Substring(1);

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
                        post.Referer = searchParam.Provider.Url + "/" + System.Web.HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""));
                        if (post.Id == "N/A")
                        {
                            post.Id = a.GetAttributeValue("id", "N/A").Substring(1);
                        }

                        var img = a.ChildNodes[i];
                        var title = img.GetAttributeValue("title", "");
                        var title2 = title.ToString();
                        post.Tags = title.Substring(0, title.LastIndexOf("rating:")).Trim();
                        post.Tags = Helper.DecodeEncodedNonAsciiCharacters(post.Tags);
                        post.TagsEntity = Helper.ParseTags(post.Tags, SearchParam.Provider);
                        post.Hidden = Helper.CheckBlacklistedTag(post, searchParam.Option);

                        post.PreviewUrl = img.GetAttributeValue("src", "");
                        post.PreviewHeight = img.GetAttributeValue("height", 0);
                        post.PreviewWidth = img.GetAttributeValue("width", 0);

                        post.Source = "";
                        post.Score = title.Substring(title.LastIndexOf("score:") + 6);
                        post.Score = post.Score.Substring(0, post.Score.LastIndexOf(" ")).Trim();

                        title2 = title2.Substring(title2.LastIndexOf("rating:"));
                        post.Rating = title2.Substring(7, 1).ToLower();

                        post.Status = "";

                        post.MD5 = post.PreviewUrl.Substring(post.PreviewUrl.LastIndexOf("/") + 1);
                        post.MD5 = post.MD5.Substring(0, post.MD5.LastIndexOf("."));
                        post.MD5 = post.MD5.Replace("thumbnail_", "");

                        posts.Add(post);
                    }
                }
            }

            TotalPost = posts.Count;
            if (!SearchParam.Page.HasValue) SearchParam.Page = 0;
            Offset = posts.Count * SearchParam.Page;

            return posts;
        }

        public int? TotalPost { get; set; }

        public int? Offset { get; set; }

        public string RawData { get; set; }

        public string ResponseMessage { get; set; }

        public bool Success { get; set; }

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
            if (!SearchParam.Page.HasValue)
            {
                SearchParam.Page = 0;
            }

            if (SearchParam.Provider.Preferred == PreferredMethod.Html)
            {
                return (SearchParam.Page.Value + 1) * TotalPost.Value;
            }

            return (SearchParam.Page.Value + 1);
        }

        public int GetPrevPage()
        {
            if (!SearchParam.Page.HasValue)
            {
                SearchParam.Page = 0;
            }

            if (SearchParam.Provider.Preferred == PreferredMethod.Html)
            {
                var temp = (SearchParam.Page.Value - 1) * TotalPost.Value;
                if (temp < 0) return 0;
                else return temp;
            }
            else
            {
                var temp = (SearchParam.Page.Value - 1);
                if (temp < 0) return 0;
                else return temp;
            }
        }
    }
}