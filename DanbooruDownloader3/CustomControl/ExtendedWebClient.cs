using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DanbooruDownloader3.CustomControl
{
    public class ExtendedWebClient : WebClient
    {
        new public IWebProxy Proxy
        {
            get { return GlobalProxy; }
            private set { this.Proxy = GlobalProxy; }
        }

        private static IWebProxy globalProxy;
        public static IWebProxy GlobalProxy
        {
            get {
                return globalProxy; 
            }
            set {
                globalProxy = value;
            }
        }

        private int timeout;
        public int Timeout
        {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        private static CookieContainer cookieJar;
        public static CookieContainer CookieJar
        {
            get { return cookieJar; }
            set { cookieJar = value; }
        }

        private bool enableCookie;
        public bool EnableCookie
        {
            get{return this.enableCookie;}
            set { this.enableCookie = value; }
        }

        private string referer;
        public string Referer
        {
            get { return this.referer; }
            set
            {
                this.referer = value;
                this.Headers.Add("Referer", this.referer);
            }
        }

        private string userAgent;
        public string UserAgent
        {
            get { return this.userAgent; }
            set
            {
                this.userAgent = value;
                this.Headers.Add("user-agent", this.userAgent);
            }
        }

        public ExtendedWebClient(){
            Timeout = 60000;
            cookieJar = new CookieContainer();
        }

        public ExtendedWebClient(int timeout){
            Timeout = timeout;
            cookieJar = new CookieContainer();
        }

        public ExtendedWebClient(CookieContainer cookieJar)
        {
            CookieJar = cookieJar;
            Timeout = 60000;
        }

        protected override WebRequest GetWebRequest(Uri address) 
        { 
            WebRequest result = base.GetWebRequest(address);

            if (result.GetType() == typeof(HttpWebRequest))
            {
                if (enableCookie) ((HttpWebRequest)result).CookieContainer = cookieJar;
            }   

            result.Timeout = this.timeout;
            result.Proxy = globalProxy;

            return result; 
        } 

    }
}
