using DanbooruDownloader3.CustomControl;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using DanbooruDownloader3.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DanbooruDownloader3
{
    public partial class FormDownloadTags : Form
    {
        private const string TAGS_FILENAME = "tags.xml";
        private ExtendedWebClient client;
        private List<DanbooruProvider> list;

        private bool isClosing = false;
        private bool hasError = false;

        private int _page = 1;

        private int Page
        {
            get
            {
                Int32.TryParse(txtStartingPage.Text, out _page);
                return _page;
            }
            set
            {
                this._page = value;
                txtStartingPage.Text = _page.ToString();
            }
        }

        private DanbooruProvider SelectedProvider;

        private bool isSankaku;
        private int retry;

        public FormDownloadTags()
        {
            InitializeComponent();
            client = new ExtendedWebClient();
            client.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
        }

        #region webclient async event

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            btnDownload.Enabled = true;
            if (e.Error != null)
            {
                if (retry > Int32.Parse(DanbooruDownloader3.Properties.Settings.Default.retry))
                {
                    hasError = true;
                    var message = e.Error.InnerException == null ? e.Error.Message : e.Error.InnerException.Message;
                    if (File.Exists(TAGS_FILENAME + ".bak"))
                    {
                        message += "," + Environment.NewLine + "Restoring backup.";
                    }
                    RestoreBackup();
                    Program.Logger.Error("[Download Tags] " + message, e.Error);
                    MessageBox.Show(message);
                }
                else
                {
                    Program.Logger.Error(string.Format("Failed when downloading tags information from {0}", cbxProvider.SelectedValue), e.Error);
                    ++retry;
                    WaitForDelay(e.Error.Message);
                    if (chkUseLoop.Checked)
                    {
                        ProcessLoop(Page);
                    }
                    else
                    {
                        ProcessSingle();
                    }
                }
            }
            else
            {
                retry = 0;
                if (chkUseLoop.Checked)
                {
                    int delay = 0;
                    Int32.TryParse(Properties.Settings.Default.BatchJobDelay, out delay);
                    delay = delay / 1000;
                    if (delay > 0)
                    {
                        WaitForDelay("Tags Delay", delay);
                    }

                    string tempName = TAGS_FILENAME + "." + Page + ".!tmp";
                    ++Page;
                    HandleLoop(tempName);
                }
                else
                {
                    HandleSingle();
                }
            }
        }

        private void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            lblStatus.Text = "Status: Downloading";
            if (chkUseLoop.Checked)
            {
                lblStatus.Text += " Page " + Page;
            }
            if (e.TotalBytesToReceive != -1)
            {
                lblStatus.Text += " " + Helper.FormatByteSize(e.BytesReceived) + " of " + Helper.FormatByteSize(e.TotalBytesToReceive);
                progressBar1.Value = e.ProgressPercentage < 100 ? e.ProgressPercentage : 100;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }
            else
            {
                lblStatus.Text += " " + Helper.FormatByteSize(e.BytesReceived);
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
        }

        #endregion webclient async event

        #region event handlers

        private void FormDownloadTags_Load(object sender, EventArgs e)
        {
            list = DanbooruProviderDao.GetInstance().Read().Where<DanbooruProvider>(x => x.BoardType == Entity.BoardType.Danbooru || x.BoardType == Entity.BoardType.Gelbooru).ToList<DanbooruProvider>();
            cbxProvider.DataSource = list;
            cbxProvider.DisplayMember = "Name";
            cbxProvider.ValueMember = "Url";
            cbxProvider.SelectedIndex = -1;
            txtUrl.Text = "";
            isClosing = false;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            retry = 0;
            hasError = false;
            if (!String.IsNullOrWhiteSpace(txtUrl.Text))
            {
                SelectedProvider = list[cbxProvider.SelectedIndex];
                btnDownload.Enabled = false;
                lblStatus.Text = "Status: Download starting...";
                lblStatus.Invalidate();
                lblStatus.Update();
                lblStatus.Refresh();
                Application.DoEvents();

                CreateBackup();

                // Merge preparation
                if (chkMerge.Checked)
                {
                    Program.Logger.Info("[Download Tags] Merge checked.");
                    var mergeTarget = TAGS_FILENAME + ".merge";
                    if (File.Exists(TAGS_FILENAME))
                    {
                        if (File.Exists(mergeTarget))
                            File.Delete(mergeTarget);
                        File.Copy(TAGS_FILENAME, mergeTarget);
                    }
                    else
                    {
                        chkMerge.Checked = false;
                    }
                }

                if (chkUseLoop.Checked)
                {
                    chkUseLoop.Enabled = false;
                    ProcessLoop(Page);
                }
                else
                {
                    ProcessSingle();
                }
            }
        }

        private void cbxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStartingPage.Text = "1";
            UpdateQueryString();
        }

        private void chkUseLoop_CheckedChanged(object sender, EventArgs e)
        {
            UpdateQueryString();
        }

        #endregion event handlers

        private void WaitForDelay(string message, int sleepTime = -1)
        {
            if (sleepTime == -1)
            {
                sleepTime = Int32.Parse(DanbooruDownloader3.Properties.Settings.Default.delay);
            }

            lblStatus.Text = string.Format("Status: Waiting for {0}s", sleepTime);
            for (int i = 0; i < sleepTime; ++i)
            {
                for (int j = 1; j <= 10; ++j)
                {
                    if (isClosing)
                        return;

                    lblStatus.Text = string.Format("Status:{0}. Waiting for {1} of {2}s", message, i, sleepTime);
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
            }
        }

        private void HandleSingle()
        {
            if (File.Exists(TAGS_FILENAME))
            {
                File.Delete(TAGS_FILENAME);
            }
            File.Move(TAGS_FILENAME + ".!tmp", TAGS_FILENAME);

            if (SelectedProvider != null)
            {
                string targetXml = "tags-" + SelectedProvider.Name + ".xml";
                if (File.Exists(targetXml))
                {
                    if (chkBackup.Checked)
                    {
                        string backupName = targetXml + ".bak";
                        if (File.Exists(backupName))
                            File.Delete(backupName);
                        File.Move(targetXml, backupName);
                    }
                    else
                    {
                        File.Delete(targetXml);
                    }
                }
                File.Copy(TAGS_FILENAME, targetXml);
                SelectedProvider.LoadProviderTagCollection();
                Program.Logger.Info(String.Format("[Download Tags] Private Tags.xml saved to {0}.", targetXml));
            }

            // merge operation
            if (chkMerge.Checked)
            {
                Program.Logger.Debug("[Download Tags] Merging...");
                lblStatus.Text = "Status: Merging..., this might take some times.";
                lblStatus.Invalidate();
                lblStatus.Update();
                lblStatus.Refresh();
                Application.DoEvents();

                var message = DanbooruTagsDao.Merge(TAGS_FILENAME + ".merge", TAGS_FILENAME);
                Program.Logger.Info("[Download Tags] " + message);
                MessageBox.Show(message, "Tags.xml merged.");
                File.Delete(TAGS_FILENAME + ".merge");
            }

            DanbooruTagsDao.Instance = new DanbooruTagsDao(TAGS_FILENAME);
            Program.Logger.Info("[Download Tags] Complete.");
            lblStatus.Text = "Status: Download complete.";
            if (chkAutoClose.Checked)
            {
                this.Close();
            }
        }

        private DanbooruTagCollection prevTag = null;

        private void HandleLoop(string tempName)
        {
            DanbooruTagCollection tempTags = null;
            if (isSankaku)
            {
                string data = File.ReadAllText(tempName);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    tempTags = new SankakuComplexParser().parseTagsPage(data, Page);
                }
                else
                {
                    string message = "Got empty response!";
                    Program.Logger.Error(message);
                    ++retry;
                    if (retry > Int32.Parse(DanbooruDownloader3.Properties.Settings.Default.retry))
                    {
                        WaitForDelay(message);
                        ProcessLoop(--Page);
                    }
                    else
                    {
                        MessageBox.Show(message);
                    }
                }
            }
            else
            {
                tempTags = new DanbooruTagsDao(tempName).Tags;
            }

            if (tempTags == null || tempTags.Tag == null || tempTags.Tag.Length == 0 ||
               (prevTag != null && prevTag.Tag.Last().Name == tempTags.Tag.Last().Name))
            {
                // no more tags
                chkUseLoop.Enabled = true;

                var newTagList = CombineLoopTag(Page);

                if (SelectedProvider != null)
                {
                    string targetXml = "tags-" + SelectedProvider.Name + ".xml";
                    if (File.Exists(targetXml))
                    {
                        if (chkBackup.Checked)
                        {
                            string backupName = targetXml + ".bak";
                            if (File.Exists(backupName))
                                File.Delete(backupName);
                            File.Move(targetXml, backupName);
                        }
                        else
                        {
                            File.Delete(targetXml);
                        }
                    }
                    DanbooruTagsDao.Save(targetXml, newTagList);
                    SelectedProvider.LoadProviderTagCollection();
                    Program.Logger.Info(String.Format("[Download Tags] Private Tags.xml saved to {0}.", targetXml));
                }

                if (chkMerge.Checked)
                {
                    Program.Logger.Debug("[Download Tags] Merging Old Tags.");
                    lblStatus.Text = "Status: Merging Old Tags, this might take some times.";
                    lblStatus.Invalidate();
                    lblStatus.Update();
                    lblStatus.Refresh();
                    Application.DoEvents();

                    DanbooruTagsDao.Save(TAGS_FILENAME + ".merge", newTagList);
                    var message = DanbooruTagsDao.Merge(TAGS_FILENAME + ".merge", TAGS_FILENAME);

                    Program.Logger.Info("[Download Tags] " + message);
                    MessageBox.Show(message, "Tags.xml merged.");

                    File.Delete(TAGS_FILENAME + ".merge");
                }
                else
                {
                    // write back to TAGS_FILENAME
                    DanbooruTagsDao.Save(TAGS_FILENAME, newTagList);
                }

                DanbooruTagsDao.Instance = new DanbooruTagsDao(TAGS_FILENAME);
                Program.Logger.Info("[Download Tags] Complete.");
                lblStatus.Text = "Status: Download complete.";
                if (chkAutoClose.Checked)
                {
                    this.Close();
                }
            }
            else
            {
                // continue next page
                ProcessLoop(Page);
                prevTag = tempTags;
            }
        }

        private List<DanbooruTag> CombineLoopTag(int lastPage)
        {
            // construct the tag list from each xml
            List<DanbooruTag> newTagList = new List<DanbooruTag>();
            for (int i = 1; i < lastPage; ++i)
            {
                Program.Logger.Debug("[Download Tags] Merging Page: " + i);
                lblStatus.Text = "Status: Merging Page: " + i + ", this might take some times.";
                lblStatus.Invalidate();
                lblStatus.Update();
                lblStatus.Refresh();
                Application.DoEvents();

                string tempName = TAGS_FILENAME + "." + i + ".!tmp";

                DanbooruTagCollection tempTag;
                if (isSankaku)
                {
                    SankakuComplexParser parser = new SankakuComplexParser();
                    tempTag = parser.parseTagsPage(File.ReadAllText(tempName), i);
                }
                else
                {
                    tempTag = new DanbooruTagsDao(tempName).Tags;
                }
                if (tempTag != null && tempTag.Tag != null)
                {
                    newTagList.AddRange(tempTag.Tag);
                }
            }

            for (int i = 1; i < lastPage; ++i)
            {
                string tempName = TAGS_FILENAME + "." + i + ".!tmp";
                File.Delete(tempName);
            }

            return newTagList;
        }

        private void ProcessSingle()
        {
            // Delete temp file
            if (File.Exists(TAGS_FILENAME + ".!tmp")) File.Delete(TAGS_FILENAME + ".!tmp");

            Program.Logger.Info("[Download Tags] Start downloading...");
            client.DownloadFileAsync(new Uri(txtUrl.Text), TAGS_FILENAME + ".!tmp");
        }

        private void ProcessLoop(int page)
        {
            if (isClosing)
            {
                return;
            }
            // Delete temp file
            string tempFile = TAGS_FILENAME + "." + page + ".!tmp";
            if (File.Exists(tempFile)) File.Delete(tempFile);

            Program.Logger.Info("[Download Tags] Start downloading page: " + page);
            string url = txtUrl.Text;
            if (SelectedProvider.BoardType == BoardType.Gelbooru)
            {
                url = txtUrl.Text + "&order=name&pid=" + page;
            }
            else
            {
                url = txtUrl.Text + "&order=name&page=" + page;
            }
            Program.Logger.Info("[Download Tags] Start downloading url: " + url);
            client.DownloadFileAsync(new Uri(url), tempFile);
        }

        private void UpdateQueryString()
        {
            var index = cbxProvider.SelectedIndex;
            if (index > -1)
            {
                SelectedProvider = list[index];
                string limit = "0";
                chkUseLoop.Checked = SelectedProvider.TagDownloadUseLoop;
                txtStartingPage.Enabled = chkUseLoop.Checked;
                if (chkUseLoop.Checked)
                {
                    limit = "1000";
                }

                Page = Int32.Parse(txtStartingPage.Text);
                if (SelectedProvider.BoardType == BoardType.Danbooru)
                {
                    txtUrl.Text = cbxProvider.SelectedValue + @"/tag/index.xml?limit=" + limit;

                    // sankaku
                    if (cbxProvider.SelectedValue.ToString().ToLowerInvariant().Contains("sankaku"))
                    {
                        txtUrl.Text = cbxProvider.SelectedValue + @"/tag/index?";
                        isSankaku = true;
                    }
                    else
                    {
                        isSankaku = false;
                    }

                    pbIcon.Image = Properties.Resources.Danbooru;
                }
                else if (SelectedProvider.BoardType == BoardType.Gelbooru)
                {
                    txtUrl.Text = cbxProvider.SelectedValue + @"/index.php?page=dapi&s=tag&q=index&limit=" + limit;
                    pbIcon.Image = Properties.Resources.Gelbooru;
                }

                if (SelectedProvider.LoginType == LoginType.Cookie || SelectedProvider.LoginType == LoginType.CookieAlwaysAsk)
                {
                    if (String.IsNullOrWhiteSpace(SelectedProvider.UserName))
                        Program.Logger.Info("[UpdateQueryString] Missing cookie value for : " + SelectedProvider.Name);
                    else
                        Program.Logger.Info("[UpdateQueryString] Using cookie value: " + SelectedProvider.UserName);
                    // need to inject csv cookie  to the webclient
                    var cookies = Helper.ParseCookie(SelectedProvider.UserName, SelectedProvider.Url);
                    foreach (var cookie in cookies)
                    {
                        ExtendedWebClient.CookieJar.Add(cookie);
                    }
                }
            }
            else
            {
                chkUseLoop.Checked = false;
                txtUrl.Text = "";
                pbIcon.Image = null;
            }
        }

        private void FormDownloadTags_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            RestoreBackup();
        }

        private void CreateBackup()
        {
            if (chkBackup.Checked)
            {
                Program.Logger.Info("[Download Tags] Backup checked.");
                // Main tags.xml
                var backup = TAGS_FILENAME + ".bak";
                if (File.Exists(TAGS_FILENAME))
                {
                    if (File.Exists(backup))
                        File.Delete(backup);

                    File.Move(TAGS_FILENAME, backup);
                }

                // selected provider
                string targetXml = "tags-" + SelectedProvider.Name + ".xml";
                string targetXmlBackup = "tags-" + SelectedProvider.Name + ".xml.bak";
                if (File.Exists(targetXml))
                {
                    if (File.Exists(targetXmlBackup))
                        File.Delete(targetXmlBackup);

                    File.Move(targetXml, targetXmlBackup);
                }
            }
        }

        private void RestoreBackup()
        {
            if (chkBackup.Checked && hasError)
            {
                // main tags.xml
                var backup = TAGS_FILENAME + ".bak";
                if (File.Exists(backup))
                {
                    Program.Logger.Info("[Download Tags] Restoring backup: " + TAGS_FILENAME);
                    File.Move(backup, TAGS_FILENAME);
                }

                // selected provider
                string targetXml = "tags-" + SelectedProvider.Name + ".xml";
                string targetXmlBackup = "tags-" + SelectedProvider.Name + ".xml.bak";
                if (File.Exists(targetXmlBackup))
                {
                    Program.Logger.Info("[Download Tags] Restoring backup: " + targetXml);
                    File.Move(targetXmlBackup, targetXml);
                }
            }
        }
    }
}