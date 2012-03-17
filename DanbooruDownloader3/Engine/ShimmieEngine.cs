using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DanbooruDownloader3.Entity;
using System.Xml;
using System.ComponentModel;
using System.Xml.Linq;

namespace DanbooruDownloader3.Engine
{
    public class ShimmieEngine
    {

        public static BindingList<DanbooruPost> Parse(XmlReader reader, DanbooruProvider provider, string query, string searchTags)
        {
            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();
            
            XDocument doc = XDocument.Load(reader);
            
            foreach (var item in doc.Descendants("item"))
            {
                DanbooruPost post = new DanbooruPost();

                var titleData = item.Element("title").Value.Split(new char[] { '-' } , 2);

                post.Id = titleData[0];
                post.Tags = titleData[1];

                post.Referer = item.Element("link").Value;
                if (!post.Referer.Contains("http://")) post.Referer = provider.Url + post.Referer;

                post.Provider = provider.Name;

                var data = item.Element("{http://search.yahoo.com/mrss}thumbnail");
                post.PreviewUrl = provider.Url + data.Attribute("url").Value;
                data = item.Element("{http://search.yahoo.com/mrss}content");
                post.FileUrl = provider.Url + data.Attribute("url").Value;

                post.Query = query;
                post.SearchTags = searchTags;

                posts.Add(post);
            }
            return posts;
        }
    }
}
