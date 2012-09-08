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
    }
}
