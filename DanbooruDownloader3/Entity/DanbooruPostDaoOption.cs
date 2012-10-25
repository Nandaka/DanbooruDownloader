using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanbooruDownloader3.Entity
{
    public class DanbooruPostDaoOption
    {
        public DanbooruProvider Provider { get; set; }

        public string Query { get; set; }

        public string SearchTags { get; set; }

        public string Url { get; set; }

        public string Referer { get; set; }

        public bool IsXML { get; set; }

        public List<DanbooruTag> BlacklistedTags { get; set; }

        public System.Text.RegularExpressions.Regex BlacklistedTagsRegex { get; set; }

        public bool BlacklistedTagsUseRegex { get; set; }
    }
}
