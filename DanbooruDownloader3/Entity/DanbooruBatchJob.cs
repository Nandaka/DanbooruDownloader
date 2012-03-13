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
        public List<DanbooruProvider> ProviderList { get; set; }

        private string providerListString;
        public string ProviderListString
        {
            get
            {
                this.providerListString = "";
                foreach (DanbooruProvider p in ProviderList) this.providerListString += p.Name + Environment.NewLine;
                this.providerListString = this.providerListString.Substring(0, providerListString.Length -2);
                return this.providerListString;
            }
            set
            {
                providerListString = value;
            }
        }
            
        public string SaveFolder { get; set; }
        public string TagQuery { get; set; }
        public int Limit { get; set; }

        public string Status { get; set; }

        [Browsable(false)]
        public bool isCompleted { get; set; }

        [Browsable(false)]
        public bool isError { get; set; }

        public string Rating { get; set; }
        public int Page { get; set; }
    }
}
