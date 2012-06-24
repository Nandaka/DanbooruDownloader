using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;
using DanbooruDownloader3.Utils;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        #region property
        public static bool Debug = false;

        List<DanbooruProvider> _listProvider;
        DanbooruProvider _currProvider;
        DanbooruProviderDao _dao;
        DanbooruPostDao _postsDao;

        BindingList<DanbooruPost> _downloadList;

        ExtendedWebClient _clientList;
        ExtendedWebClient _clientThumb;
        ExtendedWebClient _clientFile;
        ExtendedWebClient _clientBatch;

        bool _isPaused = false;
        bool _isLoadingList = false;
        bool _isLoadingThumb = false;
        bool _isDownloading = false;
        bool _isCanceled = false;

        bool _isMorePost = true;

        int _loadedThumbnail;
        bool _resetLoadedThumbnail = false;
        int _retry;

        public const int MAX_FILENAME_LENGTH = 255;

        List<DanbooruTag> TagBlacklist = new List<DanbooruTag>();
        #endregion

        public FormMain()
        {
            ToggleLogging(Properties.Settings.Default.EnableLogging);
            InitializeComponent();
            
            // Get assembly version
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += fvi.ProductVersion;
            Program.Logger.Info("Starting up " + this.Text);

            #region init webclients
            _clientList = new ExtendedWebClient();
            _clientList.Headers.Add("user-agent", txtUserAgent.Text);
            try
            {
                _clientList.Timeout = Convert.ToInt32(txtTimeout.Text);
            }
            catch (Exception) { _clientList.Timeout = 60000; }
            _clientList.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientList_DownloadProgressChanged);
            _clientList.DownloadFileCompleted += new AsyncCompletedEventHandler(clientList_DownloadFileCompleted);
            _clientList.DownloadDataCompleted += new DownloadDataCompletedEventHandler(clientList_DownloadDataCompleted);

            _clientThumb = new ExtendedWebClient();
            try
            {
                _clientThumb.Timeout = Convert.ToInt32(txtTimeout.Text);
            }
            catch (Exception) { _clientThumb.Timeout = 60000; }
            _clientThumb.Headers.Add("user-agent", txtUserAgent.Text);
            _clientThumb.DownloadDataCompleted += new DownloadDataCompletedEventHandler(clientThumb_DownloadDataCompleted);
            _clientThumb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientThumb_DownloadProgressChanged);

            _clientFile = new ExtendedWebClient();
            try
            {
                _clientFile.Timeout = Convert.ToInt32(txtTimeout.Text);
            }
            catch (Exception) { _clientFile.Timeout = 60000; }
            _clientFile.Headers.Add("user-agent", txtUserAgent.Text);
            _clientFile.DownloadFileCompleted += new AsyncCompletedEventHandler(clientFile_DownloadFileCompleted);
            _clientFile.DownloadProgressChanged += new DownloadProgressChangedEventHandler(clientFile_DownloadProgressChanged);

            _clientBatch = new ExtendedWebClient();
            _clientBatch.Headers.Add("user-agent", txtUserAgent.Text);
            try
            {
                _clientBatch.Timeout = Convert.ToInt32(txtTimeout.Text);
            }
            catch (Exception) { _clientBatch.Timeout = 60000; }
            //_clientBatch.DownloadProgressChanged += new DownloadProgressChangedEventHandler(_clientBatch_DownloadProgressChanged);
            //_clientBatch.DownloadFileCompleted += new AsyncCompletedEventHandler(_clientBatch_DownloadFileCompleted);
            #endregion
            
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
            _retry = Convert.ToInt32( txtRetry.Text );

            CheckProxyLogin();
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);

            SetTagColors();
            ToggleTagsColor();

            SetTagAutoComplete();

            ParseTagBlacklist();

            Program.Logger.Debug(this.Text + " loaded.");

            dgvList.AutoGenerateColumns = false;
            _ImageSize = cbxImageSize.Text;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
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
            _dao = new DanbooruProviderDao();
            _listProvider = _dao.Read();

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
                TagBlacklist = DanbooruTagsDao.Instance.ParseTagsString(tagStr);
            }
            else
            {
                if (TagBlacklist != null) TagBlacklist.Clear();
            }
        }

        /// <summary>
        /// Load list of tags for auto-complete
        /// </summary>
        private void SetTagAutoComplete()
        {
            txtTags.AutoCompleteCustomSource.Clear();
            if (chkTagAutoComplete.Checked)
            {
                var tagAutoComplete = DanbooruTagsDao.Instance.Tags.Tag.Select(x => x.Name).ToArray<String>();
                txtTags.AutoCompleteCustomSource.AddRange(tagAutoComplete);
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
                            _downloadList.Add(_postsDao.Posts[row.Index]);
                            row.Cells["colCheck"].Value = false;
                        }
                    }
                    dgvDownload.AutoGenerateColumns = false;
                    dgvDownload.DataSource = _downloadList;
                }
            }
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

                if (_isPaused)
                {
                    DialogResult result =  MessageBox.Show("Paused." + Environment.NewLine + "Click OK to continue.","Download",MessageBoxButtons.OKCancel);
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
                
                if (!_downloadList[row.Index].Completed)
                {
                    #region download file
                    row.Selected = true;

                    if (chkAutoFocus.Checked) dgvDownload.FirstDisplayedScrollingRowIndex = row.Index;

                    string url = (string)row.Cells["colUrl2"].Value;

                    // no url given
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        row.Cells["colProgress2"].Value = "No file_url, probably deleted.";
                        txtLog.Text += "[DownloadRow] no file_url for row: " + row.Index;
                        Program.Logger.Info("[DownloadRow] no file_url for row: " + row.Index);

                        if (row.Index < dgvDownload.Rows.GetLastRow(DataGridViewElementStates.None))
                        {
                            DownloadRows(dgvDownload.Rows[row.Index + 1]);
                        }
                        else
                        {
                            // no more row
                            _isDownloading = false;
                            _isPaused = false;
                            EnableDownloadControls(true);
                        }
                        return;
                    }

                    if (_downloadList[row.Index].Provider == null) _downloadList[row.Index].Provider = cbxProvider.Text;
                    if (_downloadList[row.Index].Query == null) _downloadList[row.Index].Query = txtQuery.Text;
                    if (_downloadList[row.Index].SearchTags == null) _downloadList[row.Index].SearchTags = txtTags.Text;

                    var format = new DanbooruFilenameFormat() {
                        FilenameFormat = txtFilenameFormat.Text,
                        Limit = Convert.ToInt32(txtFilenameLength.Text),
                        BaseFolder = txtSaveFolder.Text,
                        MissingTagReplacement = txtTagReplacement.Text};
                    string extension = url.Substring(url.LastIndexOf('.'));
                    if (chkRenameJpeg.Checked)
                    {
                        if (extension.EndsWith(".jpeg")) extension = ".jpg";
                    }
                    string filename = Helper.MakeFilename(format, _downloadList[row.Index]) + extension;
                                        
                    if ((!chkOverwrite.Checked && File.Exists(filename)))
                    {
                        row.Cells["colProgress2"].Value = "File exists!";
                        txtLog.Text += "[DownloadRow] File exists: " + filename;
                        Program.Logger.Info("[DownloadRow] File exists: " + filename);
                        if (row.Index < dgvDownload.Rows.GetLastRow(DataGridViewElementStates.None))
                        {
                            DownloadRows(dgvDownload.Rows[row.Index + 1]);
                        }
                        else
                        {
                            // no more row
                            _isDownloading = false;
                            _isPaused = false;
                            EnableDownloadControls(true);
                        }                        
                    }
                    else
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
                        _clientFile.Referer = _downloadList[row.Index].Referer;
                        if (chkPadUserAgent.Checked) _clientFile.UserAgent = Helper.PadUserAgent(txtUserAgent.Text);
                        Program.Logger.Info("[DownloadRow] Downloading " + url);
                        Program.Logger.Info("[DownloadRow] Saved to    " + filename);
                        _clientFile.DownloadFileAsync(new Uri(url), filename2, row);
                    }
                    #endregion
                }
                else if (row.Index < dgvDownload.Rows.GetLastRow(DataGridViewElementStates.None))
                {
                    // do the next row
                    DownloadRows(dgvDownload.Rows[row.Index + 1]);
                }
                else
                {
                    // no more row
                    _isDownloading = false;
                    _isPaused = false;
                    EnableDownloadControls(true);
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
                        txtLog.AppendText("Appending..." + Environment.NewLine);

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
                _isLoadingList = false;
            }
            else
            {
                _isMorePost = false;
                _isLoadingList = false;
                MessageBox.Show("No Posts!","Listing");
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
                if (chkPadUserAgent.Checked) _clientThumb.UserAgent = Helper.PadUserAgent(txtUserAgent.Text);
                _clientThumb.DownloadDataAsync(new Uri(_postsDao.Posts[_loadedThumbnail].PreviewUrl), _loadedThumbnail);
            }
        }

        /// <summary>
        /// Download posts list 
        /// </summary>
        private void GetList()
        {
            string authString = "";
            if (_currProvider.UseAuth)
            {
                authString = "login=" + _currProvider.UserName + "&password_hash=" + Helper.GeneratePasswordHash(_currProvider.Password, _currProvider.PasswordSalt);
            }
            var queryUrl = GetQueryUrl(authString);
            Program.Logger.Info("Getting list: " + Helper.RemoveAuthInfo(queryUrl));

            if (chkSaveQuery.Checked)
            {
                saveFileDialog1.FileName = cbxProvider.Text + " - " + txtTags.Text + " " + txtPage.Text + (rbJson.Checked ? ".json" : ".xml");
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gbxSearch.Enabled = false;
                    gbxList.Enabled = false;

                    tsProgressBar.Value = 0;

                    string referer = _listProvider[cbxProvider.SelectedIndex].Url;
                    _clientList.Referer = referer;
                    if (chkPadUserAgent.Checked) _clientList.UserAgent = Helper.PadUserAgent(txtUserAgent.Text);
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
                    if (chkPadUserAgent.Checked) _clientList.UserAgent = Helper.PadUserAgent(txtUserAgent.Text);
                    _clientList.DownloadDataAsync(new Uri(queryUrl), queryUrl);
                    tsProgressBar.Visible = true;
                    if (chkLoadPreview.Checked && !chkAutoLoadNext.Checked && !chkAppendList.Checked && _clientThumb.IsBusy ) _resetLoadedThumbnail = true;
                }
            }
        }

        /// <summary>
        /// Increase txtPage and get the list.
        /// </summary>
        /// <param name="currRowIndex"></param>
        private void LoadNextPage(int currRowIndex)
        {
            if (_postsDao != null && _isMorePost && chkAutoLoadNext.Checked)
            {
                if (currRowIndex >= dgvList.Rows.Count - 1)
                {
                    if (!_isLoadingList)
                    {
                        if (!_clientList.IsBusy)
                        {
                            tsStatus.Text = "Loading next page...";
                            if (txtPage.Text.Length == 0)
                            {
                                if (_currProvider.BoardType == BoardType.Danbooru) txtPage.Text = "2";
                                if (_currProvider.BoardType == BoardType.Gelbooru) txtPage.Text = "1";
                            }
                            else
                            {
                                txtPage.Text = "" + (Convert.ToInt32(txtPage.Text) + 1);
                            }
                            GetList();
                        }
                    }
                }
            }
        }
                
        #endregion

        #region batch job helper
        Thread batchJobThread;
        ManualResetEvent _shutdownEvent = null;// = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = null;// = new ManualResetEvent(true);
        BindingList<DanbooruBatchJob> batchJob;
        
        public void PauseBatchJobs()
        {
            UpdateLog("[Batch Job]","Pausing");
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

        FormAddBatchJob f;
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
            DoBatchJob((BindingList<DanbooruBatchJob>) list);
        }

        public void DoBatchJob(BindingList<DanbooruBatchJob> batchJob)
        {
            ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
            UpdateUiDelegate del = new UpdateUiDelegate(UpdateUi);
            UpdateUiDelegate2 del2 = new UpdateUiDelegate2(UpdateUi);

            if (batchJob != null)
            {
                UpdateStatus2("Starting Batch Job");
                
                for (int i = 0; i < batchJob.Count; i++)
                {
                    if (!batchJob[i].isCompleted)
                    {
                        UpdateLog("DoBatchJob", "Processing Batch Job#" + i);

                        DanbooruPostDao prevDao = null;
                        bool flag = true;
                        int currPage = 0;
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
                            int currLimit = 0;
                            bool isXml = false;
                            string url;
                            string query = "";

                            #region Construct the query

                            // check if given limit is more than the hard limit
                            if (batchJob[i].Limit > batchJob[i].Provider.HardLimit) currLimit = batchJob[i].Provider.HardLimit;
                            else currLimit = batchJob[i].Limit;

                            batchJob[i].CurrentPage = batchJob[i].StartPage + currPage;

                            if (batchJob[i].Provider.BoardType == BoardType.Shimmie2)
                            {
                                query += batchJob[i].TagQuery;
                                if (!string.IsNullOrWhiteSpace(batchJob[i].TagQuery)) query += "/";
                                query += batchJob[i].CurrentPage;                                
                                url = query;
                            }
                            else
                            {
                                query = "tags=" + batchJob[i].TagQuery;
                                if (batchJob[i].Rating != null) query += (batchJob[i].TagQuery == null ? "" : "+") + batchJob[i].Rating;

                                query += "&limit=" + currLimit;

                                if (batchJob[i].Provider.BoardType == BoardType.Danbooru)
                                {
                                    query += "&page=" + batchJob[i].CurrentPage;
                                }
                                else if (batchJob[i].Provider.BoardType == BoardType.Gelbooru)
                                {
                                    query += "&pid=" + batchJob[i].CurrentPage;
                                }
                            }

                            if (batchJob[i].Provider.Preferred == PreferredMethod.Xml)
                            {
                                url = batchJob[i].Provider.Url + batchJob[i].Provider.QueryStringXml.Replace("%_query%", query);
                                isXml = true;
                            }
                            else
                            {
                                url = batchJob[i].Provider.Url + batchJob[i].Provider.QueryStringJson.Replace("%_query%", query);
                            }

                            if (batchJob[i].Provider.UseAuth)
                            {
                                string authString = "login=" + batchJob[i].Provider.UserName + "&password_hash=" + Helper.GeneratePasswordHash(batchJob[i].Provider.Password, batchJob[i].Provider.PasswordSalt);
                                url = url + "&" + authString;
                            }
                            #endregion

                            try
                            {
                                #region Get and load the image list
                                batchJob[i].Status = "Getting list for page " + batchJob[i].CurrentPage;
                                BeginInvoke(del);
                                UpdateLog("DoBatchJob", "Downloading list: " + url);

                                int currRetry = 0;
                                while (currRetry < Convert.ToInt32(txtRetry.Text))
                                {
                                    try
                                    {
                                        var strs = _clientBatch.DownloadString(url);
                                        using (MemoryStream ms = new MemoryStream(_clientBatch.DownloadData(url)))
                                        {
                                            d = new DanbooruPostDao(ms, batchJob[i].Provider, query, batchJob[i].TagQuery, url, isXml, TagBlacklist);
                                        }
                                        break;
                                    }
                                    catch (System.Net.WebException ex)
                                    {
                                        ++currRetry;
                                        if (currRetry >= Convert.ToInt32(txtRetry.Text)) break; 
                                            //throw new Exception("Cannot load list.", ex);
                                        else
                                        {
                                            UpdateLog("DoBatchJob", "Error (" + currRetry + "): " + ex.Message, ex);
                                        }                                        
                                    }
                                }

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
                                        if (prevDao.RawData.Equals(d.RawData))
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
                                #endregion

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
                                            batchJob[i].Status =" ==> Stopped.";
                                            // toggle button
                                            BeginInvoke(bjd, new object[] { true });
                                            UpdateLog("DoBatchJob", "Batch Job Stopped.");
                                            UpdateStatus2("Batch Job Stopped.");
                                            return;
                                        }

                                        //Choose the correct urls
                                        var targetUrl = post.FileUrl;
                                        if (_ImageSize == "Thumb")
                                        {
                                            targetUrl = post.PreviewUrl;
                                        }
                                        else if (_ImageSize == "Jpeg")
                                        {
                                           targetUrl = post.JpegUrl;
                                        }
                                        else if (_ImageSize == "Sample")
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
                                                MissingTagReplacement = txtTagReplacement.Text
                                            };
                                            string extension = targetUrl.Substring(targetUrl.LastIndexOf('.'));
                                            if (chkRenameJpeg.Checked)
                                            {
                                                if (extension.EndsWith(".jpeg")) extension = ".jpg";
                                            }
                                            filename = Helper.MakeFilename(format, post) + extension;
                                        }
                                        bool download = true;
                                        // check if exist
                                        if (!chkOverwrite.Checked)
                                        {
                                            if (File.Exists(filename))
                                            {
                                                ++skipCount;
                                                ++batchJob[i].Skipped;
                                                download = false;
                                                UpdateLog("DoBatchJob", "Download skipped, file exists: " + filename);
                                            }
                                        }
                                        if (post.Hidden)
                                        {
                                            ++skipCount;
                                            ++batchJob[i].Skipped;
                                            download = false;
                                            UpdateLog("DoBatchJob", "Download skipped, contains blacklisted tag: " + post.Tags + " Url: " + targetUrl);
                                        }
                                        if (String.IsNullOrWhiteSpace(targetUrl))
                                        {
                                            ++skipCount;
                                            ++batchJob[i].Skipped;
                                            download = false;
                                            UpdateLog("DoBatchJob", "Download skipped, ID: " + post.Id + " No file_url, probably deleted");
                                        }

                                        #region download
                                        if (download)
                                        {
                                            if (chkPadUserAgent.Checked) _clientBatch.UserAgent = Helper.PadUserAgent(txtUserAgent.Text);
                                            UpdateLog("DoBatchJob", "Download: " + targetUrl);
                                            _clientBatch.Referer = post.Referer;

                                            currRetry = 0;
                                            while (currRetry <= Convert.ToInt32(txtRetry.Text))
                                            {
                                                try
                                                {
                                                    var filename2 = filename + ".!tmp";
                                                    if (File.Exists(filename2))
                                                    {
                                                        UpdateLog("DoBatchJob", "Deleting temporary file: " + filename2);
                                                        File.Delete(filename2);
                                                    }
                                                    _clientBatch.DownloadFile(targetUrl, filename2);
                                                    File.Move(filename2, filename);
                                                    UpdateLog("DoBatchJob", "Saved To: " + filename);

                                                    ++imgCount;
                                                    ++batchJob[i].Downloaded;
                                                    break;
                                                }
                                                catch (System.Net.WebException ex)
                                                {
                                                    if (currRetry < Convert.ToInt32(txtRetry.Text) && cbxAbortOnError.Checked) throw;
                                                    else
                                                    {
                                                        var message = ex.Message;
                                                        if (ex.InnerException != null) message = ex.InnerException.Message;
                                                        UpdateLog("DoBatchJob", "Error (" + currRetry + "): " + message, ex);
                                                    }
                                                    ++currRetry;
                                                    if (currRetry > Convert.ToInt32(txtRetry.Text)) ++batchJob[i].Error;
                                                }
                                            }
                                        }
                                        #endregion

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
                                                var resp = new DanbooruPostDao(response, _currProvider, query, batchJob[i].TagQuery, url, isXml, TagBlacklist);
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
                            ++currPage;
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

        #endregion
        
        #region windows form events
        private void cbxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProvider.SelectedIndex == -1) return;
            if (cbxProvider.SelectedValue.Equals(PreferredMethod.Json)) rbJson.Checked = true;
            else rbXml.Checked = true;

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
            UpdateStatus();
            txtPage.Text = "";
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
            UpdateStatus();
        }

        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            btnListCancel.Enabled = true;
            btnGet.Enabled = false;
            _isMorePost = true;
            GetList();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (txtListFile.Text.Length > 0)
            {
                DanbooruPostDao newPosts = new DanbooruPostDao(txtListFile.Text, _currProvider, TagBlacklist);

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
            Properties.Settings.Default.Save();
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
            // add row number
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                dgvList.Rows[i].Cells["colNumber"].Value = i + 1;

                var item = dgvList.Rows[i].DataBoundItem as DanbooruPost;
                if (item.Hidden)
                {
                    dgvList.Rows[i].DefaultCellStyle.BackColor = Helper.ColorBlacklisted;
                }
            }
        }

        // http://img14.pixiv.net/img/leucojum-aest/17661193_p3.jpg
        Regex pixivUrl = new Regex(@"img.*pixiv.*\/(\d+).*\.");

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
                if( match.Success) 
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
                dgvList.Rows[_loadedThumbnail].Cells["colPreview"].Value = pbLoading.Image;
                dgvList.InvalidateCell(dgvList.Columns["colPreview"].Index, _loadedThumbnail);
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
            LoadNextPage(dgvList.Rows.Count - 1);
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
            if (dgvList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvList.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colCheck"];
                    chk.Value = true;
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
                    MessageBox.Show("Maximum " + MAX_FILENAME_LENGTH.ToString() + "!", "Filename Length Limit");
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
            dgvDownload.Rows[e.RowIndex].Cells["colIndex"].Value = e.RowIndex + 1;
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
                            MessageBox.Show("Please select save folder!", "Save Folder");
                            return;
                        }

                    }
                    if (txtSaveFolder.Text.Length > 0)
                    {
                        DownloadRows(dgvDownload.Rows[0]);
                        EnableDownloadControls(false);
                    }
                }
                else
                {
                    MessageBox.Show("No image to download!", "Download List");
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
                    }
                }
            }
        }

        private void chkProxyLogin_CheckedChanged(object sender, EventArgs e)
        {
            CheckProxyLogin();
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
                    if (form.IsModified)
                    {
                        _dao.Save(form.Providers);
                        LoadProviderList();                        
                    }
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

        private void chkTagAutoComplete_CheckedChanged(object sender, EventArgs e)
        {
            SetTagAutoComplete();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DownloadTagsForm form = new DownloadTagsForm();
            form.ShowDialog();
            SetTagAutoComplete();
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
        /// Manual query string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            tsStatus.Text = "Query URL: " + _currProvider.Url + "?" + txtQuery.Text;
            tsStatus.Text = tsStatus.Text.Replace("&", "&&");
        } 

        private void chkLogging_CheckedChanged(object sender, EventArgs e)
        {
            ToggleLogging(chkLogging.Checked);
        }
        #endregion

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
    }
}
