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
        private const string filename = "tags.xml";
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
            if (File.Exists(filename))
            {
                File.Delete(filename);
                
            }
            File.Move(filename + ".!tmp", filename);
            DanbooruTagsDao.Instance = new DanbooruTagsDao(filename);
            lblStatus.Text = "Status: Download complete.";
            if (chkAutoClose.Checked)
            {
                this.Close();
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
                client.DownloadFileAsync(new Uri(txtUrl.Text), filename + ".!tmp");
                btnDownload.Enabled = false;
                lblStatus.Text = "Status: Download starting...";
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
