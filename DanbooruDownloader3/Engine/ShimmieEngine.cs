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

        public static BindingList<DanbooruPost> ParseRSS(XmlReader reader, DanbooruPostDaoOption option)
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

                if (option.BlacklistedTagsUseRegex)
                {
                    post.Hidden = IsBlacklisted(post, option.BlacklistedTagsRegex);
                }
                else
                {
                    post.Hidden = IsBlacklisted(post, option.BlacklistedTags);
                }

                post.Referer = AppendHttp(item.Element("link").Value, option.Provider);
                post.CreatedAt = item.Element("pubDate").Value;
                post.Provider = option.Provider.Name;

                var data = item.Element("{" + media + "}thumbnail");
                post.PreviewUrl = AppendHttp(data.Attribute("url").Value, option.Provider);
                data = item.Element("{" + media + "}content");
                post.FileUrl = AppendHttp(data.Attribute("url").Value, option.Provider);

                post.Query = option.Query;
                post.SearchTags = option.SearchTags;

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

        private static bool IsBlacklisted(DanbooruPost post, List<DanbooruTag> TagBlackList)
        {
            if (TagBlackList != null)
            {
                foreach (var tag in TagBlackList)
                {
                    return post.TagsEntity.Any(x => x.Name.Equals(tag.Name, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            return false;
        }

        private static bool IsBlacklisted(DanbooruPost post, Regex regex)
        {
            if (regex != null)
            {
                return post.TagsEntity.Any(x => regex.IsMatch(x.Name));
            }
            return false;
        }
    }
}
