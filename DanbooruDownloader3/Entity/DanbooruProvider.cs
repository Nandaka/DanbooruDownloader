using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanbooruDownloader3.Entity
{
    public enum PreferredMethod
    {
        Xml,
        Json
    }

    public enum BoardType
    {
        Danbooru,
        Gelbooru,
        Shimmie2
    }

    public class DanbooruProvider
    {
        public string Name { get; set; }
        public int DefaultLimit { get; set; }
        public int HardLimit { get; set; }
        public PreferredMethod Preferred { get; set; }
        public string QueryStringXml { get; set; }
        public string QueryStringJson { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseAuth { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public BoardType BoardType { get; set; }

        public override string ToString()
        {
            return "Name: " + Name +
                " Url: " + Url +
                " QueryJson: " + QueryStringJson +
                " QueryXml: " + QueryStringXml +
                " Preferred: " + Preferred +
                " Default Limit: " + DefaultLimit +
                " Type: " + BoardType.ToString();
        }
    }


}
