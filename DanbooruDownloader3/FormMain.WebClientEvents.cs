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

namespace DanbooruDownloader3
{
    public partial class FormMain : Form
    {
        int currThumbRetry = 0;
        //int currFileRetry = 0;

        #region clientList event handler
        void clientList_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                tsProgressBar.Visible = false;
                MemoryStream ms = new MemoryStream(e.Result);

                DanbooruPostDao newPosts = new DanbooruPostDao(ms, _currProvider, txtQuery.Text, txtTags.Text, _clientList.Referer, rbXml.Checked);
                
                if (chkAutoLoadNext.Checked) LoadNextList(newPosts);
                else LoadList(newPosts);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.GetType() == typeof(System.Net.WebException) && !ex.InnerException.Message.Contains("403"))
                    {
                        var wex = (System.Net.WebException)ex.InnerException;
                        
                        var resp = new DanbooruPostDao(wex.Response.GetResponseStream(), _currProvider, "", "", "", rbXml.Checked);
                        wex.Response.GetResponseStream().Close();
                        MessageBox.Show("Server Message: " + resp.ResponseMessage, "Download List");
                        
                    }
                    else MessageBox.Show(ex.InnerException.Message, "Download List");
                }
                else MessageBox.Show(ex.Message, "Download List");

                txtLog.Text += "clientList_DownloadDataCompleted(): " + (ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                txtLog.Text += _clientList.Referer + Environment.NewLine;

                chkAutoLoadNext.Checked = false;
            }

            btnGet.Enabled = true;
            btnListCancel.Enabled = false;
            _isLoadingList = false;
            //PrintFlags();
        }

        void clientList_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            tsProgressBar.Visible = false;
            txtLog.AppendText("Download Complete: " + e.UserState + Environment.NewLine);
            txtListFile.Text = saveFileDialog1.FileName;
            gbxSearch.Enabled = true;
            gbxList.Enabled = true;

            btnGet.Enabled = true;
            btnListCancel.Enabled = false;

            if (chkAutoLoadList.Checked)
            {
                DanbooruPostDao newPosts = new DanbooruPostDao(txtListFile.Text, _currProvider);
                newPosts.Referer = _clientList.Referer;
                LoadList(newPosts);
            }
            _isLoadingList = false;
        }

        void clientList_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            tsProgressBar.Visible = true;
            txtLog.AppendText("[clientList]" + e.UserState + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete..." + Environment.NewLine);
            tsProgressBar.Value = e.ProgressPercentage;
        }
        #endregion

        #region clientThumb event handler
        void clientThumb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                tsProgress2.Visible = true;
                txtLog.AppendText("[clientThumbnail]" + e.UserState + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete..." + Environment.NewLine);
                tsProgress2.Value = e.ProgressPercentage;
            }
            catch (Exception ex)
            {
                txtLog.Text += "[clientThumbnail]"+ e.UserState + " clientThumb_DownloadProgressChanged(): " + ex.Message;
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
                txtLog.Text += "[clientThumbnail]" + e.UserState + " clientThumb_DownloadDataCompleted(): " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + Environment.NewLine;
                int i = (int)e.UserState;
                //
                if (chkLoadPreview.Checked)
                {
                    if (currThumbRetry < _retry)
                    {
                        currThumbRetry++;
                        //Thread.Sleep(1000);
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
            if (e.UserState != null)
            {
                DataGridViewRow row = (DataGridViewRow)e.UserState;
                txtLog.AppendText("[clientFileDownload]" + row.Index + "    downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes. " + e.ProgressPercentage + " % complete..." + Environment.NewLine);
                row.Cells["colProgress2"].Value = "Downloading: " + e.BytesReceived + " of " + e.TotalBytesToReceive;
            }
            tsProgressBar.Value = e.ProgressPercentage;
        }

        void clientFile_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!CheckDownloadGrid()) return;

            txtLog.AppendText("[clientFileDownload]" + e.UserState + "    Completed, Status: " + e.Error + Environment.NewLine);
            txtLog.AppendText("ref: " + _clientFile.Referer);
            DataGridViewRow row = (DataGridViewRow)e.UserState;
            row.Cells["colProgress2"].Value += Environment.NewLine + "Complete, Status: " + (e.Error == null ? "OK" : e.Error.Message);

            if (e.Error == null)
            {
                _downloadList[row.Index].Completed = true;
                string filename = row.Cells["colFilename"].Value.ToString();
                try
                {
                    if (!String.IsNullOrWhiteSpace(filename)) File.Move(filename + ".!tmp", filename);
                }
                catch (IOException ex)
                {
                    txtLog.AppendText("[clientFileDownload] Cannot rename completed file: " + ex.Message);
                }
            }
            if (row.Index < dgvDownload.Rows.GetLastRow(DataGridViewElementStates.None)) DownloadRows(dgvDownload.Rows[row.Index + 1]);
            else
            {
                _isPaused = false;
                _isDownloading = false;
                MessageBox.Show("Complete!", "Download Files");
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
