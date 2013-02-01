using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanbooruDownloader3.Entity
{
    public class DanbooruSearchParam
    {
        public DanbooruProvider Provider { get; set; }

        public string Tag { get; set; }
        public int? Limit { get; set; }
        public string Source { get; set; }
        public int? Page { get; set; }

        public string OrderBy { get; set; }
        public string Rating { get; set; }
        public bool IsNotRating { get; set; }

        public DanbooruPostDaoOption Option { get; set; }

    }
}
