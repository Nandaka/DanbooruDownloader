using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.Entity;
using System.IO;
using System.Net;

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// For constructing filename.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private string MakeFilename(string baseFolder, string format, DanbooruPost post, int limit)
        {
            string filename = format;
            string provider = post.Provider != null ? post.Provider : cbxProvider.Text;
            string query = post.Query != null ? post.Query : txtQuery.Text;
            string searchTags = post.SearchTags != null ? post.SearchTags : "None";

            filename = filename.Replace("%provider%", SanitizeFilename(provider));
            filename = filename.Replace("%id%", post.Id);
            filename = filename.Replace("%tags%", SanitizeFilename(post.Tags));
            filename = filename.Replace("%rating%", post.Rating);
            filename = filename.Replace("%md5%", post.MD5);
            filename = filename.Replace("%query%", SanitizeFilename(query));
            filename = filename.Replace("%searchtag%", SanitizeFilename(searchTags));

            if (baseFolder.EndsWith(@"\")) filename = baseFolder + filename;
            else filename = baseFolder + @"\" + filename;

            filename = filename.Substring(0, filename.Length < limit ? filename.Length : limit);

            string dir = filename.Substring(0, filename.LastIndexOf(@"\"));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            return filename;
        }

        /// <summary>
        /// Sanitize the filename.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string SanitizeFilename(string input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                input = input.Replace(c, '_');
            }
            input = input.Replace(':', '_');
            input = input.Replace('\\', '_');
            return input;
        }

        /// <summary>
        /// Please donate some money.
        /// </summary>
        private void Donate()
        {
            System.Diagnostics.Process.Start(@"https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=Nchek2000%40gmail%2ecom&lc=US&item_name=Nandaka&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted");
        }

        /// <summary>
        /// Pad user agent with current date/time.
        /// </summary>
        /// <returns></returns>
        private string PadUserAgent(string originalUserAgent)
        {
            if (originalUserAgent.Length > 0)
            {
                return originalUserAgent + DateTime.UtcNow;
            }
            else return null;
        }

        /// <summary>
        /// Setting up proxy for all webclient.
        /// </summary>
        private void SetProxy(bool useProxy, string proxyAddress, int port, string username=null, string password=null)
        {
            if (useProxy)
            {
                if (proxyAddress.Length > 0)
                {
                    WebProxy proxy = new WebProxy(proxyAddress, port);
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        proxy.Credentials = new NetworkCredential(username, password);
                    }
                   _clientFile.Proxy = proxy;
                   _clientList.Proxy = proxy;
                   _clientThumb.Proxy = proxy;
                   _clientBatch.Proxy = proxy;
                }
            }
            else _clientFile.Proxy = _clientList.Proxy = _clientThumb.Proxy = _clientBatch.Proxy = null;
            
        }
    }
}
