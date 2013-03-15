using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using DanbooruDownloader3.DAO;
using System.Threading;
using DanbooruDownloader3.Entity;
using System.Threading.Tasks;

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        int currThumbRetry = 0;
        //int currFileRetry = 0;

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            object[] param = (object[])e.Argument;
            Stream ms = (Stream)param[0];
            DanbooruPostDaoOption option = (DanbooruPostDaoOption)param[1];
            DanbooruPostDao newPosts = new DanbooruPostDao(ms, option);
            e.Result = newPosts;
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadList((DanbooruPostDao)e.Result);
            tsProgressBar.Visible = false;
            btnGet.Enabled = true;
            btnNextPage.Enabled = true;
            btnPrevPage.Enabled = true;
            btnListCancel.Enabled = false;
            _isLoadingList = false;
            tsStatus.Text = "Ready.";
        }
        

        #region clientList event handler
        void clientList_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                Program.Logger.Debug("Download list completed");
                tsProgressBar.Visible = false;
                MemoryStream ms = new MemoryStream(e.Result);
                DanbooruPostDaoOption option = new DanbooruPostDaoOption()
                {
                    Provider = _currProvider,
                    //Url = txtListFile.Text,
                    Referer = _clientList.Referer,
                    Query = txtQuery.Text,
                    SearchTags = txtTags.Text,
                    BlacklistedTags = TagBlacklist,
                    BlacklistedTagsRegex = TagBlacklistRegex,
                    BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked, 
                    IgnoredTags = TagIgnore,
                    IgnoredTagsRegex = TagIgnoreRegex,
                    IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                };
                //DanbooruPostDao newPosts = new DanbooruPostDao(ms,option);
                //LoadList(newPosts);
                tsStatus.Text = "Loading downloaded list...";
                tsProgressBar.Style = ProgressBarStyle.Marquee;
                tsProgressBar.Visible = true;
                _isLoadingList = true;
                backgroundWorker1 = new BackgroundWorker();
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                backgroundWorker1.RunWorkerAsync(new object[] { ms, option });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                    if (ex.InnerException.GetType() == typeof(System.Net.WebException))
                    {
                        var wex = (System.Net.WebException)ex.InnerException;
                        var status = wex.Response != null? wex.Response.Headers["Status"] : null;
                        if (wex.Status == WebExceptionStatus.ProtocolError)
                        {
                            if (status == "403")
                            {
                                message += Environment.NewLine + "Please check your login information.";
                            }
                            else
                            {
                                using (var response = wex.Response.GetResponseStream())
                                {
                                    if (response != null)
                                    {
                                        DanbooruPostDaoOption option = new DanbooruPostDaoOption()
                                        {
                                            Provider = _currProvider,
                                            //Url = "",
                                            Referer = "",
                                            Query = "",
                                            SearchTags = "",
                                            BlacklistedTags = TagBlacklist,
                                            BlacklistedTagsRegex = TagBlacklistRegex,
                                            BlacklistedTagsUseRegex = chkBlacklistTagsUseRegex.Checked,
                                            IgnoredTags = TagIgnore,
                                            IgnoredTagsRegex = TagIgnoreRegex,
                                            IgnoredTagsUseRegex = chkIgnoreTagsUseRegex.Checked
                                        };
                                        var resp = new DanbooruPostDao(response, option);
                                        message = "Server Message: " + resp.ResponseMessage;
                                    }
                                }
                            }
                        }
                    }
                }
                
                MessageBox.Show(message, "Download List");
                Program.Logger.Error(message, ex);

                UpdateLog("clientList_DownloadDataCompleted", "Error: " + (ex.InnerException == null ? ex.Message : ex.InnerException.Message), ex);
                UpdateLog("clientList_DownloadDataCompleted", "Referer: " + _clientList.Referer);

                chkAutoLoadNext.Checked = false;
                btnGet.Enabled = true;
                btnListCancel.Enabled = false;
                _isLoadingList = false;
            }
        }
        
        void clientList_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            tsProgressBar.Visible = false;
            UpdateLog("clientList_DownloadFileCompleted", "Download Complete: " + e.UserState);
            txtListFile.Text = saveFileDialog1.FileName;
            gbxSearch.Enabled = true;
            gbxList.Enabled = true;

            btnGet.Enabled = true;
            btnListCancel.Enabled = false;

            if (chkAutoLoadList.Checked)
            {
                DanbooruPostDaoOption option = new DanbooruPostDaoOption()
                {
                    Provider = _currProvider,
                    Url = txtListFile.Text,
                    Referer = _clientList.Referer,
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
            _isLoadingList = false;
        }

        void clientList_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            tsProgressBar.Visible = true;
            txtLog.AppendText("[clientList]" + e.UserState + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete..." + Environment.NewLine);
            if (e.TotalBytesToReceive > 0)
            {
                tsProgressBar.Style = ProgressBarStyle.Continuous;
                if (tsProgressBar.Maximum <= e.ProgressPercentage)
                {
                    tsProgressBar.Value = tsProgressBar.Maximum;
                }
                else
                {
                    tsProgressBar.Maximum = 100;
                    tsProgressBar.Value = e.ProgressPercentage;
                }
            }
            else tsProgressBar.Style = ProgressBarStyle.Marquee;
        }
        #endregion

        #region clientThumb event handler
        void clientThumb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                tsProgress2.Visible = true;
                txtLog.AppendText("[clientThumbnail]" + e.UserState + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete..." + Environment.NewLine);
                if (e.TotalBytesToReceive > 0)
                {
                    tsProgress2.Style = ProgressBarStyle.Continuous;
                    tsProgress2.Maximum = 100;
                    tsProgress2.Value = e.ProgressPercentage;
                }
                else tsProgress2.Style = ProgressBarStyle.Marquee;
            }
            catch (Exception ex)
            {
                UpdateLog("clientThumb_DownloadProgressChanged", "Error: " + e.UserState + " " + ex.Message, ex);
            }
        }

        void clientThumb_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                tsProgress2.Visible = false;
                timGifAnimation.Enabled = false;

                if (_postsDao == null)
                {
                    _isLoadingThumb = false;
                    currThumbRetry = 0;
                    return;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(e.Result);
                    int i = (int)e.UserState;
                    
                    _postsDao.Posts[i].ThumbnailImage = new Bitmap(ms);
                    if (dgvList.Columns["colPreview"].Width < _postsDao.Posts[i].ThumbnailImage.Width)
                    {
                        dgvList.Columns["colPreview"].Width = _postsDao.Posts[i].ThumbnailImage.Width;
                    }
                    dgvList.Refresh();

                    i++;
                    if (chkLoadPreview.Checked)
                    {
                        currThumbRetry = 0;
                        LoadThumbnailLater(i);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateLog("clientThumb_DownloadDataCompleted", "Error" + e.UserState + " " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message), ex);
                int i = (int)e.UserState;
                //
                if (chkLoadPreview.Checked)
                {
                    if (currThumbRetry < _retry)
                    {
                        currThumbRetry++;
                        Thread.Sleep(_delay * 1000);
                    }
                    else
                    {
                        _postsDao.Posts[i].ThumbnailImage = DanbooruDownloader3.Properties.Resources.NOT_AVAILABLE;
                        i++;
                        currThumbRetry = 0;
                    }
                    LoadThumbnailLater(i);
                }
                if (FormMain.Debug) throw;
            }

        }
        #endregion

        #region clientDownloadFile event handler
        void clientFile_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            tsProgressBar.Visible = true;
            tsProgress2.Visible = true;

            if (e.UserState != null)
            {
                DataGridViewRow row = (DataGridViewRow)e.UserState;
                var startTime = (DateTime)row.Cells["colDownloadStart2"].Value;
                double speed = e.BytesReceived / (TimeSpan.FromTicks(DateTime.Now.AddTicks(-startTime.Ticks).Ticks).TotalSeconds * 1024);
                txtLog.AppendText("[clientFileDownload]" + row.Index + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete..." + Environment.NewLine);
                row.Cells["colProgress2"].Value = String.Format("Downloading: {0:0.00} of {1:0.00} KB ({2:0.00} KB/s)", e.BytesReceived / 1024, e.TotalBytesToReceive / 1024, speed);
            }
            if (e.TotalBytesToReceive > 0)
            {
                tsProgressBar.Style = ProgressBarStyle.Continuous;
                tsProgressBar.Maximum = 100;
                tsProgressBar.Value = e.ProgressPercentage;
            }
            else tsProgressBar.Style = ProgressBarStyle.Marquee;
        }

        void clientFile_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)e.UserState;

            var status = e.Error == null ? "OK" : e.Error.InnerException == null ? e.Error.Message : e.Error.InnerException.Message;
            UpdateLog("clientFileDownload", "Url: " + row.Cells["colUrl2"].Value.ToString() + " Completed, Status: " + status);
            UpdateLog("clientFileDownload", "Ref: " + _clientFile.Referer);
            
            if (e.Error == null)
            {
                Program.Logger.Info("[clientFileDownload] Download complete: " + row.Cells["colUrl2"].Value.ToString());
                _downloadList[row.Index].Completed = true;
                row.DefaultCellStyle.BackColor = Color.Green;

                string filename = row.Cells["colFilename"].Value.ToString();
                try
                {
                    if (!String.IsNullOrWhiteSpace(filename))
                    {
                        File.Move(filename + ".!tmp", filename);
                    }
                    row.Cells["colProgress2"].Value += Environment.NewLine + "Complete, Status: OK";
                }
                catch (IOException ex)
                {
                    UpdateLog("clientFileDownload", "Cannot rename completed file: " + ex.Message, ex);
                    row.Cells["colProgress2"].Value += Environment.NewLine + "Complete, Status: Cannot rename completed file";
                }
            }
            else
            {
                row.Cells["colProgress2"].Value += Environment.NewLine + "Error, Status: " + status;                
                Program.Logger.Error("Download Error: " + row.Cells["colUrl2"].Value.ToString(), e.Error);
                row.DefaultCellStyle.BackColor = Color.Red;
            }

            // check if the last row
            if (row.Index < dgvDownload.Rows.GetLastRow(DataGridViewElementStates.None))
            {
                DownloadRows(dgvDownload.Rows[row.Index + 1]);
            }
            else
            {
                _isPaused = false;
                _isDownloading = false;
                ShowMessage("Download List", "Download Complete!");
                EnableDownloadControls(true);
                tsProgress2.Visible = false;
            }
        }
        #endregion

        #region clientBatch event handler

        //void _clientBatch_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    tsProgressBar.Visible = true;
        //    tsProgressBar.Value = e.ProgressPercentage;
        //}

        //void _clientBatch_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        //{
            

        //    tsProgressBar.Visible = false;
        //}
        #endregion

    }
}
