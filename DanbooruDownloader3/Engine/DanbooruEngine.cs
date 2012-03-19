using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using DanbooruDownloader3.Entity;
using System.Xml.Linq;

namespace DanbooruDownloader3.Engine
{
    public class DanbooruEngine
    {
        public static BindingList<DanbooruPost> ParseXML(XmlReader reader, DanbooruProvider provider, string query, string searchTags) 
        {
            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();
            XDocument doc = XDocument.Load(reader);

            foreach (var item in doc.Descendants("post"))
            {
                DanbooruPost post = new DanbooruPost();
                post.Id = item.Attribute("id").Value;
                post.Source = item.Attribute("source").Value;
                post.CreatorId = item.Attribute("creator_id").Value;
                posts.Add(post);
            }

            return posts;
        }

        public static BindingList<DanbooruPost> ParseJSON(string json, DanbooruProvider provider, string query, string searchTags) 
        {
            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();

            return posts;
        }

    }
}
