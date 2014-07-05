using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.Entity;

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
            Helper.ColorUnknown = lblColorUnknown.ForeColor;
        }

        /// <summary>
        /// Enable/disable txtProxyUsername and password
        /// </summary>
        private void CheckProxyLoginInput()
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
        /// Setting up global proxy for webclient.
        /// </summary>
        private void SetProxy(bool useProxy, string proxyAddress, int port, string username = null, string password = null)
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
                    ExtendedWebClient.GlobalProxy = proxy;
                }
            }
            else ExtendedWebClient.GlobalProxy = null;
        }

        private delegate void SetUpdateLogCallback(string source, string message, Exception ex);

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
                if (ex == null) Program.Logger.Info("[" + source + "] " + message);
                else Program.Logger.Error("[" + source + "] " + message, ex);
            }
        }

        /// <summary>
        /// Update status label with the searchParam url
        /// </summary>
        private void UpdateStatus()
        {
            tsStatus.Text = "Query URL: " + GetQueryUrl();
            tsStatus.Text = tsStatus.Text.Replace("&", "&&");
        }

        private delegate void SetUpdateStatusCallback(string message, bool showDialogBox);

        private void UpdateStatus2(string message, bool showDialogBox = false)
        {
            if (statusStrip1.InvokeRequired && statusStrip1.IsHandleCreated)
            {
                SetUpdateStatusCallback d = new SetUpdateStatusCallback(UpdateStatus2);
                this.Invoke(d, new object[] { message, showDialogBox });
            }
            else
            {
                if (IsDisposed && IsHandleCreated) return;
                tsStatus.Text = message;
                if (showDialogBox)
                {
                    ShowMessage("Batch Download", message);
                }
            }
        }

        /// <summary>
        /// Generate searchParam url from text boxes.
        /// </summary>
        /// <param name="authString"></param>
        /// <returns></returns>
        public string GetQueryUrl(string authString = "")
        {
            if (_currProvider == null) return "";
            if (chkGenerate.Checked)
            {
                txtQuery.Text = "";

                // Clean up txtTags
                var tags = txtTags.Text;
                while (tags.Contains("  "))
                {
                    tags = tags.Replace("  ", " ");
                }
                tags = tags.Trim();
                tags = System.Web.HttpUtility.UrlEncode(tags);

                if (_currProvider.BoardType == BoardType.Shimmie2)
                {
                    var page = txtPage.Text;
                    if (String.IsNullOrWhiteSpace(page))
                    {
                        txtPage.Text = "1";
                        page = "1";
                    }

                    txtQuery.Text = tags;
                    if (!String.IsNullOrWhiteSpace(tags)) txtQuery.Text += "/";

                    txtQuery.Text += page;
                }
                else
                {
                    List<string> queryList = new List<string>();
                    List<string> tagsList = new List<string>();

                    //Tags
                    if (tags.Length > 0) tagsList.Add(tags.Replace(' ', '+'));

                    //Rating
                    if (cbxRating.SelectedIndex > 0) tagsList.Add(chkNotRating.Checked ? "-" + cbxRating.SelectedValue : "" + cbxRating.SelectedValue);

                    //Source
                    var source = txtSource.Text.Trim();
                    if (source.Length > 0) tagsList.Add("source:" + source);

                    //Order
                    if (cbxOrder.SelectedIndex > 0) tagsList.Add(cbxOrder.SelectedValue.ToString());

                    if (tagsList.Count > 0) queryList.Add("tags=" + String.Join("+", tagsList));

                    //Limit
                    if (txtLimit.Text.Length > 0) queryList.Add("limit=" + txtLimit.Text);

                    //StartPage
                    if (txtPage.Text.Length > 0)
                    {
                        if (_currProvider.BoardType == BoardType.Danbooru) queryList.Add("page=" + txtPage.Text);
                        else if (_currProvider.BoardType == BoardType.Gelbooru) queryList.Add("pid=" + txtPage.Text);
                    }

                    if (queryList.Count > 0) txtQuery.Text = String.Join("&", queryList);
                }
            }

            string query = "";
            switch (_currProvider.Preferred)
            {
                case PreferredMethod.Xml:
                    query = _currProvider.QueryStringXml;
                    break;

                case PreferredMethod.Json:
                    query = _currProvider.QueryStringJson;
                    break;

                case PreferredMethod.Html:
                    query = _currProvider.QueryStringHtml;
                    break;

                default:
                    break;
            }
            query = query.Replace("%_query%", txtQuery.Text);
            if (!string.IsNullOrWhiteSpace(authString)) query = query + "&" + authString;

            return _currProvider.Url + query;
        }

        public DanbooruSearchParam GetSearchParams()
        {
            DanbooruPostDaoOption option = new DanbooruPostDaoOption()
            {
                BlacklistedTags = TagBlacklist,
                BlacklistedTagsRegex = TagBlacklistRegex,
                BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                IgnoredTags = TagIgnore,
                IgnoredTagsRegex = TagIgnoreRegex,
                IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked,
                Provider = _currProvider,
                SearchTags = txtTags.Text
            };

            DanbooruSearchParam searchParam = new DanbooruSearchParam();

            searchParam.Provider = option.Provider;
            searchParam.Tag = option.SearchTags;
            searchParam.Source = txtSource.Text;

            int tempPage = _currProvider.BoardType == BoardType.Gelbooru ? 0 : 1;
            if (!String.IsNullOrWhiteSpace(txtPage.Text))
            {
                tempPage = Convert.ToInt32(txtPage.Text);
            }
            searchParam.Page = tempPage;
            searchParam.OrderBy = cbxOrder.SelectedValue.ToString();
            searchParam.Rating = cbxRating.SelectedValue.ToString();
            searchParam.IsNotRating = chkNotRating.Checked;
            searchParam.Option = option;

            return searchParam;
        }

        /// <summary>
        /// Flash the taskbar, see: http://social.msdn.microsoft.com/forums/en-US/Vsexpressvcs/thread/237b1d13-d2a5-467b-abc5-d793ce472076
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="bInvert"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public void ShowMessage(string title, string text)
        {
            if (notifyIcon1.Visible)
            {
                notifyIcon1.ShowBalloonTip(5000, title, text, ToolTipIcon.Info);
            }
            else
            {
                if (this.WindowState == FormWindowState.Minimized || !this.Focused) FlashWindow(this.Handle, true);
                MessageBox.Show(this, text, title, MessageBoxButtons.OK);
            }
        }
    }
}