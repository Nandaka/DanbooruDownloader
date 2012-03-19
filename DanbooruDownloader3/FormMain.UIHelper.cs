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
        /// <summary>
        /// print flags for debug.
        /// </summary>
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

        /// <summary>
        /// Update Log text box.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
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

        /// <summary>
        /// Generate query url from text boxes.
        /// </summary>
        /// <param name="authString"></param>
        /// <returns></returns>
        public string GetQueryUrl(string authString = "")
        {
            if (chkGenerate.Checked)
            {
                txtQuery.Text = "";
                if (_currProvider.BoardType == BoardType.Shimmie2 )
                {
                    txtQuery.Text += txtTags.Text;
                    if (!String.IsNullOrWhiteSpace(txtTags.Text)) txtQuery.Text += "/";

                    if (String.IsNullOrWhiteSpace(txtPage.Text)) txtPage.Text = "1";
                    txtQuery.Text += txtPage.Text;
                }
                else
                {                    
                    //Tags
                    txtQuery.Text += txtTags.Text.Length > 0 ? "tags=" + txtTags.Text.Replace(' ', '+') : "";

                    //Rating
                    txtQuery.Text += txtQuery.Text.Length > 0 && cbxRating.SelectedIndex > 0 ? "+" : "";
                    txtQuery.Text += (txtTags.Text.Length <= 0 && cbxRating.SelectedIndex > 0 ? "tags=" : "") + (chkNotRating.Checked && cbxRating.SelectedIndex > 0 ? "-" : "" + cbxRating.SelectedValue);

                    //Source
                    txtQuery.Text += txtQuery.Text.Length > 0 && txtSource.Text.Length > 0 ? "+" : txtSource.Text.Length > 0 ? "tags=" : "";
                    txtQuery.Text += txtSource.Text.Length > 0 ? "source:" + txtSource.Text : "";

                    //Order
                    txtQuery.Text += txtQuery.Text.Length > 0 && cbxOrder.SelectedIndex > 0 ? "+" : "";
                    txtQuery.Text += cbxOrder.SelectedValue;

                    //Limit
                    txtQuery.Text += txtQuery.Text.Length > 0 && txtLimit.Text.Length > 0 ? "&" : "";
                    txtQuery.Text += txtLimit.Text.Length > 0 ? "limit=" + txtLimit.Text : "";

                    //Page
                    txtQuery.Text += txtQuery.Text.Length > 0 && txtPage.Text.Length > 0 ? "&" : "";
                    if (_currProvider.BoardType == BoardType.Danbooru)
                    {
                        txtQuery.Text += txtPage.Text.Length > 0 ? "page=" + txtPage.Text : "";
                    }
                    else if (_currProvider.BoardType == BoardType.Gelbooru)
                    {
                        txtQuery.Text += txtPage.Text.Length > 0 ? "pid=" + txtPage.Text : "";
                    }
                }
            }

            string query = (rbJson.Checked ? _currProvider.QueryStringJson : _currProvider.QueryStringXml);
            query = query.Replace("%_query%", txtQuery.Text);
            if (!string.IsNullOrWhiteSpace(authString)) query = query + "&" + authString;

            return _currProvider.Url + query;
        }

    }
}
