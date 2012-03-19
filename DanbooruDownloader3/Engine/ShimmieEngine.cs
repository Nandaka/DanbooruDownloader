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

namespace DanbooruDownloader3.Engine
{
    public class ShimmieEngine
    {
        private static Regex imageResolutionRegex = new Regex(@"title=.*\|\| (\d+)x(\d+).*width='(\d+)'.*height='(\d+)");

        public static BindingList<DanbooruPost> ParseRSS(XmlReader reader, DanbooruProvider provider, string query, string searchTags)
        {
            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();
            
            XDocument doc = XDocument.Load(reader);
            string media = doc.Root.Attribute("{http://www.w3.org/2000/xmlns/}media").Value;

            foreach (var item in doc.Descendants("item"))
            {
                DanbooruPost post = new DanbooruPost();

                var titleData = item.Element("title").Value.Split(new char[] { '-' } , 2);

                post.Id = titleData[0];
                post.Tags = titleData[1];

                post.Referer = item.Element("link").Value;
                if (!post.Referer.Contains("http://")) post.Referer = provider.Url + post.Referer;

                post.Provider = provider.Name;

                var data = item.Element("{" + media + "}thumbnail");//("{http://search.yahoo.com/mrss}thumbnail");
                post.PreviewUrl = provider.Url + data.Attribute("url").Value;
                data = item.Element("{" + media + "}content");
                post.FileUrl = provider.Url + data.Attribute("url").Value;

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
                }
                catch (Exception) { }

                posts.Add(post);
            }
            return posts;
        }
    }
}
