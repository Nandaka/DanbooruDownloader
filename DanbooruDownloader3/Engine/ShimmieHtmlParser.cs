using DanbooruDownloader3.Entity;
using HtmlAgilityPack;
using System;
using System.ComponentModel;

namespace DanbooruDownloader3.Engine
{
    public class ShimmieHtmlParser : IEngine
    {
        public int? TotalPost { get; set; }
        public int? Offset { get; set; }
        public string RawData { get; set; }
        public string ResponseMessage { get; set; }
        public bool Success { get; set; }
        public DanbooruSearchParam SearchParam { get; set; }

        public string GenerateQueryString(DanbooruSearchParam query)
        {
            return ShimmieEngine.GetQueryString(query.Provider, query);
        }

        public int GetNextPage()
        {
            if (!SearchParam.Page.HasValue) SearchParam.Page = 1;

            return SearchParam.Page.Value + 1;
        }

        public int GetPrevPage()
        {
            if (!SearchParam.Page.HasValue) SearchParam.Page = 1;
            var temp = SearchParam.Page.Value - 1;
            if (temp > 0) return temp;
            else return 1;
        }

        public static DanbooruPost ParsePost(DanbooruPost post, string postHtml, bool overideTagParsing)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(postHtml);
                string file_url = "";
                string sample_url = "";

                // image post
                var img = doc.DocumentNode.SelectSingleNode("//img[@id='main_image']");
                if (img != null)
                {
                    sample_url = file_url = Helper.FixUrl(img.GetAttributeValue("src", ""), isHttps(post.Provider), hostname: post.Provider.Url);
                }

                // video post
                var video = doc.DocumentNode.SelectSingleNode("//video[@id='video-id']/source");

                if (video != null)
                {
                    var dl = video.GetAttributeValue("src", "");
                    sample_url = file_url = Helper.FixUrl(dl, isHttps(post.Provider), hostname: post.Provider.Url);
                }

                post.SampleUrl = sample_url;
                post.FileUrl = file_url;
                post.CreatedAt = "N/A";
                post.CreatedAtDateTime = DateTime.MinValue;

                var imgInfo = doc.DocumentNode.SelectNodes("//table[@class='image_info form']");
                foreach (var table in imgInfo)
                {
                    if (table.HasChildNodes)
                    {
                        var time = table.SelectSingleNode("//time");
                        if (time != null)
                        {
                            post.CreatedAt = time.Attributes["datetime"].Value;
                            post.CreatedAtDateTime = DateTime.Parse(post.CreatedAt);
                        }
                    }
                }

                return post;
            }
            catch (Exception ex)
            {
                string filename = $"Dump for Post {post.Id} - {post.Provider.Name} Query {post.Query}.txt";
                bool result = Helper.DumpRawData(postHtml, filename);
                if (!result) Program.Logger.Error($"Failed to dump rawdata to: {filename}", ex);
                throw;
            }
        }

        public BindingList<DanbooruPost> Parse(string data, DanbooruSearchParam query)
        {
            this.RawData = data;
            this.SearchParam = query;

            BindingList<DanbooruPost> posts = new BindingList<DanbooruPost>();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(data);

                // get all thumbs
                var thumbs = doc.DocumentNode.SelectNodes("//div/a[@class='thumb shm-thumb shm-thumb-link ']");
                foreach (var thumb in thumbs)
                {
                    if (thumb.GetAttributeValue("data-post-id", "").Length > 0)
                    {
                        DanbooruPost post = new DanbooruPost();
                        post.Id = thumb.GetAttributeValue("data-post-id", "");
                        post.Provider = query.Provider;
                        post.SearchTags = query.Tag;
                        post.Query = GenerateQueryString(query);
                        post.Referer = Helper.FixUrl(query.Provider.Url + thumb.GetAttributeValue("href", ""), isHttps(post.Provider), hostname: post.Provider.Url);

                        post.Tags = thumb.GetAttributeValue("data-tags", "");
                        post.Tags = Helper.DecodeEncodedNonAsciiCharacters(post.Tags);
                        post.TagsEntity = Helper.ParseTags(post.Tags, query.Provider);
                        post.Hidden = Helper.CheckBlacklistedTag(post, query.Option);

                        var img = thumb.ChildNodes[0];
                        post.PreviewUrl = Helper.FixUrl(img.GetAttributeValue("src", ""), isHttps(post.Provider), hostname: post.Provider.Url);
                        post.PreviewHeight = img.GetAttributeValue("height", 0);
                        post.PreviewWidth = img.GetAttributeValue("width", 0);

                        post.Source = "";
                        post.Score = "";

                        post.Width = Int32.Parse(thumb.GetAttributeValue("data-width", "-1"));
                        post.Height = Int32.Parse(thumb.GetAttributeValue("data-height", "-1"));

                        post.Rating = "";

                        post.CreatorId = "";

                        post.MD5 = "";

                        posts.Add(post);
                    }
                }
                return posts;
            }
            catch (Exception ex)
            {
                var filename = Helper.SanitizeFilename($"Dump for {query.Provider.Name} Html Parser- page {query.Page}.txt");
                var result = Helper.DumpRawData(data, filename);
                if (!result) Program.Logger.Error("Failed to dump rawdata to: " + filename, ex);
                throw;
            }
        }

        private static bool isHttps(DanbooruProvider provider)
        {
            return provider.Url.ToLowerInvariant().StartsWith("https");
        }
    }
}