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
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;
using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using DanbooruDownloader3.Entity;
using DanbooruDownloader3.Utils;

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        #region property

        public static bool Debug = false;

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
                                    "%originalFilename% = Original Filename" + Environment.NewLine +
                                    "%searchtag% = Search tag";

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

            ToggleTagsColor();
            ExtendedWebClient.EnableCookie = Properties.Settings.Default.enableCookie;
            ExtendedWebClient.EnableCompression = Properties.Settings.Default.EnableCompression;
            ExtendedWebClient.AcceptLanguage = Properties.Settings.Default.AcceptLanguage;
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
                        if (_downloadList.IndexOf(_postsDao.Posts[row.Index]) < 0)
                        {
                            var post = _postsDao.Posts[row.Index];
                            // try to get the file url if empty
                            if (string.IsNullOrWhiteSpace(post.FileUrl) && (post.Status != "deleted" || chkProcessDeletedPost.Checked))
                            {
                                if (!string.IsNullOrWhiteSpace(post.Referer))
                                {
                                    ResolveFileUrl(post);
                                }
                            }
                            _downloadList.Add(post);
                            row.Cells["colCheck"].Value = false;
                        }
                    }
                    dgvDownload.DataSource = _downloadList;
                }
            }
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
                ExtendedWebClient _clientPost = new ExtendedWebClient();
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
            //if (post.Provider.Contains("Sankaku Complex"))
            if (post.Provider.BoardType == BoardType.Danbooru)
            {
                if (e.Error == null)
                {
                    string html = e.Result;
                    post = SankakuComplexParser.ParsePost(post, html);
                    UpdateLog("SankakuComplexParser", "Resolved to file_url: " + post.FileUrl);
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "File url resolved!";
                }
                else
                {
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "SankakuComplexParser " + e.Error.Message;
                    UpdateLog("SankakuComplexParser", "Unable to get file_url for: " + post.Referer + " ==> " + e.Error.Message, e.Error);
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
                }
                else
                {
                    dgvDownload.Rows[_downloadList.IndexOf(post)].Cells["colProgress2"].Value = "GelbooruHtmlParser " + e.Error.Message;
                    UpdateLog("GelbooruHtmlParser", "Unable to get file_url for: " + post.Referer + " ==> " + e.Error.Message, e.Error);
                    post.FileUrl = "";
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

                        var format = new DanbooruFilenameFormat()
                        {
                            FilenameFormat = txtFilenameFormat.Text,
                            Limit = Convert.ToInt32(txtFilenameLength.Text),
                            BaseFolder = txtSaveFolder.Text,
                            MissingTagReplacement = txtTagReplacement.Text,
                            ArtistGroupLimit = Convert.ToInt32(txtArtistTagGrouping.Text),
                            CharacterGroupLimit = Convert.ToInt32(txtCharaTagGrouping.Text),
                            CopyrightGroupLimit = Convert.ToInt32(txtCopyTagGrouping.Text),
                            CircleGroupLimit = Convert.ToInt32(txtCircleTagGrouping.Text),
                            FaultsGroupLimit = Convert.ToInt32(txtFaultsTagGrouping.Text),
                            IgnoredTags = DanbooruTagsDao.Instance.ParseTagsString(txtIgnoredTags.Text.Replace(Environment.NewLine, " ")),
                            IgnoredTagsRegex = txtIgnoredTags.Text.Trim().Replace(Environment.NewLine, "|"),
                            IgnoreTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                        };

                        string extension = Helper.getFileExtensions(url);
                        string filename = Helper.MakeFilename(format, post) + extension;

                        if (chkOverwrite.Checked || !File.Exists(filename))
                        {
                            string dir = filename.Substring(0, filename.LastIndexOf(@"\"));
                            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                            row.Cells["colFilename"].Value = filename;
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

                            return;
                        }
                        else
                        {
                            // File exist and overwrite is no checked.
                            row.Cells["colProgress2"].Value = "File exists!";
                            UpdateLog("[DownloadRow]", "File exists: " + filename);
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
                        UpdateLog("[LoadList]", "Appending: " + postDao.Option.SearchTags + " Offset: " + postDao.Offset);

                        int oldCount = _postsDao.Posts.Count;

                        foreach (DanbooruPost po in postDao.Posts)
                        {
                            _postsDao.Posts.Add(po);
                        }
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
                _clientThumb.DownloadDataAsync(new Uri(_postsDao.Posts[_loadedThumbnail].PreviewUrl), _loadedThumbnail);
                txtLog.AppendText("[clientThumbnail] Downloading thumbnail from " + _postsDao.Posts[_loadedThumbnail].PreviewUrl + Environment.NewLine);
            }
        }

        /// <summary>
        /// Download posts list
        /// </summary>
        private void GetList()
        {
            var queryUrl = GetQueryUrl();
            Program.Logger.Info("Getting list: " + Helper.RemoveAuthInfo(queryUrl));

            if (chkSaveQuery.Checked)
            {
                saveFileDialog1.FileName = cbxProvider.Text + " - " + txtTags.Text + " " + txtPage.Text + "." + _currProvider.Preferred.ToString().ToLower();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gbxSearch.Enabled = false;
                    gbxList.Enabled = false;

                    tsProgressBar.Value = 0;

                    string referer = _listProvider[cbxProvider.SelectedIndex].Url;
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
                    string referer = _listProvider[cbxProvider.SelectedIndex].Url;
                    _clientList.Referer = referer;
                    _clientList.DownloadDataAsync(new Uri(queryUrl), queryUrl);
                    tsProgressBar.Visible = true;
                    if (chkLoadPreview.Checked && !chkAutoLoadNext.Checked && !chkAppendList.Checked && _clientThumb.IsBusy) _resetLoadedThumbnail = true;
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
                f = new FormAddBatchJob();
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
                    row.Cells["colBatchId"].Value = row.Index;
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

        public void DoBatchJob(BindingList<DanbooruBatchJob> batchJob)
        {
            ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
            UpdateUiDelegate del = new UpdateUiDelegate(UpdateUi);
            UpdateUiDelegate2 del2 = new UpdateUiDelegate2(UpdateUi);
            ExtendedWebClient _clientPost = new ExtendedWebClient();
            if (batchJob != null)
            {
                UpdateStatus2("Starting Batch Job");

                for (int i = 0; i < batchJob.Count; i++)
                {
                    batchJob[i].CurrentPage = 0;
                    if (!batchJob[i].isCompleted)
                    {
                        UpdateLog("DoBatchJob", "Processing Batch Job#" + i);

                        DanbooruPostDao prevDao = null;
                        bool flag = true;
                        int currPage = 0;
                        int postCount = 0;
                        do
                        {
                            // stop/pause event handling outside
                            _pauseEvent.WaitOne(Timeout.Infinite);
                            if (_shutdownEvent.WaitOne(0))
                            {
                                batchJob[i].Status = " ==> Stopped.";
                                // toggle button
                                BeginInvoke(bjd, new object[] { true });
                                UpdateLog("DoBatchJob", "Batch Job Stopped.");
                                UpdateStatus2("Batch Job Stopped.");
                                return;
                            }

                            DanbooruPostDao d = null;
                            int imgCount = 0;
                            int skipCount = 0;

                            string url;
                            string query = "";

                            #region Construct the searchParam

                            if (batchJob[i].Provider.BoardType == BoardType.Danbooru)
                            {
                                currPage = batchJob[i].CurrentPage;
                            }
                            else if (batchJob[i].Provider.BoardType == BoardType.Gelbooru)
                            {
                                if (batchJob[i].Provider.Preferred == PreferredMethod.Html)
                                {
                                    currPage = batchJob[i].CurrentPage * postCount;
                                }
                                else
                                {
                                    currPage = batchJob[i].CurrentPage;
                                }
                            }
                            DanbooruSearchParam searchParam = GetSearchParamsFromJob(batchJob[i], currPage);

                            url = batchJob[i].Provider.GetQueryUrl(searchParam);

                            #endregion Construct the searchParam

                            try
                            {
                                #region Get and load the image list

                                batchJob[i].Status = "Getting list for page " + batchJob[i].CurrentPage;
                                BeginInvoke(del);
                                UpdateLog("DoBatchJob", "Downloading list: " + url);

                                d = GetBatchImageList(url, query, batchJob[i]);

                                if (d == null)
                                {
                                    // Cannot get list.
                                    UpdateLog("DoBatchJob", "Cannot load list");
                                    batchJob[i].Status = "Cannot load list.";
                                    batchJob[i].isCompleted = false;
                                    batchJob[i].isError = true;
                                    flag = false;
                                }
                                else if (d.Posts == null || d.Posts.Count == 0)
                                {
                                    // No more image
                                    UpdateLog("DoBatchJob", "No more image.");
                                    batchJob[i].Status = "No more image.";
                                    flag = false;
                                    //break;
                                }
                                else
                                {
                                    if (prevDao != null)
                                    {
                                        // identical data returned, probably no more new image.
                                        if (prevDao.RawData != null && prevDao.RawData.Equals(d.RawData))
                                        {
                                            UpdateLog("DoBatchJob", "Identical list, probably last page.");
                                            batchJob[i].Status = "Identical list, probably last page.";
                                            flag = false;
                                            //break;
                                        }
                                    }
                                    prevDao = d;

                                    batchJob[i].Total = d.PostCount;
                                    batchJob[i].CurrentPageTotal = d.Posts.Count;
                                    batchJob[i].CurrentPageOffset = d.Offset;

                                #endregion Get and load the image list

                                    postCount = d.Posts.Count;

                                    foreach (DanbooruPost post in d.Posts)
                                    {
                                        // Update progress bar
                                        object[] myArray = new object[2];
                                        myArray[0] = batchJob[i].ProcessedTotal;
                                        myArray[1] = d.PostCount < batchJob[i].Limit ? d.PostCount : batchJob[i].Limit;
                                        BeginInvoke(del2, myArray);

                                        // thread handling
                                        _pauseEvent.WaitOne(Timeout.Infinite);

                                        if (_shutdownEvent.WaitOne(0))
                                        {
                                            batchJob[i].Status = " ==> Stopped.";
                                            // toggle button
                                            BeginInvoke(bjd, new object[] { true });
                                            UpdateLog("DoBatchJob", "Batch Job Stopped.");
                                            UpdateStatus2("Batch Job Stopped.");
                                            return;
                                        }

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

                                        batchJob[i].Status = "Downloading: " + targetUrl;
                                        BeginInvoke(del);
                                        //if (post.Provider == null) post.Provider = cbxProvider.Text;
                                        //if (post.Query == null) post.Query = txtQuery.Text;
                                        //if (post.SearchTags == null) post.SearchTags = txtTags.Text;

                                        string filename = "";
                                        if (!string.IsNullOrWhiteSpace(targetUrl))
                                        {
                                            var format = new DanbooruFilenameFormat()
                                            {
                                                FilenameFormat = batchJob[i].SaveFolder,
                                                Limit = Convert.ToInt32(txtFilenameLength.Text),
                                                BaseFolder = txtSaveFolder.Text,
                                                MissingTagReplacement = txtTagReplacement.Text,
                                                ArtistGroupLimit = Convert.ToInt32(txtArtistTagGrouping.Text),
                                                CharacterGroupLimit = Convert.ToInt32(txtCharaTagGrouping.Text),
                                                CopyrightGroupLimit = Convert.ToInt32(txtCopyTagGrouping.Text),
                                                CircleGroupLimit = Convert.ToInt32(txtCircleTagGrouping.Text),
                                                FaultsGroupLimit = Convert.ToInt32(txtFaultsTagGrouping.Text),
                                                IgnoredTags = DanbooruTagsDao.Instance.ParseTagsString(txtIgnoredTags.Text.Replace(Environment.NewLine, " ")),
                                                IgnoredTagsRegex = txtIgnoredTags.Text.Trim().Replace(Environment.NewLine, "|"),
                                                IgnoreTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                                            };
                                            string extension = Helper.getFileExtensions(targetUrl);
                                            filename = Helper.MakeFilename(format, post) + extension;
                                        }
                                        bool download = true;
                                        // check if exist
                                        if (download && !chkOverwrite.Checked)
                                        {
                                            if (File.Exists(filename))
                                            {
                                                ++skipCount;
                                                ++batchJob[i].Skipped;
                                                download = false;
                                                UpdateLog("DoBatchJob", "Download skipped, file exists: " + filename);
                                            }
                                        }
                                        if (download && post.Hidden)
                                        {
                                            ++skipCount;
                                            ++batchJob[i].Skipped;
                                            download = false;
                                            UpdateLog("DoBatchJob", "Download skipped, contains blacklisted tag: " + post.Tags + " Url: " + targetUrl);
                                        }
                                        if (download && String.IsNullOrWhiteSpace(targetUrl))
                                        {
                                            ++skipCount;
                                            ++batchJob[i].Skipped;
                                            download = false;
                                            UpdateLog("DoBatchJob", "Download skipped, ID: " + post.Id + " No file_url, probably deleted");
                                        }
                                        Uri uri = null;
                                        if (download && !Uri.TryCreate(targetUrl, UriKind.RelativeOrAbsolute, out uri))
                                        {
                                            ++skipCount;
                                            ++batchJob[i].Skipped;
                                            download = false;
                                            UpdateLog("DoBatchJob", "Download skipped, ID: " + post.Id + " Invalid URL: " + targetUrl);
                                        }

                                        #region download

                                        if (download)
                                        {
                                            imgCount = DoDownloadBatch(targetUrl, batchJob[i], post, filename);
                                        }

                                        #endregion download

                                        // check if more than available post
                                        if (batchJob[i].ProcessedTotal >= d.PostCount && d.PostCount != 0)
                                        {
                                            UpdateLog("DoBatchJob", "No more post.");
                                            flag = false;
                                            break;
                                        }
                                        // check if over given limit
                                        if (batchJob[i].ProcessedTotal >= batchJob[i].Limit)
                                        {
                                            UpdateLog("DoBatchJob", "Limit Reached.");
                                            flag = false;
                                            break;
                                        }

                                        // check batch job delay
                                        int delay = 0;
                                        Int32.TryParse(Properties.Settings.Default.BatchJobDelay, out delay);
                                        if ((Properties.Settings.Default.DelayIncludeSkipped || download) && delay > 0)
                                        {
                                            UpdateLog("DoBatchJob", String.Format("Waiting for {0}ms for the next post.", delay));
                                            Thread.Sleep(delay);
                                        }
                                    }
                                }
                                batchJob[i].Status = " ==> Done.";
                            }
                            catch (Exception ex)
                            {
                                string message = ex.Message;
                                string responseMessage = "";
                                if (ex.InnerException != null)
                                {
                                    message += Environment.NewLine + "Inner: " + ex.InnerException.Message;
                                }
                                message += Environment.NewLine + "Stack Trace: " + Environment.NewLine + ex.StackTrace;
                                message += Environment.NewLine + "Query: " + batchJob[i].TagQuery;

                                batchJob[i].isError = true;
                                batchJob[i].isCompleted = false;

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
                                                DanbooruPostDaoOption option = new DanbooruPostDaoOption()
                                                {
                                                    Provider = _currProvider,
                                                    Query = query,
                                                    SearchTags = batchJob[i].TagQuery,
                                                    Url = url,
                                                    Referer = "",
                                                    BlacklistedTags = TagBlacklist,
                                                    BlacklistedTagsRegex = TagBlacklistRegex,
                                                    BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                                                    IgnoredTags = TagIgnore,
                                                    IgnoredTagsRegex = TagIgnoreRegex,
                                                    IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                                                };
                                                var resp = new DanbooruPostDao(response, option);
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
                                batchJob[i].Status = " ==> Error: " + (string.IsNullOrWhiteSpace(responseMessage) ? ex.Message : responseMessage) + Environment.NewLine;
                                if (!string.IsNullOrWhiteSpace(responseMessage)) UpdateLog("DoBatchJob", "Server Message: " + responseMessage, ex);
                                else UpdateLog("DoBatchJob", "Error: " + message, ex);

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
                                    myArray[0] = batchJob[i].ProcessedTotal;
                                    if (d != null)
                                    {
                                        myArray[1] = d.PostCount < batchJob[i].Limit ? d.PostCount : batchJob[i].Limit;
                                    }
                                    else
                                    {
                                        myArray[1] = -1;
                                    }
                                    BeginInvoke(del2, myArray);
                                }
                            }
                            ++batchJob[i].CurrentPage;
                        } while (flag);

                        UpdateLog("DoBatchJob", "Batch Job #" + i + ": Done");
                        if (batchJob[i].isError)
                        {
                            batchJob[i].isCompleted = false;
                        }
                        else
                        {
                            batchJob[i].isCompleted = true;
                        }
                        BeginInvoke(del);
                    }
                }
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

        private int DoDownloadBatch(string targetUrl, DanbooruBatchJob job, DanbooruPost post, string filename)
        {
            UpdateLog("DoBatchJob", "Download: " + targetUrl);
            _clientBatch.Referer = post.Referer;

            int currRetry = 0;
            int maxRetry = Convert.ToInt32(txtRetry.Text);
            int delay = Convert.ToInt32(txtDelay.Text);

            while (currRetry <= maxRetry)
            {
                try
                {
                    var filename2 = filename + ".!tmp";
#if DEBUG
                    Thread.Sleep(100);
#else
                    if (File.Exists(filename2))
                    {
                        UpdateLog("DoBatchJob", "Deleting temporary file: " + filename2);
                        File.Delete(filename2);
                    }
                    _clientBatch.DownloadFile(targetUrl, filename2);
                    File.Move(filename2, filename);
                    UpdateLog("DoBatchJob", "Saved To: " + filename);
#endif
                    ++job.Downloaded;

                    // write to text file for downloaded file.
                    Helper.WriteTextFile(filename + Environment.NewLine);

                    return 1;
                }
                catch (System.Net.WebException ex)
                {
                    if (currRetry < maxRetry && cbxAbortOnError.Checked) throw;
                    else
                    {
                        var message = ex.Message;
                        if (ex.InnerException != null) message = ex.InnerException.Message;
                        UpdateLog("DoBatchJob", "Error Download Batch Image (" + currRetry + " of " + maxRetry + "): " + message + " Wait for " + delay + "s.", ex);

                        for (int wait = 0; wait < delay; wait++)
                        {
                            //UpdateLog("DoBatchJob", "Wait for " + wait + " of " + delay);
                            Thread.Sleep(1000);
                        }
                        UpdateLog("DoBatchJob", "Retrying...");
                    }
                    ++currRetry;
                    if (currRetry > Convert.ToInt32(txtRetry.Text)) ++job.Error;
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

                        if (post.Provider.BoardType == BoardType.Danbooru)
                        {
                            var tempPost = SankakuComplexParser.ParsePost(post, html);
                            post.FileUrl = tempPost.FileUrl;
                            post.PreviewUrl = tempPost.PreviewUrl;
                        }
                        else if (post.Provider.BoardType == BoardType.Gelbooru)
                        {
                            var tempPost = GelbooruHtmlParser.ParsePost(post, html);
                            post.FileUrl = tempPost.FileUrl;
                            post.PreviewUrl = tempPost.PreviewUrl;
                        }
                        else
                        {
                            UpdateLog("DoBatchJob", "No HTML Parser available for : " + post.Provider.Name + "(" + post.Provider.BoardType.ToString() + ")");
                        }
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
        private DanbooruPostDao GetBatchImageList(String url, String query, DanbooruBatchJob job)
        {
            DanbooruPostDao d = null;
            int currRetry = 0;
            int maxRetry = Convert.ToInt32(txtRetry.Text);
            int delay = Convert.ToInt32(txtDelay.Text);
            while (currRetry < maxRetry)
            {
                try
                {
                    var strs = _clientBatch.DownloadString(url);
                    using (MemoryStream ms = new MemoryStream(_clientBatch.DownloadData(url)))
                    {
                        DanbooruPostDaoOption option = new DanbooruPostDaoOption()
                        {
                            Provider = job.Provider,
                            Query = query,
                            SearchTags = job.TagQuery,
                            Referer = url,
                            BlacklistedTags = TagBlacklist,
                            BlacklistedTagsRegex = TagBlacklistRegex,
                            BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                            IgnoredTags = TagIgnore,
                            IgnoredTagsRegex = TagIgnoreRegex,
                            IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                        };
                        d = new DanbooruPostDao(ms, option);
                    }
                    break;
                }
                catch (System.Net.WebException ex)
                {
                    ++currRetry;
                    UpdateLog("DoBatchJob", "Error Getting List (" + currRetry + " of " + maxRetry + "): " + ex.Message + " Wait for " + delay + "s.", ex);
                    for (int wait = 0; wait < delay; ++wait)
                    {
                        //UpdateLog("DoBatchJob", "Wait for " + wait + " of " + delay);
                        Thread.Sleep(1000);
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
            uint res = 0;
            if (!String.IsNullOrWhiteSpace(txtPage.Text) && !UInt32.TryParse(txtPage.Text, out res))
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
                DanbooruPostDaoOption option = new DanbooruPostDaoOption()
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
                    ShowMessage("Filename Length Limit", "Maximum " + MAX_FILENAME_LENGTH.ToString() + "!");
                    txtFilenameLength.Text = MAX_FILENAME_LENGTH.ToString();
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
                XmlSerializer serializer = new XmlSerializer(typeof(BindingList<DanbooruPost>));
                //XmlIncludeAttribute include = new XmlIncludeAttribute(typeof(Image));
                StreamWriter writer = new StreamWriter(saveFileDialog2.FileName);

                serializer.Serialize(writer, _downloadList);

                writer.Close();
            }
        }

        private void btnLoadDownloadList_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BindingList<DanbooruPost>));
                StreamReader reader = new StreamReader(openFileDialog2.FileName);

                _downloadList = (BindingList<DanbooruPost>)serializer.Deserialize(reader);
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
                    if (_downloadList.IndexOf(_postsDao.Posts[cell.RowIndex]) < 0)
                    {
                        _downloadList.Add(_postsDao.Posts[cell.RowIndex]);

                        if (String.IsNullOrEmpty(_postsDao.Posts[cell.RowIndex].FileUrl))
                        {
                            ResolveFileUrl(_postsDao.Posts[cell.RowIndex]);
                        }
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
                    batchJob.RemoveAt(i);
                    --i;
                }
            }
            dgvBatchJob.Refresh();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            batchJob.Clear();
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
            Process.Start(@"http://nandaka.wordpress.com/tag/danbooru-batch-download/");
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
                case PreferredMethod.Html: queryStr = _currProvider.QueryStringHtml;
                    break;

                case PreferredMethod.Json: queryStr = _currProvider.QueryStringJson;
                    break;

                case PreferredMethod.Xml: queryStr = _currProvider.QueryStringXml;
                    break;

                default: queryStr = "Invalid Prefered Method!";
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
            int i;
            if (!Int32.TryParse(txtArtistTagGrouping.Text, out i))
            {
                MessageBox.Show("Invalid value: " + txtArtistTagGrouping.Text, "txtArtistTagGrouping");
                txtArtistTagGrouping.Text = "5";
            }
        }

        private void txtCopyTagGrouping_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (!Int32.TryParse(txtCopyTagGrouping.Text, out i))
            {
                MessageBox.Show("Invalid value: " + txtCopyTagGrouping.Text, "txtCopyTagGrouping");
                txtCopyTagGrouping.Text = "5";
            }
        }

        private void txtCharaTagGrouping_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (!Int32.TryParse(txtCharaTagGrouping.Text, out i))
            {
                MessageBox.Show("Invalid value: " + txtCharaTagGrouping.Text, "txtCharaTagGrouping");
                txtCharaTagGrouping.Text = "5";
            }
        }

        private void txtCircleTagGrouping_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (!Int32.TryParse(txtCircleTagGrouping.Text, out i))
            {
                MessageBox.Show("Invalid value: " + txtCircleTagGrouping.Text, "txtCircleTagGrouping");
                txtCircleTagGrouping.Text = "5";
            }
        }

        private void txtFaultsTagGrouping_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (!Int32.TryParse(txtFaultsTagGrouping.Text, out i))
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
            int port = 0;
            bool result = Int32.TryParse(txtProxyPort.Text, out port);
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
            if (!String.IsNullOrWhiteSpace(txtLog.SelectedText))
                Clipboard.SetText(txtLog.SelectedText);
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
                        int page;
                        var result = Int32.TryParse(txtPage.Text, out page);
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
                    int page;
                    var result = Int32.TryParse(txtPage.Text, out page);
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
                XmlSerializer serializer = new XmlSerializer(typeof(BindingList<DanbooruBatchJob>));
                //XmlIncludeAttribute include = new XmlIncludeAttribute(typeof(Image));
                StreamWriter writer = new StreamWriter(saveFileDialog2.FileName);

                serializer.Serialize(writer, batchJob);

                writer.Close();
            }
        }

        private void btnLoadList_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BindingList<DanbooruBatchJob>));
                StreamReader reader = new StreamReader(openFileDialog2.FileName);

                batchJob = (BindingList<DanbooruBatchJob>)serializer.Deserialize(reader);
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
    }
}