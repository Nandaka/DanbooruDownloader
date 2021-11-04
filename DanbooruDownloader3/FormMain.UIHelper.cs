using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
                dgvDownload.Columns["colTagsE2"].Visible = true;
                dgvDownload.Columns["colTags2"].Visible = false;
            }
            else
            {
                dgvList.Columns["colTagsE"].Visible = false;
                dgvList.Columns["colTags"].Visible = true;
                dgvDownload.Columns["colTagsE2"].Visible = false;
                dgvDownload.Columns["colTags2"].Visible = true;
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

        private void UpdateStatus2(string message, bool isCompleted = false)
        {
            if (statusStrip1.InvokeRequired && statusStrip1.IsHandleCreated)
            {
                SetUpdateStatusCallback d = new SetUpdateStatusCallback(UpdateStatus2);
                this.Invoke(d, new object[] { message, isCompleted });
            }
            else
            {
                if (IsDisposed && IsHandleCreated) return;
                tsStatus.Text = message;
                if (Properties.Settings.Default.ShutdownAfterCompleteBatchJob)
                {
                    Helper.Shutdown();
                }
                else if (isCompleted)
                {
                    ShowMessage("Batch Download", message);
                }
            }
        }

        /// <summary>
        /// Generate searchParam url from main tab.
        /// </summary>
        /// <param name="authString"></param>
        /// <returns></returns>
        public string GetQueryUrl()
        {
            if (_currProvider == null) return "";

            DanbooruSearchParam searchParam = GetSearchParams();
            if (_postsDao != null)
                searchParam.NextKey = _postsDao.NextId;

            if (chkGenerate.Checked)
            {
                txtQuery.Text = _currProvider.GetQueryString(searchParam);
            }

            string query = _currProvider.GetQueryUrl(searchParam);

            return query;
        }

        /// <summary>
        /// Get search parameter from Main Tab and Option Panels
        /// </summary>
        /// <returns></returns>
        public DanbooruSearchParam GetSearchParams()
        {
            var option = new DanbooruPostDaoOption()
            {
                BlacklistedTags = TagBlacklist,
                BlacklistedTagsRegex = TagBlacklistRegex,
                BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                IgnoredTags = TagIgnore,
                IgnoredTagsRegex = TagIgnoreRegex,
                IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked,
                Provider = _currProvider,
                SearchTags = !String.IsNullOrWhiteSpace(txtTags.Text) ? txtTags.Text : "",
                IsBlacklistOnlyForGeneral = chkBlacklistOnlyGeneral.Checked
            };

            DanbooruSearchParam searchParam = new DanbooruSearchParam();

            searchParam.Provider = option.Provider;
            searchParam.Tag = option.SearchTags;
            searchParam.Source = txtSource.Text.Trim();

            int limit = 0;
            if (Int32.TryParse(txtLimit.Text, out limit) && limit > 0) searchParam.Limit = limit;
            else searchParam.Limit = null;

            int page = _currProvider.BoardType == BoardType.Gelbooru ? 0 : 1;
            if (Int32.TryParse(txtPage.Text, out page) && page > 0) searchParam.Page = page;
            else searchParam.Page = null;

            searchParam.IsNotRating = chkNotRating.Checked;
            if (cbxRating.SelectedValue != null)
            {
                if (cbxRating.SelectedValue.GetType() == typeof(string))
                    searchParam.Rating = cbxRating.SelectedValue.ToString();
                else
                {
                    var rating = (KeyValuePair<string, string>)cbxRating.SelectedValue;
                    searchParam.Rating = rating.Value;
                }
            }

            if (cbxOrder.SelectedValue != null)
            {
                if (cbxOrder.SelectedValue.GetType() == typeof(string))
                    searchParam.OrderBy = cbxOrder.SelectedValue.ToString();
                else
                {
                    var order = (KeyValuePair<string, string>)cbxOrder.SelectedValue;
                    searchParam.OrderBy = order.Value;
                }
            }

            searchParam.Option = option;

            searchParam.NextKey = _lastId;

            return searchParam;
        }

        /// <summary>
        /// Get Search Param from Batch Job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public DanbooruSearchParam GetSearchParamsFromJob(DanbooruBatchJob job, int currPage)
        {
            var option = new DanbooruPostDaoOption()
            {
                BlacklistedTags = TagBlacklist,
                BlacklistedTagsRegex = TagBlacklistRegex,
                BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                IgnoredTags = TagIgnore,
                IgnoredTagsRegex = TagIgnoreRegex,
                IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked,
                Provider = _currProvider,
                SearchTags = !String.IsNullOrWhiteSpace(job.TagQuery) ? job.TagQuery : "",
                IsBlacklistOnlyForGeneral = chkBlacklistOnlyGeneral.Checked
            };

            DanbooruSearchParam searchParam = new DanbooruSearchParam();

            searchParam.Provider = option.Provider;
            searchParam.Tag = option.SearchTags;
            searchParam.Source = "";

            // check if given limit is more than the hard limit
            if (job.Limit > job.Provider.HardLimit) searchParam.Limit = job.Provider.HardLimit;
            else searchParam.Limit = job.Limit;

            // reflect to current page
            searchParam.Page = job.StartPage + currPage;

            searchParam.IsNotRating = false;
            searchParam.Rating = job.Rating;

            searchParam.OrderBy = "";

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

        /// <summary>
        /// Show message box or baloon message (if minimized to systray)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
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

        public string ResolveInnerExceptionMessages(Exception ex)
        {
            var message = ex.Message;

            while (true)
            {
                var inner = ex.InnerException;

                if (inner == null) break;
                message += " ---> " + inner.Message;
                ex = inner;
            }
            return message;
        }
    }
}