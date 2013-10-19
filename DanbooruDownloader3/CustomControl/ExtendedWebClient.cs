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
        public ExtendedWebClient(int timeout = -1, CookieContainer cookieJar = null, String userAgent = null)
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
            get
            {
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
            set
            {
                globalProxy = value;
            }
        }

        private int timeout;
        public int Timeout
        {
            get { return this.timeout; }
            set
            {
                if (value < 0) value = 0;
                this.timeout = value;
            }
        }

        private static bool enableCookie;
        public static bool EnableCookie
        {
            get
            {
                return enableCookie;
            }
            set
            {
                if (value && cookieJar == null)
                {
                    cookieJar = new CookieContainer();
                }
                enableCookie = value;
            }
        }

        public static bool EnableCompression { get; set; }

        private static string _acceptLanguage;
        public static string AcceptLanguage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_acceptLanguage)) _acceptLanguage = "en-GB,en-US;q=0.8,en;q=0.6";
                return _acceptLanguage;
            }
            set { _acceptLanguage = value; }
        }

        private static CookieContainer cookieJar;
        public static CookieContainer CookieJar
        {
            get
            {
                if (cookieJar == null)
                {
                    cookieJar = new CookieContainer();
                }
                return cookieJar;
            }
            private set { cookieJar = value; }
        }

        private string referer;
        public string Referer
        {
            get { return this.referer; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    this.referer = value;
                    this.Headers.Add("Referer", this.referer);
                }
                else
                {
                    this.Headers.Remove("Referer");
                }
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
                }
                if (Properties.Settings.Default.PadUserAgent)
                {
                    return Helper.PadUserAgent(userAgent);
                }
                else
                {
                    return this.userAgent;
                }
            }
            set
            {
                this.userAgent = value;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            this.Headers.Add("user-agent", UserAgent);

            WebRequest req = base.GetWebRequest(address);

            var httpReq = req as HttpWebRequest;
            if (httpReq != null)
            {
                if (enableCookie)
                {
                    httpReq.CookieContainer = cookieJar;
                }
                if (EnableCompression)
                {
                    httpReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                    httpReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                }
                httpReq.Headers.Add(HttpRequestHeader.AcceptLanguage, AcceptLanguage);
            }

            req.Timeout = this.timeout;
            req.Proxy = globalProxy;

            return req;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                WebResponse response = base.GetWebResponse(request, result);
                ReadCookies(response);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            try
            {
                WebResponse response = base.GetWebResponse(request);
                ReadCookies(response);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null && cookieJar != null)
            {
                CookieCollection cookies = response.Cookies;
                cookieJar.Add(cookies);
            }
        }
    }
}
