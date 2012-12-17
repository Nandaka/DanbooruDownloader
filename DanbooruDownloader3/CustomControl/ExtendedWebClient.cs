using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DanbooruDownloader3.CustomControl
{
    public class ExtendedWebClient : WebClient
    {
        #region ctor
        public ExtendedWebClient(int timeout = -1, CookieContainer cookieJar = null, String userAgent=null)
        {
            if (timeout > 0)
            {
                this.Timeout = timeout;
            }
            else
            {
                bool result = Int32.TryParse(Properties.Settings.Default.Timeout, out timeout);
                if (result) this.Timeout = timeout;
                else this.Timeout = 60000;
            }
            
            if (cookieJar != null)
            {
                // replace old cookie jar
                ExtendedWebClient.CookieJar = cookieJar;
            }

            if (userAgent != null)
            {
                // replace user agent
                this.UserAgent = userAgent;
            }
        }
        #endregion

        private static IWebProxy globalProxy;
        public static IWebProxy GlobalProxy
        {
            get {
                if (Properties.Settings.Default.UseProxy)
                {
                    WebProxy proxy = new WebProxy(Properties.Settings.Default.ProxyAddress, Convert.ToInt32(Properties.Settings.Default.ProxyPort));
                    if (Properties.Settings.Default.UseProxyLogin)
                    {
                        proxy.Credentials = new NetworkCredential(Properties.Settings.Default.ProxyUsername, Properties.Settings.Default.ProxyPassword);
                    }
                    globalProxy = proxy;
                }
                else
                {
                    globalProxy = null;
                }
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
            get {
                if (cookieJar == null)
                {
                    cookieJar = new CookieContainer();
                }
                return cookieJar; 
            }
            set { cookieJar = value; }
        }

        private bool enableCookie;
        public bool EnableCookie
        {
            get{ 
                return this.enableCookie; 
            }
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
            get 
            {
                if (userAgent == null)
                {
                    userAgent = Properties.Settings.Default.UserAgent;
                    if (Properties.Settings.Default.PadUserAgent)
                    {
                        userAgent += new DateTime().Ticks;
                    }
                    this.Headers.Add("user-agent", this.userAgent);
                }                
                return this.userAgent; 
            }
            set
            {
                this.userAgent = value;
                this.Headers.Add("user-agent", this.userAgent);
            }
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
