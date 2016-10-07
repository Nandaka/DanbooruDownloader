using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace DanbooruDownloader3.Engine
{
    public class ShimmieEngine
    {
        private static Regex imageResolutionRegex = new Regex(@"title=.*[\|\||\/\/] (\d+)x(\d+).*width='(\d+)'.*height='(\d+).*Uploaded by (.+)<");
        private static Regex thumbSrc = new Regex("src=\"(.*)\"");

        public static BindingList<DanbooruPost> ParseRSS(string xmldoc, DanbooruPostDaoOption option)
        {
            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();

            try
            {
                ReadRssMethod1(option, posts, xmldoc);
            }
            catch (Exception ex)
            {
                Program.Logger.Error("Using method2", ex);
                ReadRssMethod2(option, posts, xmldoc);
            }
            return posts;
        }

        private static void PostProcess(DanbooruPostDaoOption option, DanbooruPost post)
        {
            post.TagsEntity = Helper.ParseTags(post.Tags, option.Provider);

            if (option.BlacklistedTagsUseRegex)
            {
                post.Hidden = IsBlacklisted(post, option.BlacklistedTagsRegex);
            }
            else
            {
                post.Hidden = IsBlacklisted(post, option.BlacklistedTags);
            }

            post.Query = option.Query;
            post.SearchTags = option.SearchTags;
            post.Provider = option.Provider;
            post.CreatedAtDateTime = DanbooruPostDao.ParseDateTime(post.CreatedAt, option.Provider);
        }

        private static void ReadRssMethod2(DanbooruPostDaoOption option, BindingList<DanbooruPost> posts, string xmldoc)
        {
            using (StringReader strReader = new StringReader(xmldoc))
            {
                using (XmlReader reader = new XmlTextReader(strReader))
                {
                    XDocument doc = XDocument.Load(reader);
                    var feeds = doc.Descendants("item");

                    XNamespace dc = "http://purl.org/dc/elements/1.1/";

                    foreach (var item in feeds)
                    {
                        DanbooruPost post = new DanbooruPost();
                        var titleData = item.Element("title").Value.Split(new char[] { '-' }, 2);

                        post.Id = titleData[0].Trim();
                        post.Tags = titleData[1].Trim();

                        post.Referer = AppendHttp(item.Element("link").Value, option.Provider);
                        post.CreatedAt = item.Element("pubDate").Value;

                        post.CreatorId = item.Element(dc + "creator").Value;

                        post.FileUrl = item.Element("enclosure").Attribute("url").Value;
                        try
                        {
                            HtmlAgilityPack.HtmlDocument description = new HtmlAgilityPack.HtmlDocument();
                            description.LoadHtml(HttpUtility.HtmlDecode(item.Element("description").Value));

                            //post.Width = Convert.ToInt32(matches.Groups[1].Value);
                            //post.Height = Convert.ToInt32(matches.Groups[2].Value);
                            var img = description.DocumentNode.SelectSingleNode("//img");
                            post.PreviewWidth = Convert.ToInt32(img.Attributes["width"].Value);
                            post.PreviewHeight = Convert.ToInt32(img.Attributes["height"].Value);
                            post.PreviewUrl = img.Attributes["src"].Value;
                        }
                        catch (Exception) { }

                        PostProcess(option, post);
                        posts.Add(post);
                    }
                }
            }
        }

        private static void ReadRssMethod1(DanbooruPostDaoOption option, BindingList<DanbooruPost> posts, string xmldoc)
        {
            using (StringReader strReader = new StringReader(xmldoc))
            {
                using (XmlReader reader = new XmlTextReader(strReader))
                {
                    XDocument doc = XDocument.Load(reader);
                    string media = doc.Root.Attribute("{http://www.w3.org/2000/xmlns/}media").Value;

                    foreach (var item in doc.Descendants("item"))
                    {
                        DanbooruPost post = new DanbooruPost();

                        var titleData = item.Element("title").Value.Split(new char[] { '-' }, 2);

                        post.Id = titleData[0].Trim();
                        post.Tags = titleData[1].Trim();

                        post.Referer = AppendHttp(item.Element("link").Value, option.Provider);
                        post.CreatedAt = item.Element("pubDate").Value;

                        var data = item.Element("{" + media + "}thumbnail");
                        post.PreviewUrl = AppendHttp(data.Attribute("url").Value, option.Provider);
                        data = item.Element("{" + media + "}content");
                        post.FileUrl = AppendHttp(data.Attribute("url").Value, option.Provider);

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

                        PostProcess(option, post);
                        posts.Add(post);
                    }
                }
            }
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

        public static string GetQueryString(DanbooruProvider provider, DanbooruSearchParam query)
        {
            var queryStr = "";

            // Clean up txtTags
            var tags = query.Tag;
            while (tags.Contains("  "))
            {
                tags = tags.Replace("  ", " ");
            }
            tags = tags.Trim();
            tags = System.Web.HttpUtility.UrlEncode(tags);

            //StartPage
            var page = 1;
            if (query.Page.HasValue && query.Page > 0)
            {
                page = query.Page.Value;
            }

            queryStr = tags;
            if (!String.IsNullOrWhiteSpace(tags)) queryStr += "/";
            queryStr += page;

            return queryStr;
        }
    }
}