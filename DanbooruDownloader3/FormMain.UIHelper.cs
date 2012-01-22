using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.Entity;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace DanbooruDownloader3
{
    /// <summary>
    /// UI Helper
    /// </summary>
    public partial class FormMain : Form
    {
        public void PrintFlags()
        {
            tsFlag.Text = "isPaused = " + _isPaused.ToString() + " | isLoadingList = " + _isLoadingList.ToString() + " | isMorePost = " + _isMorePost.ToString() + " | isLoadingThumb = " + _isLoadingThumb.ToString();
        }

        /// <summary>
        /// Check if dgvDownload is not empty.
        /// </summary>
        /// <returns></returns>
        public bool CheckDownloadGrid()
        {
            return dgvDownload.Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// Check if dgvList is not empty.
        /// </summary>
        /// <returns></returns>
        public bool CheckListGrid()
        {
            return dgvList.Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// Please donate some money :)
        /// </summary>
        private void Donate()
        {
            System.Diagnostics.Process.Start(@"https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=Nchek2000%40gmail%2ecom&lc=US&item_name=Nandaka&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted");
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

        delegate void SetUpdateLogCallback(string source, string message);
        private void UpdateLog(string source, string message)
        {
            if (txtLog.InvokeRequired)
            {
                SetUpdateLogCallback d = new SetUpdateLogCallback(UpdateLog);
                this.Invoke(d, new object[] { source, message });
            }
            else
            {
                txtLog.AppendText("[" + source + "] " + message + Environment.NewLine);
            }
        }
    }
}
