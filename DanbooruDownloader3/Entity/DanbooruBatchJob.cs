using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DanbooruDownloader3.Entity
{
    [Serializable()]
    public class DanbooruBatchJob
    {
        [Browsable(false)]
        public DanbooruProvider Provider { get; set; }

        public string ProviderName
        {
            get
            {
                return Provider.Name;
            }
            set { }
        }

        public string SaveFolder { get; set; }
        public string TagQuery { get; set; }

        private int _Limit = -1;

        public int Limit
        {
            get
            {
                if (_Limit == -1)
                {
                    _Limit = Provider.DefaultLimit < Provider.HardLimit ? Provider.DefaultLimit : Provider.HardLimit;
                }
                return _Limit;
            }
            set
            {
                _Limit = value;
            }
        }

        [XmlIgnore]
        private string _status;

        [XmlIgnore]
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = $"Downloaded: {Downloaded} Skipped: {Skipped} Processed Total: {ProcessedTotal} Total Available: {Total}{Environment.NewLine}";
                _status += $"Current Page: {CurrentPage} Current Page Total: {CurrentPageTotal } Offset: {CurrentPageOffset}{Environment.NewLine}";
                _status += value;
            }
        }

        [Browsable(false)]
        public bool isCompleted { get; set; }

        [Browsable(false)]
        public bool isError { get; set; }

        public string Rating { get; set; }

        private int _Page = -1;

        public int StartPage
        {
            get
            {
                if (_Page == -1)
                {
                    if (Provider.BoardType == BoardType.Gelbooru)
                    {
                        _Page = 0;
                    }
                    else _Page = 1;
                }
                return _Page;
            }
            set
            {
                _Page = value;
            }
        }

        [XmlIgnore]
        public int Downloaded { get; set; }

        [XmlIgnore]
        public int Skipped { get; set; }

        [XmlIgnore]
        public int Error { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public int Total { get; set; }

        [XmlIgnore]
        public int ProcessedTotal
        {
            get
            {
                return Downloaded + Skipped + Error;
            }
            set { }
        }

        [Browsable(false)]
        [XmlIgnore]
        public int CurrentPage { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public int CurrentPageTotal { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public int CurrentPageOffset { get; set; }

        // Feature #95

        private string filter;

        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        private bool isExclude;

        public bool IsExclude
        {
            get { return isExclude; }
            set { isExclude = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public int ImgCount { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public int PostCount { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public int CalculatedCurrentPage
        {
            get
            {
                if (this.Provider.BoardType == BoardType.Danbooru || this.Provider.BoardType == BoardType.Shimmie2)
                {
                    return this.CurrentPage;
                }
                else if (this.Provider.BoardType == BoardType.Gelbooru)
                {
                    if (this.Provider.Preferred == PreferredMethod.Html)
                    {
                        return this.CurrentPage * this.PostCount;
                    }
                    else
                    {
                        return this.CurrentPage;
                    }
                }
                return this.CurrentPage;
            }
            private set { }
        }

        [Browsable(false)]
        [XmlIgnore]
        public string Query { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public string Url { get; set; }

        [XmlIgnore]
        public string LastFileUrl { get; set; }
    }
}