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

    public class DanbooruProvider
    {
        private string name;
        private string url;
        private string queryJson;
        private string queryXml;
        private PreferredMethod preferred;
        private int defaultLimit;
        private int hardLimit;

        public string Name { get{return name;} set{this.name = value;} }

        public int DefaultLimit { get { return defaultLimit; } set { this.defaultLimit = value; } }
        public int HardLimit { get { return hardLimit; } set { this.hardLimit = value; } }

        public PreferredMethod Preferred { get { return preferred; } set { this.preferred = value; } }

        public string QueryStringXml { get { return queryXml; } set { this.queryXml = value; } }

        public string QueryStringJson { get { return queryJson; } set { this.queryJson = value; } }

        public string Url { get { return url; } set { this.url = value; } }


        public override string ToString()
        {
            return "Name: " + name +
                " Url: " + url +
                " QueryJson: " + queryJson +
                " QueryXml: " + queryXml +
                " Preferred: " + preferred +
                " Default Limit: " + defaultLimit;
        }
    }

    
}
