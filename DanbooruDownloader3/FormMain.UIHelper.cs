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
        /// Enable/Disable colored tags.
        /// </summary>
        private void ToggleTagsColor()
        {
            if (chkUseTagColor.Checked)
            {
                dgvList.Columns["colTagsE"].Visible = true;
                dgvList.Columns["colTags"].Visible = false;
            }
            else
            {
                dgvList.Columns["colTagsE"].Visible = false;
                dgvList.Columns["colTags"].Visible = true;
            }
            dgvList.Refresh();
        }

        /// <summary>
        /// Set color for tags.
        /// </summary>
        private void SetTagColors()
        {
            Helper.ColorGeneral = lblColorGeneral.ForeColor;
            Helper.ColorArtist = lblColorArtist.ForeColor;
            Helper.ColorCopyright = lblColorCopy.ForeColor;
            Helper.ColorCharacter = lblColorChara.ForeColor;
            Helper.ColorCircle = lblColorCircle.ForeColor;
            Helper.ColorFaults = lblColorFaults.ForeColor;
            Helper.ColorBlacklisted = lblColorBlacklistedTag.ForeColor;
        }

        /// <summary>
        /// Enable/disable txtProxyUsername and password
        /// </summary>
        private void CheckProxyLogin()
        {
            if (chkProxyLogin.Checked)
            {
                txtProxyPassword.Enabled = true;
                txtProxyUsername.Enabled = true;
            }
            else
            {
                txtProxyPassword.Enabled = false;
                txtProxyUsername.Enabled = false;
            }
        }

        /// <summary>
        /// Enable/Disable Main group box
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableControls(bool enabled)
        {
            gbxSearch.Enabled = enabled;
            gbxList.Enabled = enabled;
            gbxDanbooru.Enabled = enabled;
        }

        /// <summary>
        /// Enable/Disable Download tab buttons
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableDownloadControls(bool enabled)
        {
            btnClearDownloadList.Enabled = enabled;
            btnLoadDownloadList.Enabled = enabled;            
            btnDownloadFiles.Enabled = enabled;

            btnCancelDownload.Enabled = !enabled;
            btnPauseDownload.Enabled = !enabled;
        }

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

        delegate void SetUpdateLogCallback(string source, string message, Exception ex);

        /// <summary>
        /// Update Log text box.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        private void UpdateLog(string source, string message, Exception ex = null)
        {
            if (txtLog.InvokeRequired && txtLog.IsHandleCreated)
            {
                SetUpdateLogCallback d = new SetUpdateLogCallback(UpdateLog);
                this.Invoke(d, new object[] { source, message, ex });
            }
            else
            {
                if (IsDisposed) return;
                if (txtLog.TextLength > Int32.MaxValue / 2) txtLog.Clear();
                txtLog.AppendText("[" + source + "] " + message + Environment.NewLine);

                message = Helper.RemoveAuthInfo(message);
                if(ex == null) Program.Logger.Info("[" + source + "] " + message);
                else           Program.Logger.Error("[" + source + "] " + message, ex);
            }
        }

        /// <summary>
        /// Update status label with the query url
        /// </summary>
        private void UpdateStatus()
        {
            tsStatus.Text = "Query URL: " + GetQueryUrl();
            tsStatus.Text = tsStatus.Text.Replace("&", "&&");
        }

        delegate void SetUpdateStatusCallback(string message);
        private void UpdateStatus2(string message)
        {
            if (statusStrip1.InvokeRequired && statusStrip1.IsHandleCreated) 
            {
                SetUpdateStatusCallback d = new SetUpdateStatusCallback(UpdateStatus2);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                if (IsDisposed) return;
                tsStatus.Text = message;
            }
        }

        /// <summary>
        /// Generate query url from text boxes.
        /// </summary>
        /// <param name="authString"></param>
        /// <returns></returns>
        public string GetQueryUrl(string authString = "")
        {
            if (_currProvider == null) return "";
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
                    if (cbxRating.SelectedIndex > 0)
                    {
                        txtQuery.Text += txtQuery.Text.Length > 0 ? "+" : "tags=";
                        txtQuery.Text += chkNotRating.Checked ? "-" + cbxRating.SelectedValue : "" + cbxRating.SelectedValue;
                    }

                    //Source
                    txtQuery.Text += txtQuery.Text.Length > 0 && txtSource.Text.Length > 0 ? "+" : txtSource.Text.Length > 0 ? "tags=" : "";
                    txtQuery.Text += txtSource.Text.Length > 0 ? "source:" + txtSource.Text : "";

                    //Order
                    if (cbxOrder.SelectedIndex > 0)
                    {
                        txtQuery.Text += txtQuery.Text.Length > 0 ? "+" : "tags=";
                        txtQuery.Text += cbxOrder.SelectedValue;
                    }

                    //Limit
                    txtQuery.Text += txtQuery.Text.Length > 0 && txtLimit.Text.Length > 0 ? "&" : "";
                    txtQuery.Text += txtLimit.Text.Length > 0 ? "limit=" + txtLimit.Text : "";

                    //StartPage
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

        public DanbooruSearchParam GetSearchParams()
        {
            DanbooruSearchParam searchParam = new DanbooruSearchParam();

            searchParam.Provider = _currProvider;
            searchParam.Tag = txtTags.Text;
            searchParam.Source = txtSource.Text;

            int tempPage = _currProvider.BoardType == BoardType.Gelbooru ? 0 : 1;
            if(!String.IsNullOrWhiteSpace(txtPage.Text)) 
            {
                tempPage = Convert.ToInt32(txtPage.Text);
            }
            searchParam.Page = tempPage;
            searchParam.OrderBy = cbxOrder.SelectedValue.ToString();
            searchParam.Rating = cbxRating.SelectedValue.ToString();
            searchParam.IsNotRating = chkNotRating.Checked;
            searchParam.IsXML = rbXml.Checked;
            searchParam.BlacklistedTag = TagBlacklist;

            return searchParam;
        }

        public void ToggleLogging(bool enable)
        {
            if (enable)
            {
                Program.Logger.Logger.Repository.Threshold = log4net.Core.Level.All;
                Program.Logger.Info("Turning on logging");
            }
            else
            {
                Program.Logger.Info("Turning off logging");
                Program.Logger.Logger.Repository.Threshold = log4net.Core.Level.Off;
            }
        }
    }
}
