using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DanbooruDownloader3.Entity
{
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
            private set { }
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

        public string Status { get; set; }

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

        public int Downloaded { get; set; }
        public int Skipped { get; set; }
        public int Error { get; set; }
        
        [Browsable(false)]
        public int Total { get; set; }
        
        public int ProcessedTotal
        {
            get
            {
                return Downloaded + Skipped + Error;
            }
            private set { }
        }

        [Browsable(false)]
        public int CurrentPage { get; set; }
        [Browsable(false)]
        public int CurrentPageTotal { get; set; }
        [Browsable(false)]
        public int CurrentPageOffset { get; set; }
    }
}
