using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DanbooruDownloader3.Entity;
using System.Xml;
using System.ComponentModel;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DanbooruDownloader3.DAO;

namespace DanbooruDownloader3.Engine
{
    public class ShimmieEngine
    {
        private static Regex imageResolutionRegex = new Regex(@"title=.*[\|\||\/\/] (\d+)x(\d+).*width='(\d+)'.*height='(\d+).*Uploaded by (.+)<");

        public static BindingList<DanbooruPost> ParseRSS(XmlReader reader, DanbooruProvider provider, string query, string searchTags, List<DanbooruTag> BlacklistedTag)
        {
            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();
            
            XDocument doc = XDocument.Load(reader);
            string media = doc.Root.Attribute("{http://www.w3.org/2000/xmlns/}media").Value;

            foreach (var item in doc.Descendants("item"))
            {
                DanbooruPost post = new DanbooruPost();

                var titleData = item.Element("title").Value.Split(new char[] { '-' } , 2);

                post.Id = titleData[0].Trim();
                post.Tags = titleData[1].Trim();
                post.TagsEntity = DanbooruTagsDao.Instance.ParseTagsString(post.Tags);
                post.Hidden = CheckBlacklisted(post, BlacklistedTag);

                post.Referer = AppendHttp(item.Element("link").Value, provider);
                post.CreatedAt = item.Element("pubDate").Value;
                post.Provider = provider.Name;

                var data = item.Element("{" + media + "}thumbnail");
                post.PreviewUrl = AppendHttp(data.Attribute("url").Value, provider);
                data = item.Element("{" + media + "}content");
                post.FileUrl = AppendHttp(data.Attribute("url").Value, provider);

                post.Query = query;
                post.SearchTags = searchTags;

                try
                {
                    string description = HttpUtility.HtmlDecode(item.Element("description").Value);
                    Match matches = imageResolutionRegex.Match(description);
                    post.Width = Convert.ToInt32(matches.Groups[1].Value);
                    post.Height = Convert.ToInt32(matches.Groups[2].Value);
                    post.PreviewWidth = Convert.ToInt32(matches.Groups[3].Value);
                    post.PreviewHeight = Convert.ToInt32(matches.Groups[4].Value);
                    post.CreatorId = matches.Groups[5].Value;
                }
                catch (Exception) { }

                posts.Add(post);
            }
            return posts;
        }

        private static string AppendHttp(string url, DanbooruProvider provider) 
        {
            if (!url.StartsWith("http")) url = provider.Url + url;
            return url;
        }

        private static bool CheckBlacklisted(DanbooruPost post, List<DanbooruTag> TagBlackList)
        {
            foreach (var tag in TagBlackList)
            {
                if (post.Tags.Contains(tag.Name)) return true;
            }
            return false;
        }
    }
}
