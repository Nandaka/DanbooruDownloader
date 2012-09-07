using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using DanbooruDownloader3.Entity;

namespace DanbooruDownloader3.Engine
{
    public class SankakuComplexParser
    {
        public static DanbooruPost ParsePost(DanbooruPost post, string postHtml)
        {
            string file_url = "";
            string preview_url = "";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(postHtml);
            var lowresElement = doc.DocumentNode.SelectSingleNode("//a[@id='lowres']");
            if (lowresElement != null)
            {
                preview_url = lowresElement.GetAttributeValue("href", "");
            }
            var highresElement = doc.DocumentNode.SelectSingleNode("//a[@id='highres']");
            if (highresElement != null)
            {
                file_url = highresElement.GetAttributeValue("href", "");
            }

            post.FileUrl = file_url;
            if (!string.IsNullOrWhiteSpace(file_url) && string.IsNullOrWhiteSpace(preview_url)) 
                preview_url = file_url;
            post.PreviewUrl = preview_url;

            return post;
        }
    }
}
