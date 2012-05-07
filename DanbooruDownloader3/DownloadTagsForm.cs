using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.Utils;
using System.IO;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;

namespace DanbooruDownloader3
{
    public partial class DownloadTagsForm : Form
    {
        private const string TAGS_FILENAME = "tags.xml";
        private ExtendedWebClient client;

        public DownloadTagsForm()
        {
            InitializeComponent();
            client = new ExtendedWebClient();
            client.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            btnDownload.Enabled = true;
            if (e.Error != null)
            {
                var message = e.Error.Message;
                if (File.Exists(TAGS_FILENAME + ".bak"))
                {
                    message += "," + Environment.NewLine + "Restoring backup.";
                    File.Move(TAGS_FILENAME + ".bak", TAGS_FILENAME);
                }
                MessageBox.Show(message);
            }
            else
            {                
                if (File.Exists(TAGS_FILENAME))
                {
                    File.Delete(TAGS_FILENAME);
                }
                File.Move(TAGS_FILENAME + ".!tmp", TAGS_FILENAME);

                // merge operation
                if (chkMerge.Checked)
                {
                    lblStatus.Text = "Status: Merging...";
                    lblStatus.Invalidate();
                    lblStatus.Update();
                    lblStatus.Refresh();
                    Application.DoEvents();
                    var message = DanbooruTagsDao.Merge(TAGS_FILENAME + ".merge", TAGS_FILENAME);
                    MessageBox.Show(message, "Merge tags.xml");
                    File.Delete(TAGS_FILENAME + ".merge");
                }

                DanbooruTagsDao.Instance = new DanbooruTagsDao(TAGS_FILENAME);
                lblStatus.Text = "Status: Download complete.";
                if (chkAutoClose.Checked)
                {
                    this.Close();
                }
            }
        }

        void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive != -1)
            {
                lblStatus.Text = "Status: Downloading " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes";
                progressBar1.Value = e.ProgressPercentage < 100 ? e.ProgressPercentage : 100;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }
            else
            {
                lblStatus.Text = "Status: Downloading " + e.BytesReceived + " bytes";
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtUrl.Text))
            {
                btnDownload.Enabled = false;
                lblStatus.Text = "Status: Download starting...";
                lblStatus.Invalidate();
                lblStatus.Update();
                lblStatus.Refresh();
                Application.DoEvents();
                
                // Delete temp file
                if (File.Exists(TAGS_FILENAME + ".!tmp")) File.Delete(TAGS_FILENAME + ".!tmp");

                // Merge preparation
                if (chkMerge.Checked)
                {
                    if (File.Exists(TAGS_FILENAME))
                    {
                        if (File.Exists(TAGS_FILENAME + ".merge")) File.Delete(TAGS_FILENAME + ".merge");
                        File.Copy(TAGS_FILENAME, TAGS_FILENAME + ".merge");
                    }
                    else
                    {
                        chkMerge.Checked = false;
                    }
                }

                // Backup
                if (chkBackup.Checked)
                {
                    if (File.Exists(TAGS_FILENAME))
                    {
                        if (File.Exists(TAGS_FILENAME + ".bak")) File.Delete(TAGS_FILENAME + ".bak");
                        File.Move(TAGS_FILENAME, TAGS_FILENAME + ".bak");
                    }
                }

                client.DownloadFileAsync(new Uri(txtUrl.Text), TAGS_FILENAME + ".!tmp");
                
            }
        }

        private void DownloadTagsForm_Load(object sender, EventArgs e)
        {
            DanbooruProviderDao provider = new DanbooruProviderDao();
            List<DanbooruProvider> list = provider.Read().Where<DanbooruProvider>(x => x.BoardType == Entity.BoardType.Danbooru).ToList<DanbooruProvider>();
            cbxProvider.DataSource = list;
            cbxProvider.DisplayMember = "Name";
            cbxProvider.ValueMember = "Url";
            cbxProvider.SelectedIndex = -1;
            txtUrl.Text = "";
        }

        private void cbxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProvider.SelectedIndex > -1)
            {
                txtUrl.Text = cbxProvider.SelectedValue + @"/tag/index.xml?limit=0";
            }
        }
    }
}
