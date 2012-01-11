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

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        List<DanbooruProvider> _listProvider;
        DanbooruProvider _currProvider;
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

        bool _isMorePost = true;

        int _loadedThumbnail;
        int _retry;

        public FormMain()
        {
            InitializeComponent();
            
            // Get assembly version
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += fvi.ProductVersion;

            // init webclients
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
                

            // Auto populate the Provider List
            DanbooruProviderDao dao = new DanbooruProviderDao();
            _listProvider = dao.GetAllProvider();
            cbxProvider.DataSource = _listProvider;
            cbxProvider.DisplayMember = "Name";
            cbxProvider.ValueMember = "Preferred";
            // end auto populate

            //Auto populate Order and Rating
            cbxOrder.DataSource = new BindingSource(Constants.OrderBy, null);
            cbxOrder.DisplayMember = "Key";
            cbxOrder.ValueMember = "Value";
            cbxRating.DataSource = new BindingSource(Constants.Rating, null);
            cbxRating.DisplayMember = "Key";
            cbxRating.ValueMember = "Value";

            txtFilenameHelp.Text = "%provider% = Provider Name" + Environment.NewLine +
                                    "%id% = Image ID" + Environment.NewLine +
                                    "%tags% = Image Tags" + Environment.NewLine +
                                    "%rating% = Image Rating" + Environment.NewLine +
                                    "%md5% = MD5 Hash" + Environment.NewLine +
                                    "%query% = Query String" + Environment.NewLine +
                                    "%searchtag% = Search tag";

            pbLoading.Image = DanbooruDownloader3.Properties.Resources.AJAX_LOADING;
            _retry = Convert.ToInt32( txtRetry.Text );

            CheckProxyLogin();
            SetProxy(chkUseProxy.Checked, txtProxyAddress.Text, Convert.ToInt32(txtProxyPort.Text), txtProxyUsername.Text, txtProxyPassword.Text);
        }

        public void PrintFlags()
        {
            tsFlag.Text = "isPaused = " + _isPaused.ToString() + " | isLoadingList = " + _isLoadingList.ToString() + " | isMorePost = " + _isMorePost.ToString() + " | isLoadingThumb = " + _isLoadingThumb.ToString();
        }

        public bool CheckListGrid()
        {
            return dgvList.Rows.Count > 0 ? true : false;
        }

        public bool CheckDownloadGrid()
        {
            return dgvDownload.Rows.Count > 0 ? true : false;
        }

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
            _isDownloading = true;

            if (CheckDownloadGrid())
            {
                if (_isPaused)
                {
                    DialogResult result =  MessageBox.Show("Paused." + Environment.NewLine + "Click OK to continue.","Download",MessageBoxButtons.OKCancel);
                    _isPaused = false;

                    EnableControls(true);

                    if (result.Equals(DialogResult.Cancel))
                    {
                        tsStatus.Text = "Canceled.";
                        return;
                    }
                }

                tsStatus.Text = "Downloading Post #" + row.Index;
                
                if (!_downloadList[row.Index].Completed)
                {
                    row.Selected = true;

                    if (chkAutoFocus.Checked) dgvDownload.FirstDisplayedScrollingRowIndex = row.Index;

                    string url = (string)row.Cells["colUrl2"].Value;
                    
                    string filename = MakeFilename(txtSaveFolder.Text , txtFilenameFormat.Text, _downloadList[row.Index], Convert.ToInt32(txtFilenameLength.Text)) + url.Substring(url.LastIndexOf('.'));

                    if (!chkOverwrite.Checked && File.Exists(filename))
                    {
                        row.Cells["colProgress2"].Value = "File exists!";

                        try
                        {
                            DownloadRows(dgvDownload.Rows[row.Index + 1]);//dgvDownload.Rows.GetNextRow(row.Index,DataGridViewElementStates.None)]);
                        }
                        catch (Exception ex) { txtLog.Text += "[DownloadRow] overwrite = false, " + ex.Message; }
                    }
                    else
                    {
                        string dir = filename.Substring(0, filename.LastIndexOf(@"\"));
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                        // the actual download
                        _clientFile.Referer = _downloadList[row.Index].Referer;
                        if (chkPadUserAgent.Checked) _clientFile.UserAgent = PadUserAgent(txtUserAgent.Text);
                        _clientFile.DownloadFileAsync(new Uri(url), filename, row);
                    }
                }
                else
                {
                    DownloadRows(dgvDownload.Rows[row.Index + 1]);
                }
            }
        }

        public void LoadNextList(DanbooruPostDao newPostDao)
        {
            try
            {
                if (newPostDao.Posts.Count > 0)
                {
                    if (_postsDao == null)
                    {
                        LoadList(newPostDao);
                        return;
                    }

                    _isLoadingList = true;

                    txtLog.AppendText("Loading next page..." + Environment.NewLine);

                    int oldCount = _postsDao.Posts.Count;

                    foreach (DanbooruPost po in newPostDao.Posts)
                    {
                        _postsDao.Posts.Add(po);
                    }
                    dgvList.DataSource = _postsDao.Posts;

                    if (chkLoadPreview.Checked && !_clientThumb.IsBusy && !_isLoadingThumb) LoadThumbnailLater(oldCount);

                    tsCount.Text = "| Count = " + _postsDao.Posts.Count;
                    tsTotalCount.Text = "| Total Count = " + newPostDao.PostCount;
                    //isLoadingList = false;
                }
                else
                {
                    _isMorePost = false;
                    _isLoadingList = false;
                    MessageBox.Show("No more posts!","Listing");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadNextList" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException.Message, "Listing");
                throw ex;
            }

        }

        private void LoadList(DanbooruPostDao postDao)
        {
            if (postDao.Posts.Count > 0)
            {
                _isLoadingList = true;

                if (chkAppendList.Checked && _postsDao != null)
                {
                    txtLog.AppendText("Appending..." + Environment.NewLine);

                    int oldCount = _postsDao.Posts.Count;

                    foreach (DanbooruPost po in postDao.Posts)
                    {
                        _postsDao.Posts.Add(po);
                    }
                    dgvList.DataSource = _postsDao.Posts;

                    if(chkLoadPreview.Checked && !_clientThumb.IsBusy && !_isLoadingThumb) LoadThumbnailLater(oldCount);
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

        public string GetQueryUrl()
        {
            if (chkGenerate.Checked)
            {
                txtQuery.Text = "";
                //Tags
                txtQuery.Text += txtTags.Text.Length > 0 ? "tags="+txtTags.Text.Replace(' ', '+') : "";

                //Rating
                txtQuery.Text += txtQuery.Text.Length > 0 && cbxRating.SelectedIndex > 0 ? "+" : "";
                txtQuery.Text += (txtTags.Text.Length <= 0 && cbxRating.SelectedIndex > 0? "tags=" : "") + (chkNotRating.Checked && cbxRating.SelectedIndex > 0 ? "-" : "" + cbxRating.SelectedValue);

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
                txtQuery.Text += txtPage.Text.Length > 0 ? "page=" + txtPage.Text : "";
            }

            string query = (rbJson.Checked ? _currProvider.QueryStringJson : _currProvider.QueryStringXml);
            query = query.Replace("%_query%", txtQuery.Text);
            return _currProvider.Url + query;
        }

        private void UpdateStatus()
        {
            tsStatus.Text = "Query URL: " + GetQueryUrl();
        }

        private void LoadThumbnailLater(int i)
        {
            if (i >= _postsDao.Posts.Count)
            {
                _isLoadingThumb = false;
                return;
            }
            // check whether the previous thread still running.
            if (!_clientThumb.IsBusy)
            {
                _isLoadingThumb = true;
                tsProgressBar.Visible = true;
                _loadedThumbnail = i;
                timGifAnimation.Enabled = true;
                _clientThumb.Referer = _postsDao.Posts[i].Referer;
                if (chkPadUserAgent.Checked) _clientThumb.UserAgent = PadUserAgent(txtUserAgent.Text);
                _clientThumb.DownloadDataAsync(new Uri(_postsDao.Posts[_loadedThumbnail].PreviewUrl), _loadedThumbnail);
            }
        }

        private void GetList()
        {
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
                    if (chkPadUserAgent.Checked) _clientList.UserAgent = PadUserAgent(txtUserAgent.Text);
                    _clientList.DownloadFileAsync(new Uri(GetQueryUrl()), saveFileDialog1.FileName, saveFileDialog1.FileName.Clone());
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
                    if (chkPadUserAgent.Checked) _clientList.UserAgent = PadUserAgent(txtUserAgent.Text);
                    _clientList.DownloadDataAsync(new Uri(GetQueryUrl()), GetQueryUrl());
                    tsProgressBar.Visible = true;
                }
            }
        }

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
                                txtPage.Text = "2";
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

        #region batch job helper
        Thread batchJobThread;
        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        BindingList<DanbooruBatchJob> batchJob;

        public void PauseBatchJobs()
        {
            _pauseEvent.Reset();
        }

        public void ResumeBatchJobs()
        {
            _pauseEvent.Set();
        }

        public void StopBatchJobs()
        {
            // Signal the shutdown event
            _shutdownEvent.Set();

            // Make sure to resume any paused threads
            _pauseEvent.Set();

            // Wait for the thread to exit
            batchJobThread.Join();
        }

        private void btnAddBatchJob_Click(object sender, EventArgs e)
        {
            FormAddBatchJob f = new FormAddBatchJob();
            if (f.ShowDialog() == DialogResult.OK)
            {
                DanbooruBatchJob job = f.Job;
                if (batchJob == null) batchJob = new BindingList<DanbooruBatchJob>();
                batchJob.Add(job);
                dataGridView1.DataSource = batchJob;
            }
        }

        private void btnStartBatchJob_Click(object sender, EventArgs e)
        {
            batchJobThread = new Thread(DoBatchJob);
            batchJobThread.Start(batchJob);
            btnStartBatchJob.Enabled = false;
        }
        
        private void btnStopBatchJob_Click(object sender, EventArgs e)
        {
            if (batchJobThread != null && batchJobThread.IsAlive)
            {
                StopBatchJobs();
                //batchJobThread.Abort();
                btnStartBatchJob.Enabled = true;
                dataGridView1.Refresh();
            }
        }

        public void DoBatchJob(object list)
        {
            DoBatchJob((BindingList<DanbooruBatchJob>) list);
        }

        public void DoBatchJob(BindingList<DanbooruBatchJob> batchJob)
        {
            string progressStatus = "Starting...";
            string providerStatus = "";
            UpdateUiDelegate del = new UpdateUiDelegate(UpdateUi);
            for (int i = 0; i < batchJob.Count; i++)
            {
                if (!batchJob[i].isCompleted)
                {
                    //get the list from each provider
                    for (int j = 0; j < batchJob[i].ProviderList.Count; j++)
                    {
                        bool flag = true;
                        int currPage = 0;
                        int totalImgCount = 0;
                        int totalSkipCount = 0;
                        DanbooruPostDao prevDao = null;
                        do
                        {
                            int imgCount = 0;
                            int skipCount = 0;
                            DanbooruProvider p = batchJob[i].ProviderList[j];
                            providerStatus = p.Name;

                            // Construct the query
                            DanbooruPostDao d;
                            bool isXml = false;
                            string url;
                            string query = "tags=" + batchJob[i].TagQuery;
                            if (batchJob[i].Rating != null) query += (batchJob[i].TagQuery == null ? "" : "+") + batchJob[i].Rating;
                            if (batchJob[i].Limit <= 0) batchJob[i].Limit = p.DefaultLimit;
                            query += "&limit=" + batchJob[i].Limit;
                            
                            if (batchJob[i].Page <= 0) batchJob[i].Page = 1;
                            query += "&page=" + (batchJob[i].Page + currPage);
                            
                            if (p.Preferred == PreferredMethod.Xml)
                            {
                                url = p.Url + p.QueryStringXml.Replace("%_query%", query);
                                isXml = true;
                            }
                            else
                            {
                                url = p.Url + p.QueryStringJson.Replace("%_query%", query);
                            }

                            // Get the image list
                            try
                            {
                                progressStatus = "Getting list for page " + (batchJob[i].Page + currPage);
                                batchJob[i].Status = providerStatus + Environment.NewLine + progressStatus;
                                BeginInvoke(del);

                                //load the list
                                MemoryStream ms = new MemoryStream(_clientBatch.DownloadData(url));
                                d = new DanbooruPostDao(ms, p.Name, query, batchJob[i].TagQuery, url, isXml);

                                if (prevDao != null)
                                {
                                    // identical data returned, probably no more new image.
                                    if (prevDao.RawData.Equals(d.RawData))
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                prevDao = d;

                                if (d.Posts.Count == 0) flag = false;
                                providerStatus += "(p:" + (batchJob[i].Page + currPage) + ") (tot:" + d.Posts.Count + ") ";

                                foreach (DanbooruPost post in d.Posts)
                                {
                                    // thread handling
                                    _pauseEvent.WaitOne(Timeout.Infinite);

                                    if (_shutdownEvent.WaitOne(0))
                                    {
                                        batchJob[i].Status = providerStatus + "(dl:" + totalImgCount + ") (sk:" + totalSkipCount + ")" + Environment.NewLine + "Aborted";
                                        return;
                                    }

                                    progressStatus = "Downloading: ";
                                    batchJob[i].Status = providerStatus + "(dl:" + totalImgCount + ") (sk:" + totalSkipCount + ")" + Environment.NewLine + progressStatus + post.FileUrl;
                                    BeginInvoke(del);
                                    string filename = MakeFilename(txtSaveFolder.Text, batchJob[i].SaveFolder, post, Convert.ToInt16(txtFilenameLength.Text)) + post.FileUrl.Substring(post.FileUrl.LastIndexOf('.'));

                                    bool download = true;
                                    // check if exist
                                    if (!chkOverwrite.Checked)
                                    {
                                        if (File.Exists(filename))
                                        {
                                            ++skipCount;
                                            ++totalSkipCount;
                                            download = false;
                                        }
                                    }
                                    // download
                                    if (download)
                                    {
                                        if (chkPadUserAgent.Checked) _clientBatch.UserAgent = PadUserAgent(txtUserAgent.Text);
                                        _clientBatch.DownloadFile(post.FileUrl, filename);
                                        ++imgCount;
                                        ++totalImgCount;
                                    }

                                    // check if over limit
                                    if (skipCount + imgCount >= d.Posts.Count)
                                    {
                                        if (totalSkipCount + totalImgCount >= batchJob[i].Limit)
                                        {
                                            flag = false;
                                        }
                                        break;
                                    }
                                }
                                providerStatus += "(dl:" + totalImgCount + ") (sk:" + totalSkipCount + ")" + " done." + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                providerStatus += " Error: " + ex.Message + Environment.NewLine;
                            }

                            ++currPage;
                        } while (flag); 
                    }
                    batchJob[i].Status = providerStatus + Environment.NewLine + "All Done.";
                    batchJob[i].isCompleted = true;
                    BeginInvoke(del);
                }
            }
            ToggleBatchJobButtonDelegate bjd = new ToggleBatchJobButtonDelegate(ToggleBatchJobButton);
            BeginInvoke(bjd, new object[] { true });
        }

        public delegate void UpdateUiDelegate();
        public void UpdateUi()
        {
            dataGridView1.Refresh();
        }

        public delegate void ToggleBatchJobButtonDelegate(bool enabled);
        public void ToggleBatchJobButton(bool enabled)
        {
            btnStartBatchJob.Enabled = enabled;
        }

        #endregion
        
        #region windows form events
        private void cbxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProvider.SelectedValue.Equals(PreferredMethod.Json)) rbJson.Checked = true;
            else rbXml.Checked = true;

            _currProvider = _listProvider[cbxProvider.SelectedIndex];
            UpdateStatus();
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
                DanbooruPostDao newPosts = new DanbooruPostDao(txtListFile.Text);

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
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) dgvList.Rows[i].Cells["colNumber"].Value = i + 1;

        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvList.Columns["colUrl"].Index || e.ColumnIndex == dgvList.Columns["colSourceUrl"].Index)
            {
                // Load web browser to the image url
                Process.Start(dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
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

                if (i > 200)
                {
                    MessageBox.Show("Maximum 200!", "Filename Length Limit");
                    txtFilenameLength.Text = "200";
                }

            }
            catch (Exception)
            {
                txtFilenameLength.Text = "200";
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

        private void FormMain_Load(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Show();
            }
            if (chkMinimizeTray.Checked) notifyIcon1.Visible = true;
            else notifyIcon1.Visible = false;
        }

        private void EnableControls(bool enabled)
        {
            gbxSearch.Enabled = enabled;
            gbxList.Enabled = enabled;
            gbxDanbooru.Enabled = enabled;
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
            if (!_clientFile.IsBusy)
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
                    btnPauseDownload.Enabled = true;
                }
            }
        }

        private void btnPauseDownload_Click(object sender, EventArgs e)
        {
            _isPaused = true;
            EnableControls(false);

            btnPauseDownload.Enabled = false;
            tsStatus.Text = "Pausing...";
        }

        private void btnCancelDownload_Click(object sender, EventArgs e)
        {
            if (_clientFile.IsBusy)
            {
                _clientFile.CancelAsync();
            }
        }

        private void btnSaveDownloadList_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BindingList<DanbooruPost>));
                XmlIncludeAttribute include = new XmlIncludeAttribute(typeof(Image));
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
        #endregion        
    }
}
