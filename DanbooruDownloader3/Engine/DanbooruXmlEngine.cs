using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using DanbooruDownloader3.Entity;
using System.Xml.Linq;
using DanbooruDownloader3.DAO;

namespace DanbooruDownloader3.Engine
{
    /// <summary>
    /// TODO: not used yet
    /// </summary>

    public class DanbooruXmlEngine:IEngine
    {
        public int? TotalPost { get; set; }
        public int? Offset { get; set; }
        public string RawData { get; set; }

        public string ResponseMessage { get; set; }
        public bool Success { get; set; }

        public BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam query)
        {
            BindingList<DanbooruPost> list = new BindingList<DanbooruPost>();
            XDocument posts = XDocument.Parse(data);
            this.RawData = posts.ToString();

            Success = true;
            var responses = posts.Descendants("response");
            if (responses != null && responses.Count() > 0)
            {
                ResponseMessage = responses.First().Attribute("reason").Value.ToString();
                Success = Convert.ToBoolean(responses.First().Attribute("success").Value);
                if (!Success)
                {
                    return null;
                }                
            }

            this.TotalPost = Convert.ToInt32(posts.Root.Attribute("count").Value);
            this.Offset = Convert.ToInt32(posts.Root.Attribute("offset").Value);

            string queryStr = GenerateQueryString(query);

            foreach (var post in posts.Descendants("post"))
            {
                DanbooruPost p = new DanbooruPost();
                p.Id = post.Attribute("id").Value.ToString();
                p.Tags = post.Attribute("tags").Value.ToString();
                p.TagsEntity = DanbooruTagsDao.Instance.ParseTagsString(p.Tags);
                p.Source = post.Attribute("source").Value.ToString();
                p.Score = post.Attribute("score").Value.ToString();
                p.Rating = post.Attribute("rating").Value.ToString();

                p.FileUrl = AppendHttp(post.Attribute("file_url").Value.ToString(), query.Provider);
                p.Width = Convert.ToInt32(post.Attribute("width").Value);
                p.Height = Convert.ToInt32(post.Attribute("height").Value);

                p.PreviewUrl = AppendHttp(post.Attribute("preview_url").Value.ToString(), query.Provider);
                if (post.Attribute("actual_preview_width") != null &&           // yande.re extensions
                    post.Attribute("actual_preview_height") != null)
                {
                    p.PreviewWidth = Convert.ToInt32(post.Attribute("actual_preview_width").Value);
                    p.PreviewHeight = Convert.ToInt32(post.Attribute("actual_preview_height").Value);
                }
                else
                {
                    p.PreviewWidth = Convert.ToInt32(post.Attribute("preview_width").Value);
                    p.PreviewHeight = Convert.ToInt32(post.Attribute("preview_height").Value);
                }

                p.SampleUrl = AppendHttp(post.Attribute("sample_url").Value.ToString(), query.Provider);
                p.SampleWidth = Convert.ToInt32(post.Attribute("sample_width").Value);
                p.SampleHeight = Convert.ToInt32(post.Attribute("sample_height").Value);

                p.Filesize = Convert.ToInt32(post.Attribute("file_size").Value);
                p.Status = post.Attribute("status").Value.ToString();
                p.HasChildren = Convert.ToBoolean(post.Attribute("has_children").Value);
                p.ParentId = post.Attribute("parent_id").Value.ToString();
                p.Change = post.Attribute("change").Value.ToString();
                p.CreatorId = post.Attribute("creator_id").Value.ToString();
                p.CreatedAt = post.Attribute("created_at").Value.ToString();
                p.MD5 = post.Attribute("md5").Value.ToString();

                p.Provider = query.Provider.Name;
                p.Query = queryStr;
                p.SearchTags = query.Tag;
                p.Referer = query.Provider.Url + @"/post/show/" + p.Id;

                list.Add(p);
            }
            return list;
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

        private string AppendHttp(string url, DanbooruProvider provider)
        {
            if (!url.StartsWith("http"))
            {
                return provider.Url + url;
            }
            return url;
        }
    }
}
