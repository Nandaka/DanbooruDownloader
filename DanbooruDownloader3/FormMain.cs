using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        #region property

        private static XmlSerializer postSerializer = new XmlSerializer(typeof(BindingList<DanbooruPost>));
        private static XmlSerializer jobSerializer = new XmlSerializer(typeof(BindingList<DanbooruBatchJob>));

        private List<DanbooruProvider> _listProvider;
        private DanbooruProvider _currProvider;
        private int _currCount;
        private DanbooruPostDao _postsDao;

        private BindingList<DanbooruPost> _downloadList;

        private ExtendedWebClient _clientList;
        private ExtendedWebClient _clientThumb;
        private ExtendedWebClient _clientFile;
        private ExtendedWebClient _clientBatch;

        private bool _isPaused = false;
        private bool _isLoadingList = false;
        private bool _isLoadingThumb = false;
        private bool _isDownloading = false;
        private bool _isCanceled = false;

        private bool _isMorePost = true;

        private int _loadedThumbnail;
        private bool _resetLoadedThumbnail = false;
        private int _retry;
        private int _delay;
        private string _lastId; // Issue #111, used in main tab.

        public const int MAX_FILENAME_LENGTH = 255;

        private List<DanbooruTag> TagBlacklist = new List<DanbooruTag>();
        private Regex TagBlacklistRegex = new Regex("$^");
        private List<DanbooruTag> TagIgnore = new List<DanbooruTag>();
        private Regex TagIgnoreRegex = new Regex("$^");

        #endregion property

        public FormMain()
        {
            if (Properties.Settings.Default.UpdateRequired)
            {
                Program.Logger.Info("Upgrading configuration");
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateRequired = false;
                Properties.Settings.Default.Save();
            }

            InitializeComponent();

            // Get assembly version
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += fvi.ProductVersion;
#if  DEBUG
            this.Text += " DEBUG";
#endif
            Program.Logger.Info("Starting up " + this.Text);

            #region init webclients

            _clientList = new ExtendedWebClient();
            _clientList.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientList_DownloadProgressChanged);
            _clientList.DownloadFileCompleted += new AsyncCompletedEventHandler(clientList_DownloadFileCompleted);
            _clientList.DownloadDataCompleted += new DownloadDataCompletedEventHandler(clientList_DownloadDataCompleted);

            _clientThumb = new ExtendedWebClient();
            _clientThumb.DownloadDataCompleted += new DownloadDataCompletedEventHandler(clientThumb_DownloadDataCompleted);
            _clientThumb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientThumb_DownloadProgressChanged);

            _clientFile = new ExtendedWebClient();
            _clientFile.DownloadFileCompleted += new AsyncCompletedEventHandler(clientFile_DownloadFileCompleted);
            _clientFile.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientFile_DownloadProgressChanged);

            _clientBatch = new ExtendedWebClient();
            //_clientBatch.DownloadProgressChanged += new DownloadProgressChangedEventHandler(_clientBatch_DownloadProgressChanged);
            //_clientBatch.DownloadFileCompleted += new AsyncCompletedEventHandler(_clientBatch_DownloadFileCompleted);

            #endregion init webclients

            LoadProviderList();

            //Auto populate Order and Rating
            cbxOrder.DataSource = new BindingSource(Constants.OrderBy, null);
            cbxOrder.DisplayMember = "Key";
            cbxOrder.ValueMember = "Value";
            cbxRating.DataSource = new BindingSource(Constants.Rating, null);
            cbxRating.DisplayMember = "Key";
            cbxRating.ValueMember = "Value";

            txtFilenameHelp.Text = "%provider% = provider Name" + Environment.NewLine +
                                    "%id% = Image ID" + Environment.NewLine +
                                    "%tags% = Image Tags" + Environment.NewLine +
                                    "%rating% = Image Rating" + Environment.NewLine +
                                    "%md5% = MD5 Hash" + Environment.NewLine +
                                    "%artist% = Artist Tag" + Environment.NewLine +
                                    "%copyright% = Copyright Tag" + Environment.NewLine +
                                    "%character% = Character Tag" + Environment.NewLine +
                                    "%circle% = Circle Tag" + Environment.NewLine +
                                    "%faults% = Faults Tag" + Environment.NewLine +
                                    "%general% = General Tag" + Environment.NewLine +
                                    "%originalFilename% = Original Filename" + Environment.NewLine +
                                    "%searchtag% = Search tag" + Environment.NewLine +
                                    "%uploadDateTime% = Upload Date Time";

            pbLoading.Image = DanbooruDownloader3.Properties.Resources.AJAX_LOADING;
            _retry = Convert.ToInt32(txtRetry.Text);

            CheckProxyLoginInput();
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);

            SetTagColors();

            //SetTagAutoComplete();

            ParseTagBlacklist();

            Program.Logger.Debug(this.Text + " loaded.");

            dgvList.AutoGenerateColumns = false;
            dgvDownload.AutoGenerateColumns = false;

            _ImageSize = cbxImageSize.Text;

            ExtendedWebClient.EnableCookie = Properties.Settings.Default.enableCookie;
            ExtendedWebClient.EnableCompression = Properties.Settings.Default.EnableCompression;
            ExtendedWebClient.AcceptLanguage = Properties.Settings.Default.AcceptLanguage;

            UpdateImageSizeOption();
            ToggleTagsColor();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!Helper.IsTagsXmlExist())
            {
                string message = "No tags.xml, need to download!";
                MessageBox.Show(message, "No tags.xml Error");
                Program.Logger.Warn(message);
                btnUpdate_Click(sender, e);
            }

            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Show();
            }
            if (chkMinimizeTray.Checked) notifyIcon1.Visible = true;
            else notifyIcon1.Visible = false;
        }

        #region method

        /// <summary>
        /// Populate the provider List
        /// </summary>
        private void LoadProviderList()
        {
            Program.Logger.Debug("Loading provider list.");
            _listProvider = DanbooruProviderDao.GetInstance().Read();

            cbxProvider.DataSource = null;
            cbxProvider.DataSource = _listProvider;
            cbxProvider.DisplayMember = "Name";
            cbxProvider.ValueMember = "Preferred";

            Program.Logger.Debug("Provider list loaded.");
        }

        /// <summary>
        /// Parse blacklisted tags
        /// </summary>
        private void ParseTagBlacklist()
        {
            var tagStr = txtTagBlacklist.Text.Trim();
            if (!String.IsNullOrWhiteSpace(tagStr))
            {
                TagBlacklist = DanbooruTagsDao.Instance.ParseTagsString(tagStr.Replace(Environment.NewLine, " "));
                try
                {
                    TagBlacklistRegex = new Regex(tagStr.Trim().Replace(Environment.NewLine, "|").Replace(" ", "|"), RegexOptions.IgnoreCase);
                }
                catch (Exception ex)
                {
                    Program.Logger.Debug(ex.Message);
                    TagBlacklistRegex = new Regex("$^");
                }
            }
            else
            {
                if (TagBlacklist != null) TagBlacklist.Clear();
                if (TagBlacklistRegex != null) TagBlacklistRegex = new Regex("$^");
            }
        }

        /// <summary>
        /// Copy checked row from dgvList to _downloadList
        /// </summary>
        public void TransferDownloadRows()
        {
            if (CheckListGrid())
            {
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    // get the check box status.
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colCheck"];
                    bool chkValue = (chk != null && chk.Value != null && true == (bool)chk.Value);

                    if (_downloadList == null) _downloadList = new BindingList<DanbooruPost>();

                    if (chkValue == true)
                    {
                        var post = _postsDao.Posts[row.Index];
                        if (_downloadList.IndexOf(post) < 0)
                        {
                            TransferDownloadRow(post);
                            row.Cells["colCheck"].Value = false;
                        }
                    }
                    dgvDownload.DataSource = _downloadList;
                }
            }
        }

        private void TransferDownloadRow(DanbooruPost post)
        {
            // try to get the file url if empty only for sankaku
            // Fix issue #130
            if (string.IsNullOrWhiteSpace(post.FileUrl) &&
                (post.Status != "deleted" || chkProcessDeletedPost.Checked) &&
                !string.IsNullOrWhiteSpace(post.Referer) &&
                post.Provider.Name.ToLower().Contains("sankaku"))
            {
                ResolveFileUrl(post);
            }

            // reset status
            post.Completed = false;
            // resolve filename
            if (String.IsNullOrWhiteSpace(post.Filename) && !String.IsNullOrWhiteSpace(post.FileUrl))
            {
                post.Filename = MakeCompleteFilename(post, post.FileUrl);
            }
            if (!String.IsNullOrWhiteSpace(post.Filename) && File.Exists(post.Filename))
            {
                post.Progress = "File exists!";
            }

            _downloadList.Add(post);
        }

        private Queue<DanbooruPost> resolveQueue = new Queue<DanbooruPost>();
        private bool isResolverRunning = false;

        private void ResolveFileUrl(DanbooruPost post)
        {
            resolveQueue.Enqueue(post);
            post.FileUrl = Constants.LOADING_URL;
            if (!isResolverRunning) ResolveFileUrl();
        }

        private void ResolveFileUrl()
        {
            if (resolveQueue.Count > 0)
            {
                isResolverRunning = true;
                var post = resolveQueue.Dequeue();
                UpdateLog("SankakuComplexParser", "Trying to resolve: " + post.Referer);
                ExtendedWebClient _clientPost = new ExtendedWebClient
                {
                    Encoding = System.Text.Encoding.UTF8
                };
                _clientPost.DownloadStringAsync(new Uri(post.Referer), post);
                _clientPost.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_clientPost_DownloadStringCompleted);
            }
            else
            {
                isResolverRunning = false;
            }
        }

        private void _clientPost_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            DanbooruPost post = (DanbooruPost)e.UserState;
            if (post.Provider.BoardType == BoardType.Danbooru)
            {
                // Fix #130
                if (e.Error == null && post.Provider.Name.ToLower().Contains("sankaku"))
                {
                    string html = e.Result;
                    post = SankakuComplexParser.ParsePost(post, html, !chkUseGlobalProviderTags.Checked);
                    UpdateLog("SankakuComplexParser", "Resolved to file_url: " + post.FileUrl);
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "File url resolved!";
                    post.Filename = MakeCompleteFilename(post, post.FileUrl);
                }
                else
                {
                    if (post.Provider.Name.ToLower().Contains("sankaku"))
                    {
                        dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "SankakuComplexParser " + e.Error.Message;
                        UpdateLog("SankakuComplexParser", "Unable to get file_url for: " + post.Referer + " ==> " + e.Error.Message, e.Error);
                    }
                    else
                    {
                        dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "Failed to get Image Url, restricted tags?";
                        UpdateLog("SankakuComplexParser", "Unable to get file_url for: " + post.Referer + " ==> Restricted tags?");
                    }

                    post.FileUrl = "";
                }
            }
            else if (post.Provider.BoardType == BoardType.Gelbooru)
            {
                if (e.Error == null)
                {
                    string html = e.Result;
                    post = GelbooruHtmlParser.ParsePost(post, html);
                    UpdateLog("GelbooruHtmlParser", "Resolved to file_url: " + post.FileUrl);
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "File url resolved!";
                    post.Filename = MakeCompleteFilename(post, post.FileUrl);
                }
                else
                {
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "GelbooruHtmlParser " + e.Error.Message;
                    UpdateLog("GelbooruHtmlParser", "Unable to get file_url for: " + post.Referer + " ==> " + e.Error.Message, e.Error);
                    post.FileUrl = "";
                }
            }
            else if (post.Provider.BoardType == BoardType.Shimmie2 && post.Provider.Preferred == PreferredMethod.Html)
            {
                if (e.Error == null)
                {
                    string html = e.Result;
                    post = ShimmieHtmlParser.ParsePost(post, html, !chkUseGlobalProviderTags.Checked);
                    UpdateLog("ShimmieHtmlParser", $"Resolved to file_url: {post.FileUrl}");
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "File url resolved!";
                    post.Filename = MakeCompleteFilename(post, post.FileUrl);
                }
                else
                {
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = $"ShimmieHtmlParser {e.Error.Message}";
                    UpdateLog("SankakuComplexParser", $"Unable to get file_url for: {post.Referer} ==> {e.Error.Message}", e.Error);
                }
            }
            else
            {
                post.FileUrl = Constants.NO_POST_PARSER;
            }
            dgvDownload.Refresh();
            if (isResolverRunning) ResolveFileUrl();
        }

        /// <summary>
        /// Async, will be called from event handler client file download.
        /// </summary>
        /// <param name="i"> downloadList index</param>
        public void DownloadRows(DataGridViewRow row)
        {
            if (_isCanceled) return;

            if (CheckDownloadGrid())
            {
                _isDownloading = true;

                // Update the progress bar
                tsProgress2.Visible = true;
                tsProgress2.Maximum = row.DataGridView.Rows.Count;
                tsProgress2.Value = row.Index > tsProgress2.Maximum ? tsProgress2.Maximum : row.Index + 1;
                tsProgressBar.Style = ProgressBarStyle.Continuous;

                if (_isPaused)
                {
                    DialogResult result = MessageBox.Show("Paused." + Environment.NewLine + "Click OK to continue.", "Download", MessageBoxButtons.OKCancel);
                    _isPaused = false;
                    btnPauseDownload.Enabled = true;

                    if (result.Equals(DialogResult.Cancel))
                    {
                        EnableDownloadControls(true);
                        tsStatus.Text = "Canceled.";
                        _isDownloading = false;
                        return;
                    }
                }

                tsStatus.Text = "Downloading Post #" + row.Index;

                DanbooruPost post = _downloadList[row.Index];

                if (!post.Completed)
                {
                    row.Selected = true;

                    if (chkAutoFocus.Checked) dgvDownload.FirstDisplayedScrollingRowIndex = row.Index;

                    string url = (string)row.Cells["colUrl2"].Value;
                    Uri uri = null;

                    // optimize for sankaku, as only post id is checked, so better check early.
                    var skip = false;
                    if (chkDBIfExists.Checked)
                    {
                        var result = Program.DB.GetDownloadedFileByProviderAndPostId(post.Provider.Name, post.Id.ToString());
                        if (result.Count > 0)
                        {
                            skip = true;
                            row.Cells["colProgress2"].Value = "File exists in DB!";
                            UpdateLog("[DownloadRow]", "File exists DB: " + result[0].Path + "\\" + result[0].Filename);
                        }
                    }
                    if (!skip)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                        {
                            ResolveFileUrl(post);
                        }

                        if (url == Constants.LOADING_URL)
                        {
                            // still loading post url
                            row.Cells["colProgress2"].Value = "Still loading post url, try again later.";
                            UpdateLog("[DownloadRow]", "Still loading post url, try again later.: " + row.Index);
                        }
                        else if (url == Constants.NO_POST_PARSER)
                        {
                            // no parser post url
                            row.Cells["colProgress2"].Value = "No post parser for provider: " + post.Provider;
                            UpdateLog("[DownloadRow]", "No post parser for provider: " + post.Provider + " at : " + row.Index);
                        }
                        else if (post.Status == "deleted" && !chkProcessDeletedPost.Checked)
                        {
                            row.Cells["colProgress2"].Value = "Post is deleted.";
                            UpdateLog("[DownloadRow]", "Post is deleted for row: " + row.Index);
                        }
                        else if (!string.IsNullOrWhiteSpace(url) &&
                                 Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri)) // check if post active and url valid
                        {
                            if (post.Provider == null) post.Provider = _listProvider[cbxProvider.SelectedIndex];
                            if (post.Query == null) post.Query = txtQuery.Text;
                            if (post.SearchTags == null) post.SearchTags = txtTags.Text;

                            string filename = MakeCompleteFilename(post, url);

                            if (chkOverwrite.Checked || !File.Exists(filename))
                            {
                                string dir = filename.Substring(0, filename.LastIndexOf(@"\"));
                                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                                row.Cells["colFilename"].Value = filename;
                                // check if there is existing temporary file then delete it.
                                var filename2 = filename + ".!tmp";
                                if (File.Exists(filename2))
                                {
                                    row.Cells["colProgress2"].Value = "Deleting temporary file: " + filename2;
                                    File.Delete(filename2);
                                }

                                // the actual download
                                _clientFile.Referer = post.Referer;
                                Program.Logger.Info("[DownloadRow] Downloading " + url);
                                Program.Logger.Info("[DownloadRow] Saved to    " + filename);
                                row.Cells["colDownloadStart2"].Value = DateTime.Now;
                                _clientFile.DownloadFileAsync(uri, filename2, row);
                                txtLog.AppendText("[clientFileDownload] Saving to " + filename2 + Environment.NewLine);
                                return;
                            }
                            else
                            {
                                // File exist and overwrite is no checked.
                                row.Cells["colProgress2"].Value = "File exists!";
                                UpdateLog("[DownloadRow]", "File exists: " + filename);
                            }
                        }
                    }
                    else
                    {
                        // no valid url
                        row.Cells["colProgress2"].Value = "No valid file_url, probably deleted.";
                        UpdateLog("[DownloadRow]", "No valid file_url for row: " + row.Index);
                    }
                }

                // proceed with the next row
                if (row.Index < dgvDownload.Rows.GetLastRow(DataGridViewElementStates.None))
                {
                    DownloadRows(dgvDownload.Rows[row.Index + 1]);
                }
                else
                {
                    // no more row
                    _isPaused = false;
                    _isDownloading = false;
                    ShowMessage("Download List", "Download Complete!");
                    EnableDownloadControls(true);
                    tsProgress2.Visible = false;
                }
            }
        }

        /// <summary>
        /// Load post from given DanbooruPostDao to _postsDao
        /// if chkAppendList, the post will be appended to current _postsDao
        /// </summary>
        /// <param name="postDao"></param>
        private void LoadList(DanbooruPostDao postDao)
        {
            Program.Logger.Debug("Loading list");
            if (postDao.Posts.Count > 0)
            {
                _isLoadingList = true;

                if (_postsDao != null)
                {
                    if (chkAppendList.Checked || chkAutoLoadNext.Checked)
                    {
                        UpdateLog("[LoadList]", String.Format("Appending {2} posts: {0} Offset: {1}", postDao.Option.SearchTags, postDao.Offset, postDao.Posts.Count));

                        int oldCount = _postsDao.Posts.Count;

                        foreach (DanbooruPost po in postDao.Posts)
                        {
                            _postsDao.Posts.Add(po);
                        }

                        _lastId = postDao.NextId;
                        dgvList.DataSource = _postsDao.Posts;

                        if (chkLoadPreview.Checked && !_clientThumb.IsBusy && !_isLoadingThumb) LoadThumbnailLater(oldCount);

                        if (chkAutoLoadNext.Checked)
                        {
                            tsCount.Text = "| Count = " + (oldCount + _postsDao.Posts.Count);
                            tsTotalCount.Text = "| Total Count = " + _postsDao.PostCount;
                            _currCount = postDao.Posts.Count; // only the new post
                        }
                    }
                    else
                    {
                        _postsDao = postDao;
                        dgvList.DataSource = _postsDao.Posts;
                        if (chkLoadPreview.Checked && !_clientThumb.IsBusy && !_isLoadingThumb) LoadThumbnailLater(0);
                    }
                }
                else
                {
                    _postsDao = postDao;
                    dgvList.DataSource = _postsDao.Posts;
                    if (chkLoadPreview.Checked && !_clientThumb.IsBusy && !_isLoadingThumb) LoadThumbnailLater(0);
                }

                tsCount.Text = "| Count = " + _postsDao.Posts.Count;
                tsTotalCount.Text = "| Total Count = " + postDao.PostCount;
                _currCount = postDao.Posts.Count;
                _isLoadingList = false;
            }
            else
            {
                _isMorePost = false;
                _isLoadingList = false;
                ShowMessage("Main", "No Posts!");
            }
        }

        /// <summary>
        /// Load thumbnails
        /// </summary>
        /// <param name="i"></param>
        private void LoadThumbnailLater(int i)
        {
            if (this.IsDisposed || this.Disposing) return;
            if (i >= _postsDao.Posts.Count)
            {
                _isLoadingThumb = false;
                return;
            }
            // check whether the previous thread still running.
            if (!_clientThumb.IsBusy)
            {
                if (_resetLoadedThumbnail)
                {
                    _resetLoadedThumbnail = false;
                    _isLoadingThumb = false;
                    _loadedThumbnail = 0;
                    _clientThumb.CancelAsync();
                    return;
                }
                _isLoadingThumb = true;
                tsProgressBar.Visible = true;
                _loadedThumbnail = i;
                timGifAnimation.Enabled = true;
                _clientThumb.Referer = _postsDao.Posts[i].Referer;

                if (!String.IsNullOrWhiteSpace(_postsDao.Posts[_loadedThumbnail].PreviewUrl))
                {
                    _clientThumb.DownloadDataAsync(new Uri(_postsDao.Posts[_loadedThumbnail].PreviewUrl), _loadedThumbnail);
                    txtLog.AppendText("[clientThumbnail] Downloading thumbnail from " + _postsDao.Posts[_loadedThumbnail].PreviewUrl + Environment.NewLine);
                }
                else
                {
                    txtLog.AppendText("[clientThumbnail] No url for thumbnails for post: " + _postsDao.Posts[_loadedThumbnail].Id + Environment.NewLine);
                    if (_postsDao.Posts.Count > i)
                        LoadThumbnailLater(++i);
                }
            }
        }

        /// <summary>
        /// Download posts list
        /// </summary>
        private void GetList()
        {
            var queryUrl = GetQueryUrl();
            Program.Logger.Info("Getting list: " + Helper.RemoveAuthInfo(queryUrl));
            var provider = _listProvider[cbxProvider.SelectedIndex];
            if (provider.LoginType == LoginType.Cookie ||
                provider.LoginType == LoginType.CookieAlwaysAsk)
            {
                _clientList.Headers.Remove("Cookie");
                _clientList.Headers.Add("Cookie", provider.UserName);
            }

            string referer = provider.Url;

            if (chkSaveQuery.Checked)
            {
                saveFileDialog1.FileName = String.Format("{0} - {1} {2}.{3}",
                                                         cbxProvider.Text,
                                                         txtTags.Text,
                                                         txtPage.Text,
                                                         _currProvider.Preferred.ToString().ToLower());
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gbxSearch.Enabled = false;
                    gbxList.Enabled = false;

                    tsProgressBar.Value = 0;
                    _clientList.Referer = referer;
                    _clientList.DownloadFileAsync(new Uri(queryUrl), saveFileDialog1.FileName, saveFileDialog1.FileName.Clone());
                    tsProgressBar.Visible = true;
                }
            }
            else
            {
                // check whether the previous thread is over.
                if (!_clientList.IsBusy)
                {
                    tsProgressBar.Value = 0;
                    _clientList.Referer = referer;
                    _clientList.DownloadDataAsync(new Uri(queryUrl), queryUrl);

                    tsProgressBar.Visible = true;
                    if (chkLoadPreview.Checked &&
                        !chkAutoLoadNext.Checked &&
                        !chkAppendList.Checked &&
                        _clientThumb.IsBusy)
                        _resetLoadedThumbnail = true;
                }
            }
        }

        /// <summary>
        /// Increase txtPage and get the list.
        /// </summary>
        /// <param name="currRowIndex"></param>
        private void AutoLoadNextPage(int currRowIndex)
        {
            if (_postsDao != null && _isMorePost && chkAutoLoadNext.Checked)
            {
                if (currRowIndex >= dgvList.Rows.Count - 1)
                {
                    if (!_isLoadingList && !_clientList.IsBusy)
                    {
                        tsStatus.Text = "Loading next page...";
                        doGetNextPage();
                    }
                }
            }
        }

        #endregion method

        #region batch job helper

        private Thread batchJobThread;
        private ManualResetEvent _shutdownEvent = null;// = new ManualResetEvent(false);
        private ManualResetEvent _pauseEvent = null;// = new ManualResetEvent(true);
        private BindingList<DanbooruBatchJob> batchJob;

        public void PauseBatchJobs()
        {
            UpdateLog("[Batch Job]", "Pausing");
            UpdateStatus2("Pausing...");
            _pauseEvent.Reset();
        }

        public void ResumeBatchJobs()
        {
            UpdateLog("[Batch Job]", "Resuming");
            UpdateStatus2("Resuming...");
            _pauseEvent.Set();
        }

        public void StopBatchJobs()
        {
            Program.Logger.Debug("[Batch Job] Stopping");
            // Signal the shutdown event
            _shutdownEvent.Set();

            // Make sure to resume any paused threads
            _pauseEvent.Set();
        }

        private FormAddBatchJob f;

        private void btnAddBatchJob_Click(object sender, EventArgs e)
        {
            if (f == null)
            {
                f = new FormAddBatchJob(_listProvider);
            }
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (batchJob == null)
                {
                    batchJob = new BindingList<DanbooruBatchJob>();
                }
                foreach (var job in f.Jobs)
                {
                    batchJob.Add(job);
                }
                dgvBatchJob.DataSource = batchJob;
                foreach (DataGridViewRow row in dgvBatchJob.Rows)
                {
                    // Fix Issue #100
                    row.Cells["colBatchId"].Value = row.Index + 1;
                }
            }
        }

        private void btnStartBatchJob_Click(object sender, EventArgs e)
        {
            _shutdownEvent = new ManualResetEvent(false);
            _pauseEvent = new ManualResetEvent(true);

            batchJobThread = new Thread(DoBatchJob);
            batchJobThread.Start(batchJob);
            ToggleBatchJobButton(false);
        }

        private void btnPauseBatchJob_Click(object sender, EventArgs e)
        {
            if (btnPauseBatchJob.Text == "Pause Batch Job")
            {
                btnPauseBatchJob.Text = "Resume Batch Job";
                PauseBatchJobs();
            }
            else
            {
                btnPauseBatchJob.Text = "Pause Batch Job";
                ResumeBatchJobs();
            }
        }

        private void btnStopBatchJob_Click(object sender, EventArgs e)
        {
            if (batchJobThread != null && batchJobThread.IsAlive)
            {
                StopBatchJobs();
                btnStopBatchJob.Enabled = false;
                tsStatus.Text = "Stopping Batch Jobs...";
                //batchJobThread.Abort();
                //ToggleBatchJobButton(true);
                //dgvBatchJob.Refresh();
            }
        }

        public void DoBatchJob(object list)
        {
            DoBatchJob((BindingList<DanbooruBatchJob>)list);
        }

        public void DoBatchJob(BindingList<DanbooruBatchJob> jobs)
        {
            ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
            UpdateUiDelegate del = new UpdateUiDelegate(UpdateUi);
            UpdateUiDelegate2 del2 = new UpdateUiDelegate2(UpdateUi);
            ExtendedWebClient _clientPost = new ExtendedWebClient();

            if (jobs == null || jobs.Count() == 0)
            {
                return;
            }

            UpdateStatus2("Starting Batch Job");

            for (int i = 0; i < jobs.Count; i++)
            {
                var job = jobs[i];

                if (job.isCompleted)
                {
                    UpdateLog("DoBatchJob", $"Skipping Batch Job#{i} because marked as complete.");
                    continue;
                }

                UpdateLog("DoBatchJob", $"Processing Batch Job#{i}");

                DanbooruPostDao prevDao = null;
                DanbooruPostDao d = null;

                bool flag = true;
                job.CurrentPage = 0;
                job.PostCount = 0;

                do
                {
                    // stop/pause event handling outside
                    _pauseEvent.WaitOne(Timeout.Infinite);
                    if (_shutdownEvent.WaitOne(0))
                    {
                        job.Status = " ==> Stopped.";
                        // toggle button
                        BeginInvoke(bjd, new object[] { true });
                        UpdateLog("DoBatchJob", "Batch Job Stopped.");
                        UpdateStatus2("Batch Job Stopped.");
                        return;
                    }

                    job.ImgCount = 0;
                    job.Url = "";
                    job.Query = "";

                    #region Construct the searchParam

                    DanbooruSearchParam searchParam = GetSearchParamsFromJob(job);
                    if (prevDao != null)
                        searchParam.NextKey = prevDao.NextId;
                    job.Url = job.Provider.GetQueryUrl(searchParam);

                    #endregion Construct the searchParam

                    try
                    {
                        #region Get and load the image list

                        job.Status = $"Getting list for page: {searchParam.Page}";
                        BeginInvoke(del);
                        UpdateLog("DoBatchJob", $"Downloading list: {job.Url}");

                        d = GetBatchImageList(job, searchParam.Page);

                        #endregion Get and load the image list

                        if (d == null)
                        {
                            // Cannot get list.
                            UpdateLog("DoBatchJob", "Cannot load list");
                            job.Status = "Cannot load list.";
                            job.isCompleted = false;
                            job.isError = true;
                            flag = false;
                        }
                        else if (d.Posts == null || d.Posts.Count == 0)
                        {
                            // No more image
                            UpdateLog("DoBatchJob", "No more image.");
                            job.Status = "No more image.";
                            flag = false;
                        }
                        else
                        {
                            //lastId =
                            ProcessBatchJobPosts(prevDao, d, ref flag, job, _clientPost);
                        }
                        job.Status = " ==> Done.";
                    }
                    catch (Exception ex)
                    {
                        var message = HandleBatchJobException(ex, ref flag, job);

                        if (cbxAbortOnError.Checked)
                        {
                            MessageBox.Show(message, "Batch Download");
                            break;
                        }
                    }
                    finally
                    {
                        BeginInvoke(del);
                        {
                            // Update progress bar
                            object[] myArray = new object[2];
                            myArray[0] = job.ProcessedTotal;
                            if (d != null)
                            {
                                myArray[1] = d.PostCount < job.Limit ? d.PostCount : job.Limit;
                            }
                            else
                            {
                                myArray[1] = -1;
                            }
                            BeginInvoke(del2, myArray);
                        }
                    }
                    ++job.CurrentPage;
                } while (flag);

                if (d != null)
                {
                    d.Posts.Clear();
                    d = null;
                }
                if (prevDao != null)
                {
                    prevDao.Posts.Clear();
                    prevDao = null;
                }

                // Fix issue #99: memory leak
                // might increase cpu usage (reparse tags.xml).
                DanbooruTagsDao.Instance = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                UpdateLog("DoBatchJob", $"Batch Job #{i}: Done");
                if (job.isError)
                {
                    job.isCompleted = false;
                }
                else
                {
                    job.isCompleted = true;
                }
                BeginInvoke(del);
            }

            BeginInvoke(bjd, new object[] { true });
            UpdateStatus2("Batch Job Completed!", true);
            {
                // hide progress bar
                object[] myArray = new object[2];
                myArray[0] = -1;
                myArray[1] = -1;
                BeginInvoke(del2, myArray);
            }
        }

        private string HandleBatchJobException(Exception ex, ref bool flag, DanbooruBatchJob job)
        {
            string message = ResolveInnerExceptionMessages(ex);
            string responseMessage = "";
            message += Environment.NewLine + $"Stack Trace: {Environment.NewLine}{ex.StackTrace}";
            message += Environment.NewLine + $"Query: {job.TagQuery}";

            job.isError = true;
            job.isCompleted = false;

            if (ex.GetType() == typeof(System.Net.WebException))
            {
                System.Net.WebException wex = (System.Net.WebException)ex;

                if (wex.Status == WebExceptionStatus.ProtocolError &&
                    wex.Response.Headers.AllKeys.Contains("Status") &&
                    wex.Response.Headers["Status"].ToString() == "500")
                {
                    using (var response = wex.Response.GetResponseStream())
                    {
                        if (response != null)
                        {
                            var option = new DanbooruPostDaoOption()
                            {
                                Provider = _currProvider,
                                Query = job.Query,
                                SearchTags = job.TagQuery,
                                Url = job.Url,
                                Referer = "",
                                BlacklistedTags = TagBlacklist,
                                BlacklistedTagsRegex = TagBlacklistRegex,
                                BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                                IgnoredTags = TagIgnore,
                                IgnoredTagsRegex = TagIgnoreRegex,
                                IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked,
                                IsBlacklistOnlyForGeneral = chkBlacklistOnlyGeneral.Checked
                            };
                            var resp = new DanbooruPostDao(response, option, job.CalculatedCurrentPage);
                            responseMessage = resp.ResponseMessage;
                            flag = false;
                        }
                    }
                }
            }
            if (ex.Message.Contains("(400)") ||
                ex.Message.Contains("(403)") ||
                ex.Message.Contains("(500)") ||
                ex.Message.Contains("resolved"))
            {
                flag = false;
            }

            job.Status = $" ==> Error: {(string.IsNullOrWhiteSpace(responseMessage) ? ex.Message : responseMessage)}{Environment.NewLine}";
            if (!string.IsNullOrWhiteSpace(responseMessage))
            {
                UpdateLog("DoBatchJob", $"Server Message: {responseMessage}", ex);
            }
            else
            {
                UpdateLog("DoBatchJob", $"Error: {ex}", ex);
            }

            return message;
        }

        private void ProcessBatchJobPosts(DanbooruPostDao prevDao, DanbooruPostDao d, ref bool flag, DanbooruBatchJob job, ExtendedWebClient clientPost)
        {
            ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
            UpdateUiDelegate del = new UpdateUiDelegate(UpdateUi);
            UpdateUiDelegate2 del2 = new UpdateUiDelegate2(UpdateUi);

            string lastId = "";

            if (prevDao != null)
            {
                // identical data returned, probably no more new image.
                if (prevDao.RawData != null && prevDao.RawData.Equals(d.RawData))
                {
                    UpdateLog("DoBatchJob", "Identical list, probably last page.");
                    job.Status = "Identical list, probably last page.";
                    flag = false;
                    //break;
                }
            }
            prevDao = d;

            job.Total = d.PostCount;
            job.CurrentPageTotal = d.Posts.Count;
            job.CurrentPageOffset = d.Offset;

            job.PostCount = d.Posts.Count;

            foreach (DanbooruPost post in d.Posts)
            {
                // Update progress bar
                object[] myArray = new object[2];
                myArray[0] = job.ProcessedTotal;
                myArray[1] = d.PostCount < job.Limit ? d.PostCount : job.Limit;
                BeginInvoke(del2, myArray);

                // thread handling
                _pauseEvent.WaitOne(Timeout.Infinite);

                if (_shutdownEvent.WaitOne(0))
                {
                    job.Status = " ==> Stopped.";
                    // toggle button
                    BeginInvoke(bjd, new object[] { true });
                    UpdateLog("DoBatchJob", "Batch Job Stopped.");
                    UpdateStatus2("Batch Job Stopped.");
                    return;
                }
                bool download = false;
                try
                {
                    download = ProcessBatchJobPost(job, clientPost, post);
                    lastId = post.Id;
                }
                catch (Exception ex)
                {
                    var message = HandleBatchJobException(ex, ref flag, job);

                    if (cbxAbortOnError.Checked)
                    {
                        MessageBox.Show(message, "Batch Download");
                        break;
                    }
                }

                // clean up for post tag
                var index = d.Posts.IndexOf(post);
                var tempPost = d.Posts[index];
                tempPost.TagsEntity.Clear();
                tempPost.TagsEntity = null;
                tempPost = null;

                // check if more than available post
                if (job.ProcessedTotal >= d.PostCount && d.PostCount != 0)
                {
                    UpdateLog("DoBatchJob", "No more post.");
                    flag = false;
                    break;
                }
                // check if over given limit
                bool isLimitReached = Properties.Settings.Default.IgnoreSkipLimit ?
                                      ((job.ProcessedTotal - job.Skipped) >= job.Limit) :
                                      (job.ProcessedTotal >= job.Limit);
                if (isLimitReached)
                {
                    UpdateLog("DoBatchJob", "Limit Reached.");
                    flag = false;
                    break;
                }

                // check batch job delay
                Int32.TryParse(Properties.Settings.Default.BatchJobDelay, out int delay);
                if ((Properties.Settings.Default.DelayIncludeSkipped || download) && delay > 0)
                {
                    // UpdateLog("DoBatchJob", $"Waiting for {delay}ms for the next post.");
                    Thread.Sleep(delay);
                }
            }
            return;
        }

        private bool ProcessBatchJobPost(DanbooruBatchJob currentJob, ExtendedWebClient _clientPost, DanbooruPost post)
        {
            UpdateUiDelegate del = new UpdateUiDelegate(UpdateUi);
            bool download = true;

            if (chkDBIfExists.Checked)
            {
                var result = Program.DB.GetDownloadedFileByProviderAndPostId(post.Provider.Name, post.Id.ToString());
                if (result.Count > 0)
                {
                    ++currentJob.Skipped;
                    download = false;
                    // UpdateLog("DoBatchJob", $"Download skipped, ID: {post.Id} File exists in DB: {result[0].Path}\\{result[0].Filename}");
                }
            }

            if (download)
            {
                // check if have url and post is not deleted
                if (string.IsNullOrWhiteSpace(post.FileUrl) && (post.Status != "deleted" || chkProcessDeletedPost.Checked))
                {
                    ResolveFileUrlBatch(_clientPost, post);
                }

                //Choose the correct urls
                var targetUrl = post.FileUrl;
                if (_ImageSize == "Thumb" && !String.IsNullOrWhiteSpace(post.PreviewUrl))
                {
                    targetUrl = post.PreviewUrl;
                }
                else if (_ImageSize == "Jpeg" && !String.IsNullOrWhiteSpace(post.JpegUrl))
                {
                    targetUrl = post.JpegUrl;
                }
                else if (_ImageSize == "Sample" && !String.IsNullOrWhiteSpace(post.SampleUrl))
                {
                    targetUrl = post.SampleUrl;
                }

                //currentJob.Status = "Downloading: " + targetUrl;

                BeginInvoke(del);
                //if (post.Provider == null) post.Provider = cbxProvider.Text;
                //if (post.Query == null) post.Query = txtQuery.Text;
                //if (post.SearchTags == null) post.SearchTags = txtTags.Text;

                // check if blacklisted
                if (download && post.Hidden)
                {
                    ++currentJob.Skipped;
                    download = false;

                    var matchingBlacklistedTag = post.TagsEntity.Intersect(this.TagBlacklist).ToList();
                    if (matchingBlacklistedTag.Count > 0)
                    {
                        UpdateLog("DoBatchJob", $"Download skipped, contains blacklisted tag: {String.Join(" ", matchingBlacklistedTag.OrderBy(x => x.Name))} Url: {targetUrl}");
                    }
                    else
                    {
                        UpdateLog("DoBatchJob", $"Download skipped, contains blacklisted tag: {post.Tags} Url: {targetUrl}");
                    }
                }

                // Feature #95: filter by extensions
                if (!String.IsNullOrWhiteSpace(currentJob.Filter))
                {
                    // get file extensions
                    var ext = Helper.getFileExtensions(targetUrl);
                    bool tempResult = false;

                    if (currentJob.IsExclude)
                    {
                        // skip if match
                        tempResult = !Regex.IsMatch(ext, currentJob.Filter);
                        if (!tempResult)
                            UpdateLog("DoBatchJob", $"Download skipped, file extension: {ext} matching with filter: {currentJob.Filter} (Exclude Mode) in url {targetUrl}.");
                    }
                    else
                    {
                        // download if match
                        tempResult = Regex.IsMatch(ext, currentJob.Filter);
                        if (!tempResult)
                            UpdateLog("DoBatchJob", $"Download skipped, file extension: {ext} doesn't match with filter: {currentJob.Filter} in url {targetUrl}.");
                    }
                    download = tempResult;
                }

                string filename = "";
                if (download && !string.IsNullOrWhiteSpace(targetUrl))
                {
                    filename = MakeCompleteFilename(post, targetUrl, currentJob.SaveFolder);
                }

                // check if exist
                if (download && !chkOverwrite.Checked)
                {
                    if (File.Exists(filename))
                    {
                        ++currentJob.Skipped;
                        download = false;
                        UpdateLog("DoBatchJob", $"Download skipped, file exists: {filename}");
                    }
                }
                if (download && String.IsNullOrWhiteSpace(targetUrl))
                {
                    ++currentJob.Skipped;
                    download = false;
                    UpdateLog("DoBatchJob", $"Download skipped, ID: {post.Id} ==> No file_url, probably deleted");
                }
                if (download && !Uri.TryCreate(targetUrl, UriKind.RelativeOrAbsolute, out Uri uri))
                {
                    ++currentJob.Skipped;
                    download = false;
                    UpdateLog("DoBatchJob", $"Download skipped, ID: {post.Id} ==> Invalid URL: {targetUrl}");
                }

                #region download

#if DEBUG
                download = false;
#endif
                if (download)
                {
                    // delay subdir creation just before download
                    if (filename.Contains(@"\"))
                    {
                        string dir = filename.Substring(0, filename.LastIndexOf(@"\"));
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    }

                    var oldUrl = targetUrl;
                    targetUrl = Helper.ReplaceHost(targetUrl);
                    if (oldUrl != targetUrl)
                    {
                        UpdateLog("DoBatchJob", $"Url updated by host_replacement.txt: {oldUrl} ==> {targetUrl}");
                    }

                    currentJob.LastFileUrl = targetUrl;
                    currentJob.ImgCount = DoDownloadBatch(targetUrl, currentJob, post, filename);
                }
#if DEBUG
                //currentJob.CurrentFileUrl = targetUrl;
                currentJob.Downloaded++;
                currentJob.ProcessedTotal++;
#endif

                #endregion download
            }
            return download;
        }

        private int DoDownloadBatch(string targetUrl, DanbooruBatchJob job, DanbooruPost post, string filename)
        {
            UpdateLog("DoBatchJob", "Download: " + targetUrl);
            _clientBatch.Referer = post.Referer;

            int currRetry = 0;
            int maxRetry = Convert.ToInt32(txtRetry.Text);
            int delay = Convert.ToInt32(txtDelay.Text);

            while (currRetry <= maxRetry)
            {
                // check if temporary file exists and then delete it.
                var filename2 = filename + ".!tmp";
                if (File.Exists(filename2))
                {
                    UpdateLog("DoBatchJob", "Deleting temporary file: " + filename2);
                    File.Delete(filename2);
                }

                try
                {
#if DEBUG
                    UpdateLog("DoBatchJob", "DEBUG Saved To: " + filename);
                    Thread.Sleep(100);
#else
                    //_clientBatch.DownloadFile(targetUrl, filename2);
                    _clientBatch.DownloadFileTaskAsync(targetUrl, filename2, new Progress<Tuple<long, int, long>>(t =>
                    {
                        job.Status = $"Downloading: {targetUrl}\r\n\r\nProgress: {Helper.FormatByteSize(t.Item1)} of {Helper.FormatByteSize(t.Item3)} ({t.Item2}%)";
                        this.dgvBatchJob.BeginInvoke((Action)(() =>
                        {
                            if (this.Focused) this.Refresh();
                        }));
                    })).Wait();
                    File.Move(filename2, filename);
                    UpdateLog("DoBatchJob", "Saved To: " + filename);

                    var fileInfo = new FileInfo(filename);
                    Program.DB.Insert(post.Provider.Name, post.Id.ToString(), fileInfo.Name, fileInfo.DirectoryName);
#endif
                    ++job.Downloaded;

                    if (Properties.Settings.Default.WriteDownloadedFile)
                    {
                        // write to text file for downloaded file.
                        Helper.WriteTextFile(filename + Environment.NewLine);
                    }
                    if (Properties.Settings.Default.WriteTagFile)
                    {
                        Helper.WriteTagFile(post, filename + ".txt");
                    }

                    return 1;
                }
                catch (Exception ex)
                {
                    if (currRetry < maxRetry && cbxAbortOnError.Checked) throw;
                    else
                    {
                        var message = ResolveInnerExceptionMessages(ex);
                        UpdateLog("DoBatchJob", $"Error Download Batch Image ({currRetry} of {maxRetry}): {message} Wait for {delay}s.", ex);

                        for (int wait = 0; wait < delay; wait++)
                        {
                            _pauseEvent.WaitOne(Timeout.Infinite);
                            if (_shutdownEvent.WaitOne(0))
                            {
                                job.Status = " ==> Stopped.";
                                // toggle button
                                ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
                                BeginInvoke(bjd, new object[] { true });
                                UpdateLog("DoBatchJob", "Batch Job Stopped.");
                                UpdateStatus2("Batch Job Stopped.");
                                return 0;
                            }
                            //UpdateLog("DoBatchJob", "Wait for " + wait + " of " + delay);
                            Thread.Sleep(1000);
                        }
                        UpdateLog("DoBatchJob", "Retrying...");
                    }
                    ++currRetry;
                    if (currRetry > Convert.ToInt32(txtRetry.Text)) ++job.Error;
                }
                finally
                {
                    // Issue #251 ensure temporary file is deleted
                    if (File.Exists(filename2))
                    {
                        UpdateLog("DoBatchJob", $"Ensure to delete temporary file: {filename2}");
                        File.Delete(filename2);
                    }
                }
            }

            return 0; // failed
        }

        /// <summary>
        /// Get File Url for batch job
        /// </summary>
        /// <param name="_clientPost"></param>
        /// <param name="post"></param>
        private void ResolveFileUrlBatch(ExtendedWebClient _clientPost, DanbooruPost post)
        {
            _clientPost.Encoding = System.Text.Encoding.UTF8;
            if (!string.IsNullOrWhiteSpace(post.Referer))
            {
                UpdateLog("DoBatchJob", "Getting file_url from " + post.Referer);
                int currRetry = 0;
                int maxRetry = Convert.ToInt32(txtRetry.Text);
                int delay = Convert.ToInt32(txtDelay.Text);

                while (currRetry < maxRetry)
                {
                    try
                    {
                        string html = _clientPost.DownloadString(post.Referer);
                        _clientPost.Timeout = Convert.ToInt32(txtTimeout.Text);

                        if (post.Provider.BoardType == BoardType.Danbooru && post.Provider.Name.ToLower().Contains("sankaku"))
                        {
                            post = SankakuComplexParser.ParsePost(post, html, !chkUseGlobalProviderTags.Checked);
                            //post.FileUrl = tempPost.FileUrl;
                            //post.PreviewUrl = tempPost.PreviewUrl;
                        }
                        else if (post.Provider.BoardType == BoardType.Gelbooru)
                        {
                            var tempPost = GelbooruHtmlParser.ParsePost(post, html);
                            post.FileUrl = tempPost.FileUrl;
                            post.PreviewUrl = tempPost.PreviewUrl;
                        }
                        else if (post.Provider.BoardType == BoardType.Shimmie2 && post.Provider.Preferred == PreferredMethod.Html)
                        {
                            post = ShimmieHtmlParser.ParsePost(post, html, !chkUseGlobalProviderTags.Checked);
                        }
                        else
                        {
                            UpdateLog("DoBatchJob", "No HTML Parser available for : " + post.Provider.Name + "(" + post.Provider.BoardType.ToString() + ")");
                        }
                        UpdateLog("DoBatchJob", $"File Url: {post.FileUrl}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (currRetry >= maxRetry)
                        {
                            UpdateLog("DoBatchJob", "Giving Up Resolving FileUrl: " + ex.StackTrace, ex);
                            post.FileUrl = "";
                            post.JpegUrl = "";
                            post.SampleUrl = "";
                            break;
                        }
                        ++currRetry;

                        UpdateLog("DoBatchJob", "Error Resolving FileUrl (" + currRetry + " of " + maxRetry + "): " + ex.Message + " Wait for " + delay + "s.", ex);
                        for (int wait = 0; wait < delay; ++wait)
                        {
                            //UpdateLog("DoBatchJob", "Wait for " + wait + " of " + delay);
                            Thread.Sleep(1000);
                        }
                        UpdateLog("DoBatchJob", "Retrying...");
                    }
                }
            }
        }

        /// <summary>
        /// Get image list, return null if failed
        /// </summary>
        /// <param name="url"></param>
        /// <param name="searchParam"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        private DanbooruPostDao GetBatchImageList(DanbooruBatchJob job, int? currPage)
        {
            DanbooruPostDao d = null;
            int currRetry = 0;
            int maxRetry = Convert.ToInt32(txtRetry.Text);
            int delay = Convert.ToInt32(txtDelay.Text);
            while (currRetry < maxRetry)
            {
                try
                {
                    //var strs = _clientBatch.DownloadString(job.Url);
                    using (MemoryStream ms = new MemoryStream(_clientBatch.DownloadData(job.Url)))
                    {
                        var option = new DanbooruPostDaoOption()
                        {
                            Provider = job.Provider,
                            Query = job.Query,
                            SearchTags = job.TagQuery,
                            Referer = job.Url,
                            BlacklistedTags = TagBlacklist,
                            BlacklistedTagsRegex = TagBlacklistRegex,
                            BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                            IgnoredTags = TagIgnore,
                            IgnoredTagsRegex = TagIgnoreRegex,
                            IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked,
                            IsBlacklistOnlyForGeneral = chkBlacklistOnlyGeneral.Checked
                        };
                        d = new DanbooruPostDao(ms, option, currPage);
                    }
                    break;
                }
                catch (System.Net.WebException ex)
                {
                    ++currRetry;
                    UpdateLog("DoBatchJob", $"Error Getting List ({currRetry} of {maxRetry}): {ex.Message} Wait for {delay}s.", ex);
                    for (int wait = 0; wait < delay; ++wait)
                    {
                        //UpdateLog("DoBatchJob", "Wait for " + wait + " of " + delay);
                        Thread.Sleep(1000);

                        _pauseEvent.WaitOne(Timeout.Infinite);
                        if (_shutdownEvent.WaitOne(0))
                        {
                            ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
                            job.Status = " ==> Stopped.";
                            // toggle button
                            BeginInvoke(bjd, new object[] { true });
                            UpdateLog("DoBatchJob", "Batch Job Stopped.");
                            UpdateStatus2("Batch Job Stopped.");
                            return null;
                        }
                    }
                    UpdateLog("DoBatchJob", "Retrying...");
                }
            }

            return d;
        }

        public delegate void UpdateUiDelegate();

        public void UpdateUi()
        {
            foreach (DataGridViewRow row in dgvBatchJob.Rows)
            {
                if (batchJob[row.Index].isError)
                    row.DefaultCellStyle.BackColor = Color.Red;
                else if (batchJob[row.Index].isCompleted)
                    row.DefaultCellStyle.BackColor = Color.Lime;
            }
            dgvBatchJob.Refresh();
        }

        public delegate void UpdateUiDelegate2(int current, int total);

        public void UpdateUi(int current, int total)
        {
            tsProgressBar.Visible = true;
            if (current != -1 && total != -1)
            {
                if (total != 0)
                {
                    tsProgressBar.Style = ProgressBarStyle.Continuous;
                    tsProgressBar.Maximum = total;
                    tsProgressBar.Value = current >= total ? total : current;
                }
                else
                {
                    tsProgressBar.Style = ProgressBarStyle.Marquee;
                }
            }
            else
            {
                tsProgressBar.Visible = false;
            }

            statusStrip1.Refresh();
            dgvBatchJob.Refresh();
        }

        public delegate void ToggleBatchJobButtonDelegate(bool enabled);

        public void ToggleBatchJobButton(bool enabled)
        {
            btnStartBatchJob.Enabled = enabled;
            btnStopBatchJob.Enabled = !enabled;
            btnPauseBatchJob.Enabled = !enabled;
            btnClearCompleted.Enabled = enabled;
            btnClearAll.Enabled = enabled;
            deleteToolStripMenuItem1.Enabled = enabled;
            if (enabled)
            {
                btnPauseBatchJob.Text = "Pause Batch Job";
            }
        }

        #endregion batch job helper

        #region windows form events

        private void cbxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProvider.SelectedIndex == -1) return;

            txtPage.Text = "";
            _currProvider = _listProvider[cbxProvider.SelectedIndex];
            UpdateStatus();

            if (_currProvider.BoardType == BoardType.Danbooru) pbIcon.Image = Properties.Resources.Danbooru;
            else if (_currProvider.BoardType == BoardType.Gelbooru) pbIcon.Image = Properties.Resources.Gelbooru;
            else if (_currProvider.BoardType == BoardType.Shimmie2) pbIcon.Image = Properties.Resources.Shimmie2;
            else pbIcon.Image = null;
        }

        private void chkGenerate_CheckedChanged(object sender, EventArgs e)
        {
            txtQuery.Enabled = !chkGenerate.Checked;
            txtTags.Enabled = chkGenerate.Checked;
            txtLimit.Enabled = chkGenerate.Checked;
            txtPage.Enabled = chkGenerate.Checked;
            txtSource.Enabled = chkGenerate.Checked;
            cbxRating.Enabled = chkGenerate.Checked;
            cbxOrder.Enabled = chkGenerate.Checked;
            chkNotRating.Enabled = chkGenerate.Checked;
        }

        private void txtTags_TextChanged(object sender, EventArgs e)
        {
            txtPage.Text = "";
            _lastId = "";
            if (chkTagAutoComplete.Checked)
            {
                DoAutoComplete();
            }
            UpdateStatus();
        }

        private void cbxOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void cbxRating_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void chkRating_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void txtLimit_TextChanged(object sender, EventArgs e)
        {
            uint res = 0;
            if (!String.IsNullOrWhiteSpace(txtLimit.Text) && !UInt32.TryParse(txtLimit.Text, out res))
            {
                MessageBox.Show("Invalid limit value = " + txtLimit.Text, "Image Limit");
                txtLimit.Text = "";
            }
            if (res > _currProvider.HardLimit)
            {
                MessageBox.Show("Image limit too high! Setting back to " + _currProvider.HardLimit);
                txtLimit.Text = _currProvider.HardLimit.ToString();
            }
            UpdateStatus();
        }

        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtPage.Text) && !UInt32.TryParse(txtPage.Text, out uint res))
            {
                MessageBox.Show("Invalid page value = " + txtPage.Text, "List Page");
                if (_currProvider.BoardType == BoardType.Gelbooru)
                {
                    txtPage.Text = "0";
                }
                else txtPage.Text = "1";
            }
            UpdateStatus();
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            _lastId = ""; // reset the last id upon clicking get button.
            doGetList();
        }

        private void doGetList()
        {
            btnListCancel.Enabled = true;
            btnGet.Enabled = false;
            btnNextPage.Enabled = false;
            btnPrevPage.Enabled = false;
            _isMorePost = true;
            GetList();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (txtListFile.Text.Length > 0)
            {
                var option = new DanbooruPostDaoOption()
                {
                    Provider = _currProvider,
                    Url = txtListFile.Text,
                    Referer = _currProvider.Url,
                    Query = txtListFile.Text.Split('\\').Last(),
                    SearchTags = "",
                    BlacklistedTags = TagBlacklist,
                    BlacklistedTagsRegex = TagBlacklistRegex,
                    BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                    IgnoredTags = TagIgnore,
                    IgnoredTagsRegex = TagIgnoreRegex,
                    IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                };
                DanbooruPostDao newPosts = new DanbooruPostDao(option);
                LoadList(newPosts);
            }
        }

        private void btnBrowseListFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtListFile.Text = openFileDialog1.FileName;
            }
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            TransferDownloadRows();
            tabControl1.SelectedIndex = 1;
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (txtFilenameFormat.Text.Any(x => { return ":*?\"<>|".Contains(x); }))
            {
                MessageBox.Show("':*?\"<>|' characters in the filename format not allowed.");
                txtFilenameFormat.Focus();
            }
            else Properties.Settings.Default.Save();
            //MessageBox.Show("Saved!");
        }

        private void chkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _loadedThumbnail = 0;
            _postsDao = null;
            dgvList.Rows.Clear();
        }

        private void dgvList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool hideRow = chkHideBlaclistedImage.Checked;
            // add row number
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                dgvList.Rows[i].Cells["colNumber"].Value = i + 1;

                var item = dgvList.Rows[i].DataBoundItem as DanbooruPost;
                if (item.Hidden)
                {
                    dgvList.Rows[i].DefaultCellStyle.BackColor = Helper.ColorBlacklisted;
                    if (hideRow) dgvList.Rows[i].Visible = false;
                }
                else if (item.Status == "deleted")
                {
                    dgvList.Rows[i].DefaultCellStyle.BackColor = Helper.ColorDeleted;
                }
                else
                {
                    dgvList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        // http://img14.pixiv.net/img/leucojum-aest/17661193_p3.jpg
        private Regex pixivUrl = new Regex(@"img.*pixiv.*\/(\d+).*\.");

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvList.Columns["colUrl"].Index ||
                e.ColumnIndex == dgvList.Columns["colSourceUrl"].Index ||
                e.ColumnIndex == dgvList.Columns["colReferer"].Index
                )
            {
                if (e.RowIndex == -1) return;
                // Load web browser to the image url
                string url = dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                // preprocess Pixiv Url
                var match = pixivUrl.Match(url);
                if (match.Success)
                {
                    url = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id=" + match.Groups[1].Value;
                }

                Process.Start(url);
            }
        }

        private void timGifAnimation_Tick(object sender, EventArgs e)
        {
            // for AJAX_LOADER gif animation, may cause high CPU usage...
            if (CheckListGrid())
            {
                try
                {
                    dgvList.Rows[_loadedThumbnail].Cells["colPreview"].Value = pbLoading.Image;
                    dgvList.InvalidateCell(dgvList.Columns["colPreview"].Index, _loadedThumbnail);
                }
                catch (Exception ex)
                {
                    UpdateLog("timGifAnimation_Tick", ex.Message, ex);
                }
            }
        }

        // for auto load the next page
        private void dgvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= dgvList.Rows.Count - 2)
                timLoadNextPage.Enabled = true;
            else if (e.RowIndex < dgvList.Rows.Count - 5) timLoadNextPage.Enabled = false;
        }

        // the actual timer for auto load the next page
        private void timLoadNextPage_Tick(object sender, EventArgs e)
        {
            timLoadNextPage.Enabled = false;
            AutoLoadNextPage(dgvList.Rows.Count - 1);
            //MessageBox.Show("Hitt!");
        }

        private void txtUserAgent_TextChanged(object sender, EventArgs e)
        {
            _clientFile.Headers.Add("user-agent", txtUserAgent.Text);
            _clientList.Headers.Add("user-agent", txtUserAgent.Text);
            _clientThumb.Headers.Add("user-agent", txtUserAgent.Text);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            bool hideRow = chkHideBlaclistedImage.Checked;
            if (dgvList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colCheck"];
                    if (row.Visible) chk.Value = true;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Donate();
        }

        private void txtFilenameLength_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(txtFilenameLength.Text);

                if (i > MAX_FILENAME_LENGTH)
                {
                    var ok = false;
                    // Feature #79
                    DialogResult result = MessageBox.Show(String.Format("Maximum filename length exceeding limit ({0}). This might cause the file cannot be saved, continue?", MAX_FILENAME_LENGTH),
                                                "Warning", MessageBoxButtons.YesNo);

                    // test if can create long file name
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                            tempDirectory = tempDirectory.PadRight(300, 'x');
                            Directory.CreateDirectory(tempDirectory);
                            ok = true;
                            Directory.Delete(tempDirectory);
                        }
                        catch (PathTooLongException ex)
                        {
                            ShowMessage("Error", String.Format("Cannot create long file name {0}!\r\n{1}", i, ex.Message));
                        }
                    }

                    if (!ok)
                    {
                        txtFilenameLength.Text = MAX_FILENAME_LENGTH.ToString();
                    }
                }
            }
            catch (Exception)
            {
                txtFilenameLength.Text = MAX_FILENAME_LENGTH.ToString();
            }
        }

        private void btnListCancel_Click(object sender, EventArgs e)
        {
            if (_clientList.IsBusy)
            {
                _clientList.CancelAsync();
                btnListCancel.Enabled = false;
                btnList.Enabled = true;
            }
        }

        private void dgvDownload_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; ++i)
            {
                dgvDownload.Rows[i].Cells["colIndex"].Value = i + 1;
            }
        }

        private void btnClearDownloadList_Click(object sender, EventArgs e)
        {
            dgvDownload.Rows.Clear();
            _downloadList = null;
        }

        private void btnDownloadFiles_Click(object sender, EventArgs e)
        {
            _isCanceled = false;
            if (!_clientFile.IsBusy)
            {
                if (CheckDownloadGrid())
                {
                    if (txtSaveFolder.Text.Length == 0)
                    {
                        if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
                        }
                        else
                        {
                            ShowMessage("Save Folder", "Please select save folder!");
                            return;
                        }
                    }
                    if (txtSaveFolder.Text.Length > 0)
                    {
                        EnableDownloadControls(false);
                        DownloadRows(dgvDownload.Rows[0]);
                    }
                }
                else
                {
                    ShowMessage("Download List", "No image to download!");
                }
            }
        }

        private void btnPauseDownload_Click(object sender, EventArgs e)
        {
            _isPaused = true;
            _isDownloading = false;
            EnableDownloadControls(false);
            btnPauseDownload.Enabled = false;
            tsStatus.Text = "Pausing...";
        }

        private void btnCancelDownload_Click(object sender, EventArgs e)
        {
            if (_clientFile.IsBusy)
            {
                _clientFile.CancelAsync();
            }
            EnableDownloadControls(true);
            _isCanceled = true;
        }

        private void btnSaveDownloadList_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //XmlIncludeAttribute include = new XmlIncludeAttribute(typeof(Image));
                StreamWriter writer = new StreamWriter(saveFileDialog2.FileName);

                postSerializer.Serialize(writer, _downloadList);

                writer.Close();
            }
        }

        private void btnLoadDownloadList_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog2.FileName);

                _downloadList = (BindingList<DanbooruPost>)postSerializer.Deserialize(reader);
                reader.Close();
            }
            dgvDownload.DataSource = _downloadList;
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                if (chkMinimizeTray.Checked) Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void chkMinimizeTray_CheckedChanged(object sender, EventArgs e)
        {
            notifyIcon1.Visible = chkMinimizeTray.Checked;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDownload.SelectedRows)
            {
                dgvDownload.Rows.Remove(row);
            }
        }

        private void dgvDownload_MouseClick(object sender, MouseEventArgs e)
        {
            if (MouseButtons.Right == e.Button)
            {
                var hti = dgvDownload.HitTest(e.X, e.Y);
                dgvDownload.ClearSelection();
                dgvDownload.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void txtRetry_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _retry = Convert.ToInt32(txtRetry.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid format.");
                txtRetry.Text = "5";
                _retry = 5;
                txtRetry.Focus();
            }
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnSearchHelp_Click(object sender, EventArgs e)
        {
            FormCheatSheet fmCheat = new FormCheatSheet();
            fmCheat.ShowDialog();
        }

        private void searchByParentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count > 0)
            {
                txtTags.Text = "parent:" + dgvList.CurrentRow.Cells["colId"].Value.ToString();
            }
        }

        private void dgvList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    dgvList.CurrentCell = dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDownloading)
                if (MessageBox.Show("Still Downloading!", "Close Warning!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel) e.Cancel = true;

            if (chkSaveFolderWhenExit.Checked)
            {
                // save the contents of the folder
                string saveFolder = txtDefaultSaveFolder.Text;
                // reload the last setting
                Properties.Settings.Default.Reload();
                // Update the save folder
                Properties.Settings.Default.SaveFolder = saveFolder;
                // Save the setting
                Properties.Settings.Default.Save();
            }
        }

        private void addSelectedRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count >= 0)
            {
                if (_downloadList == null)
                {
                    _downloadList = new BindingList<DanbooruPost>();
                    dgvDownload.DataSource = _downloadList;
                }

                foreach (DataGridViewCell cell in dgvList.SelectedCells)
                {
                    var post = _postsDao.Posts[cell.RowIndex];
                    if (_downloadList.IndexOf(post) < 0)
                    {
                        TransferDownloadRow(post);
                    }
                }
            }
        }

        private void chkProxyLogin_CheckedChanged(object sender, EventArgs e)
        {
            CheckProxyLoginInput();
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);
        }

        private void btnBrowseFolder_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnClearCompleted_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < batchJob.Count; ++i)
            {
                if (batchJob[i].isCompleted)
                {
                    batchJob[i] = null;
                    batchJob.RemoveAt(i);
                    --i;
                }
            }
            dgvBatchJob.Refresh();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            dgvBatchJob.DataSource = null;
            batchJob.AllowEdit = false;
            batchJob.AllowNew = false;
            batchJob.Clear();
            batchJob = null;
            batchJob = new BindingList<DanbooruBatchJob>();
            dgvBatchJob.DataSource = batchJob;
            dgvBatchJob.Refresh();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvBatchJob.SelectedRows.Count > 0)
            {
                dgvBatchJob.Rows.Remove(dgvBatchJob.SelectedRows[0]);
            }
        }

        private void dgvBatchJob_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                var hti = dgvBatchJob.HitTest(e.X, e.Y);
                dgvBatchJob.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvBatchJob.Rows[hti.RowIndex].Selected = true;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (FormProvider form = new FormProvider())
            {
                form.Providers = _listProvider.ToList<DanbooruProvider>();
                form.SelectedIndex = cbxProvider.SelectedIndex;
                DialogResult result = form.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    DanbooruProviderDao.GetInstance().Save(form.Providers);
                    LoadProviderList();
                    cbxProvider.SelectedIndex = form.SelectedIndex;
                    _currProvider = _listProvider[cbxProvider.SelectedIndex];
                }
            }
        }

        private void linkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkUrl.Text);
        }

        private void chkUseTagColor_CheckedChanged(object sender, EventArgs e)
        {
            ToggleTagsColor();
        }

        private void lblColorGeneral_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorGeneral.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorGeneral.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        private void lblColorArtist_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorArtist.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorArtist.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        private void lblColorCopy_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorCopy.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorCopy.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        private void lblColorChara_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorChara.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorChara.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        private void lblColorCircle_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorCircle.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorCircle.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        private void lblColorFaults_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorFaults.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorFaults.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        //private void chkTagAutoComplete_CheckedChanged(object sender, EventArgs e)
        //{
        //    SetTagAutoComplete();
        //}

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FormDownloadTags form = new FormDownloadTags();
            form.ShowDialog();
            //SetTagAutoComplete();
        }

        private void txtTagBlacklist_TextChanged(object sender, EventArgs e)
        {
            ParseTagBlacklist();
        }

        private void lblColorBlacklistedTag_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorBlacklistedTag.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorBlacklistedTag.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        /// <summary>
        /// Manual searchParam string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            string queryStr = "";
            switch (_currProvider.Preferred)
            {
                case PreferredMethod.Html:
                    queryStr = _currProvider.QueryStringHtml;
                    break;

                case PreferredMethod.Json:
                    queryStr = _currProvider.QueryStringJson;
                    break;

                case PreferredMethod.Xml:
                    queryStr = _currProvider.QueryStringXml;
                    break;

                default:
                    queryStr = "Invalid Prefered Method!";
                    break;
            }
            tsStatus.Text = "Query URL: " + _currProvider.Url + queryStr.Replace("%_query%", txtQuery.Text);
            tsStatus.Text = tsStatus.Text.Replace("&", "&&");
        }

        private void chkLogging_CheckedChanged(object sender, EventArgs e)
        {
            Program.SetLogger(chkLogging.Checked);
        }

        #endregion windows form events

        private string _ImageSize;

        private void cbxImageSize_TextChanged(object sender, EventArgs e)
        {
            UpdateImageSizeOption();
        }

        private void UpdateImageSizeOption()
        {
            if (cbxImageSize.Text == "Thumb")
            {
                dgvList.Columns["colUrl"].DataPropertyName = "PreviewUrl";
                dgvDownload.Columns["colUrl2"].DataPropertyName = "PreviewUrl";
            }
            else if (cbxImageSize.Text == "Jpeg")
            {
                dgvList.Columns["colUrl"].DataPropertyName = "JpegUrl";
                dgvDownload.Columns["colUrl2"].DataPropertyName = "JpegUrl";
            }
            else if (cbxImageSize.Text == "Sample")
            {
                dgvList.Columns["colUrl"].DataPropertyName = "SampleUrl";
                dgvDownload.Columns["colUrl2"].DataPropertyName = "SampleUrl";
            }
            else
            {
                dgvList.Columns["colUrl"].DataPropertyName = "FileUrl";
                dgvDownload.Columns["colUrl2"].DataPropertyName = "FileUrl";
            }
            _ImageSize = cbxImageSize.Text;
            dgvList.Refresh();
            dgvDownload.Refresh();
        }

        private void dgvDownload_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDownload.Columns["colUrl2"].Index//||
                                                                     //e.ColumnIndex == dgvDownload.Columns["colSourceUrl"].Index ||
                                                                     //e.ColumnIndex == dgvDownload.Columns["colReferer"].Index
                 )
            {
                if (e.RowIndex == -1) return;
                // Load web browser to the image url
                string url = dgvDownload.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                // preprocess Pixiv Url
                var match = pixivUrl.Match(url);
                if (match.Success)
                {
                    url = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id=" + match.Groups[1].Value;
                }

                Process.Start(url);
            }
        }

        #region Auto Complete Logic

        private string keyword;

        private void DoAutoComplete()
        {
            // get the last word
            if (!String.IsNullOrWhiteSpace(txtTags.Text))
            {
                keyword = txtTags.Text.Split(' ').LastOrDefault();
                if (!String.IsNullOrWhiteSpace(keyword))
                {
                    int limit = 200;
                    try
                    {
                        limit = Convert.ToInt32(txtAutoCompleteLimit.Text);
                    }
                    catch (Exception)
                    {
                        txtAutoCompleteLimit.Text = limit.ToString();
                    }
                    List<string> candidate = null;
                    // get the autocomplete candidate
                    if (Properties.Settings.Default.UseGlobalProviderTags && _currProvider.HasPrivateTags)
                    {
                        candidate = _currProvider.ProviderTagCollection.Tag.Where(x => x.Name.StartsWith(keyword)).Select(x => x.Name).Take(limit).ToList<String>();
                    }
                    else
                        candidate = DanbooruTagsDao.Instance.Tags.Tag.Where(x => x.Name.StartsWith(keyword)).Select(x => x.Name).Take(limit).ToList<String>();
                    if (candidate.Count > 0)
                    {
                        lbxAutoComplete.DataSource = candidate;
                        lbxAutoComplete.SelectedIndex = -1;
                        return;
                    }
                }
            }
            lbxAutoComplete.DataSource = null;
        }

        private void btnBrowseDefaultSave_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDefaultSaveFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void lbxAutoComplete_DoubleClick(object sender, EventArgs e)
        {
            ReplaceKeyword();
            txtTags.Focus();
            txtTags.SelectionStart = txtTags.Text.LastIndexOf(' ') + 1;
        }

        private string ReplaceKeyword()
        {
            var keyword = "";
            if (lbxAutoComplete.SelectedIndex > -1)
            {
                keyword = lbxAutoComplete.SelectedValue.ToString();
                var lastSpaceIndex = txtTags.Text.LastIndexOf(' ');
                if (lastSpaceIndex > -1)
                {
                    txtTags.Text = txtTags.Text.Substring(0, lastSpaceIndex + 1) + keyword;
                }
                else
                {
                    txtTags.Text = keyword;
                }
            }
            return keyword;
        }

        private void txtTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (lbxAutoComplete.Visible)
            {
                if (e.KeyCode == Keys.Down)
                {
                    lbxAutoComplete.Focus();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    lbxAutoComplete.DataSource = null;
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                doGetList();
            }
        }

        private void lbxAutoComplete_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReplaceKeyword();
                txtTags.Focus();
                txtTags.SelectionStart = txtTags.Text.LastIndexOf(' ') + 1;
            }
            else if (e.KeyCode == Keys.Space)
            {
                ReplaceKeyword();
                txtTags.Focus();
                txtTags.Text += " ";
                txtTags.SelectionStart = txtTags.Text.Length;
            }
            else if (e.KeyCode == Keys.Back)
            {
                txtTags.Focus();
                txtTags.SelectionStart = txtTags.Text.Length;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                lbxAutoComplete.DataSource = null;
            }
        }

        /// <summary>
        /// show the listbox if have item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbxAutoComplete_DataSourceChanged(object sender, EventArgs e)
        {
            if (lbxAutoComplete.DataSource != null)
            {
                var ds = lbxAutoComplete.DataSource as List<String>;
                if (ds.Count > 0)
                {
                    if (ds.Count != 1 || ds[0] != keyword)
                    {
                        var location = txtTags.GetPositionFromCharIndex(txtTags.Text.LastIndexOf(' '));
                        lbxAutoComplete.Left = location.X + txtTags.Left;

                        lbxAutoComplete.Visible = true;
                        return;
                    }
                }
            }
            lbxAutoComplete.Visible = false;
        }

        private void txtTags_Leave(object sender, EventArgs e)
        {
            if (lbxAutoComplete.Visible && !lbxAutoComplete.Focused)
            {
                lbxAutoComplete.Visible = false;
            }
        }

        private void lbxAutoComplete_Leave(object sender, EventArgs e)
        {
            if (!txtTags.Focused)
            {
                lbxAutoComplete.Visible = false;
            }
        }

        #endregion Auto Complete Logic

        private void reloadThumbnailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvList.Rows.Count > 0)
            {
                LoadThumbnailLater(dgvList.CurrentRow.Index);
            }
        }

        private void resetColumnOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvList.ResetColumnOrder();
        }

        private void resolveFileUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDownload.SelectedRows)
            {
                if (_downloadList[row.Index].Status != "deleted" || chkProcessDeletedPost.Checked)
                {
                    _downloadList[row.Index].FileUrl = Constants.LOADING_URL;
                    ResolveFileUrl(_downloadList[row.Index]);
                    dgvDownload.Refresh();
                }
            }
        }

        private void txtArtistTagGrouping_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtArtistTagGrouping.Text, out int i))
            {
                MessageBox.Show("Invalid value: " + txtArtistTagGrouping.Text, "txtArtistTagGrouping");
                txtArtistTagGrouping.Text = "5";
            }
        }

        private void txtCopyTagGrouping_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtCopyTagGrouping.Text, out int i))
            {
                MessageBox.Show("Invalid value: " + txtCopyTagGrouping.Text, "txtCopyTagGrouping");
                txtCopyTagGrouping.Text = "5";
            }
        }

        private void txtCharaTagGrouping_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtCharaTagGrouping.Text, out int i))
            {
                MessageBox.Show("Invalid value: " + txtCharaTagGrouping.Text, "txtCharaTagGrouping");
                txtCharaTagGrouping.Text = "5";
            }
        }

        private void txtCircleTagGrouping_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtCircleTagGrouping.Text, out int i))
            {
                MessageBox.Show("Invalid value: " + txtCircleTagGrouping.Text, "txtCircleTagGrouping");
                txtCircleTagGrouping.Text = "5";
            }
        }

        private void txtFaultsTagGrouping_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtFaultsTagGrouping.Text, out int i))
            {
                MessageBox.Show("Invalid value: " + txtFaultsTagGrouping.Text, "txtFaultsTagGrouping");
                txtFaultsTagGrouping.Text = "5";
            }
        }

        private void txtProxyAddress_TextChanged(object sender, EventArgs e)
        {
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);
        }

        private void txtProxyPort_TextChanged(object sender, EventArgs e)
        {
            bool result = Int32.TryParse(txtProxyPort.Text, out int port);
            if (result)
            {
                SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, port, txtProxyUsername.Text, txtProxyPassword.Text);
            }
            else
            {
                MessageBox.Show("Invalid Port Number: " + txtProxyPort.Text, "Error Parsing Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProxyPort.SelectAll();
                txtProxyPort.Focus();
            }
        }

        private void txtProxyUsername_TextChanged(object sender, EventArgs e)
        {
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);
        }

        private void txtProxyPassword_TextChanged(object sender, EventArgs e)
        {
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var source = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            if (source == txtLog)
            {
                if (!String.IsNullOrWhiteSpace(txtLog.SelectedText))
                    Clipboard.SetText(txtLog.SelectedText);
            }
            //else if (source == txtQuery)
            //{
            //    if (!String.IsNullOrWhiteSpace(txtQuery.Text))
            //        Clipboard.SetText(txtQuery.Text);
            //}
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            doGetPrevPage();
        }

        private void doGetPrevPage()
        {
            if (_postsDao != null)
            {
                if (!_isLoadingList)
                {
                    if (!_clientList.IsBusy)
                    {
                        var result = Int32.TryParse(txtPage.Text, out int page);
                        if (result)
                        {
                            if ((_currProvider.BoardType == BoardType.Gelbooru && page == 0) || page == 1)
                            {
                                MessageBox.Show("First Page!", "Prev Page");
                                return;
                            }
                            if (_currProvider.BoardType == BoardType.Gelbooru && _currProvider.Preferred == PreferredMethod.Html)
                            {
                                page = page - _currCount;
                                if (page < 0) page = 0;
                            }
                            else
                            {
                                --page;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No previous page information", "Prev Page");
                            return;
                        }
                        txtPage.Text = page.ToString();
                        doGetList();
                    }
                }
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            doGetNextPage();
        }

        private void doGetNextPage()
        {
            if (_postsDao != null && _isMorePost)
            {
                if (!_isLoadingList && !_clientList.IsBusy)
                {
                    var result = Int32.TryParse(txtPage.Text, out int page);
                    if (result)
                    {
                        if (_currProvider.BoardType == BoardType.Gelbooru && _currProvider.Preferred == PreferredMethod.Html)
                        {
                            page = _currCount + page;
                        }
                        else
                        {
                            ++page;
                        }
                    }
                    else
                    {
                        if (_currProvider.BoardType == BoardType.Gelbooru) page = 1;
                        else page = 2;
                    }
                    txtPage.Text = page.ToString();
                    doGetList();
                }
            }
        }

        private void txtDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _delay = Convert.ToInt32(txtDelay.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid format.");
                txtDelay.Text = "10";
                _delay = 10;
                txtDelay.Focus();
            }
        }

        private void txtIgnoredTags_TextChanged(object sender, EventArgs e)
        {
            ParseIgnoredTags();
        }

        private void ParseIgnoredTags()
        {
            var tagStr = txtIgnoredTags.Text.Trim();
            if (!String.IsNullOrWhiteSpace(tagStr))
            {
                TagIgnore = DanbooruTagsDao.Instance.ParseTagsString(tagStr.Replace(Environment.NewLine, " "));
                try
                {
                    TagIgnoreRegex = new Regex(tagStr.Trim().Replace(Environment.NewLine, "|").Replace(" ", "|"), RegexOptions.IgnoreCase);
                }
                catch (Exception ex)
                {
                    Program.Logger.Debug(ex.Message);
                    TagIgnoreRegex = new Regex("$^");
                }
            }
            else
            {
                if (TagIgnore != null) TagIgnore.Clear();
                if (TagIgnoreRegex != null) TagIgnoreRegex = new Regex("$^");
            }
        }

        private void btnSaveBatchList_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //XmlIncludeAttribute include = new XmlIncludeAttribute(typeof(Image));
                StreamWriter writer = new StreamWriter(saveFileDialog2.FileName);

                jobSerializer.Serialize(writer, batchJob);

                writer.Close();
            }
        }

        private void btnLoadList_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog2.FileName);

                batchJob = (BindingList<DanbooruBatchJob>)jobSerializer.Deserialize(reader);
                reader.Close();
            }
            dgvBatchJob.DataSource = batchJob;
        }

        private void dgvBatchJob_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; ++i)
            {
                dgvBatchJob.Rows[i].Cells["colBatchId"].Value = i + 1;
            }
        }

        private void btnClearCompletedDownload_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _downloadList.Count; ++i)
            {
                if (_downloadList[i].Completed)
                {
                    _downloadList.RemoveAt(i);
                    --i;
                }
            }
        }

        private void chkEnableCookie_CheckedChanged(object sender, EventArgs e)
        {
            ExtendedWebClient.EnableCookie = chkEnableCookie.Checked;
        }

        private void btnCookie_Click(object sender, EventArgs e)
        {
            FormCookie cookieForm = new FormCookie();
            cookieForm.Show();
        }

        private void lblColorUnknown_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColorUnknown.ForeColor;
            if (DialogResult.OK == colorDialog1.ShowDialog())
            {
                lblColorUnknown.ForeColor = colorDialog1.Color;
                SetTagColors();
            }
        }

        private void txtAcceptLanguage_TextChanged(object sender, EventArgs e)
        {
            ExtendedWebClient.AcceptLanguage = txtAcceptLanguage.Text;
        }

        private void chkEnableCompression_CheckedChanged(object sender, EventArgs e)
        {
            ExtendedWebClient.EnableCompression = chkEnableCompression.Checked;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDownload.SelectedRows.Count > 5)
            {
                var result = MessageBox.Show("You are going to open too many images (>5), proceed?", "Warning", MessageBoxButtons.OKCancel);
                if (result == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            foreach (DataGridViewRow row in dgvDownload.SelectedRows)
            {
                string path = row.Cells["colFilename"].Value.ToString();
                if (!String.IsNullOrWhiteSpace(path)) System.Diagnostics.Process.Start(path);
            }
        }

        /// <summary>
        /// make complete filename
        /// filename format will be overwritten based on each job settings.
        /// save folder follow txtSaveFolder in settings tab.
        /// </summary>
        /// <param name="post"></param>
        /// <param name="url"></param>
        /// <param name="filenameFormat"></param>
        /// <returns></returns>
        private string MakeCompleteFilename(DanbooruPost post, string url, string filenameFormat = null)
        {
            if (String.IsNullOrWhiteSpace(filenameFormat))
                filenameFormat = txtFilenameFormat.Text;

            var format = new DanbooruFilenameFormat()
            {
                FilenameFormat = filenameFormat,
                Limit = Convert.ToInt32(txtFilenameLength.Text),
                BaseFolder = txtSaveFolder.Text,
                MissingTagReplacement = txtTagReplacement.Text,
                ArtistGroupLimit = Convert.ToInt32(txtArtistTagGrouping.Text),
                ArtistGroupReplacement = txtOverArtist.Text,
                CharacterGroupLimit = Convert.ToInt32(txtCharaTagGrouping.Text),
                CharacterGroupReplacement = txtOverChara.Text,
                CopyrightGroupLimit = Convert.ToInt32(txtCopyTagGrouping.Text),
                CopyrightGroupReplacement = txtOverCopyright.Text,
                CircleGroupLimit = Convert.ToInt32(txtCircleTagGrouping.Text),
                CircleGroupReplacement = txtOverCircle.Text,
                FaultsGroupLimit = Convert.ToInt32(txtFaultsTagGrouping.Text),
                FaultsGroupReplacement = txtOverFault.Text,
                IgnoredTags = DanbooruTagsDao.Instance.ParseTagsString(txtIgnoredTags.Text.Replace(Environment.NewLine, " ")),
                IgnoredTagsRegex = txtIgnoredTags.Text.Trim().Replace(Environment.NewLine, "|"),
                IgnoreTagsUseRegex = chkIgnoreTagsUseRegex.Checked,
                IsReplaceMode = chkReplaceMode.Checked,
                IgnoredTagsOnlyForGeneral = chkIgnoreForGeneralTag.Checked,
                TagReplaceUnderscoreToSpace = chkIsReplaceUnderscoreTag.Checked
            };

            string extension = Helper.getFileExtensions(url);
            string filename = Helper.MakeFilename(format, post) + extension;
            return filename;
        }
    }
}