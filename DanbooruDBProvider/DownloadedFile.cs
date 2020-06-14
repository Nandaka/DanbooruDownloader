using System;

namespace DanbooruDBProvider
{
    public class DownloadedFile
    {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string PostId { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public DateTime Timestamp { get; set; }
    }
}