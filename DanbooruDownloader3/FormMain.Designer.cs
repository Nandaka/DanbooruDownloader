namespace DanbooruDownloader3
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (_pauseEvent != null) _pauseEvent.Dispose();
            if (_shutdownEvent!= null) _shutdownEvent.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbxAutoComplete = new System.Windows.Forms.ListBox();
            this.gbxSearch = new System.Windows.Forms.GroupBox();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSearchHelp = new System.Windows.Forms.Button();
            this.btnListCancel = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnGet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkNotRating = new System.Windows.Forms.CheckBox();
            this.cbxRating = new System.Windows.Forms.ComboBox();
            this.cbxProvider = new System.Windows.Forms.ComboBox();
            this.cbxOrder = new System.Windows.Forms.ComboBox();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkGenerate = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.gbxDanbooru = new System.Windows.Forms.GroupBox();
            this.chkAutoLoadNext = new System.Windows.Forms.CheckBox();
            this.chkAppendList = new System.Windows.Forms.CheckBox();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.chkSaveQuery = new System.Windows.Forms.CheckBox();
            this.chkAutoLoadList = new System.Windows.Forms.CheckBox();
            this.chkLoadPreview = new System.Windows.Forms.CheckBox();
            this.gbxList = new System.Windows.Forms.GroupBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAddDownload = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.btnBrowseListFile = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtListFile = new System.Windows.Forms.TextBox();
            this.dgvList = new DanbooruDownloader3.CustomControl.gfDataGridView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPreview = new System.Windows.Forms.DataGridViewImageColumn();
            this.colProvider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTagsE = new DanbooruDownloader3.CustomControl.TagsColumn();
            this.colUrl = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colMD5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceUrl = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colReferer = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxMenuList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.searchByParentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadThumbnailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.resetColumnOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnClearCompletedDownload = new System.Windows.Forms.Button();
            this.btnBrowseFolder = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btnLoadDownloadList = new System.Windows.Forms.Button();
            this.btnSaveDownloadList = new System.Windows.Forms.Button();
            this.btnCancelDownload = new System.Windows.Forms.Button();
            this.btnClearDownloadList = new System.Windows.Forms.Button();
            this.btnPauseDownload = new System.Windows.Forms.Button();
            this.btnDownloadFiles = new System.Windows.Forms.Button();
            this.txtSaveFolder = new System.Windows.Forms.TextBox();
            this.dgvDownload = new DanbooruDownloader3.CustomControl.gfDataGridView();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPreview2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.colProgress2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProvider2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRating2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTags2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUrl2 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colReferer2 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colMD52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuery2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDownloadStart2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxMenuDownload = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resolveFileUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnLoadList = new System.Windows.Forms.Button();
            this.btnSaveBatchList = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClearCompleted = new System.Windows.Forms.Button();
            this.btnPauseBatchJob = new System.Windows.Forms.Button();
            this.btnStopBatchJob = new System.Windows.Forms.Button();
            this.btnStartBatchJob = new System.Windows.Forms.Button();
            this.btnAddBatchJob = new System.Windows.Forms.Button();
            this.cbxAbortOnError = new System.Windows.Forms.CheckBox();
            this.dgvBatchJob = new DanbooruDownloader3.CustomControl.gfDataGridView();
            this.colBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchTagQuery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchStartPage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchProviders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchSaveFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxMenuBatch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkProcessDeletedPost = new System.Windows.Forms.CheckBox();
            this.chkHideBlaclistedImage = new System.Windows.Forms.CheckBox();
            this.chkUseGlobalProviderTags = new System.Windows.Forms.CheckBox();
            this.chkMinimizeTray = new System.Windows.Forms.CheckBox();
            this.chkAutoFocus = new System.Windows.Forms.CheckBox();
            this.chkLogging = new System.Windows.Forms.CheckBox();
            this.chkUseTagColor = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtNoFault = new System.Windows.Forms.TextBox();
            this.txtNoCircle = new System.Windows.Forms.TextBox();
            this.txtNoChara = new System.Windows.Forms.TextBox();
            this.txtNoCopyright = new System.Windows.Forms.TextBox();
            this.txtNoArtist = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.chkBlacklistOnlyGeneral = new System.Windows.Forms.CheckBox();
            this.chkIgnoreForGeneralTag = new System.Windows.Forms.CheckBox();
            this.chkReplaceMode = new System.Windows.Forms.CheckBox();
            this.lblColorUnknown = new System.Windows.Forms.Label();
            this.chkBlacklistTagsUseRegex = new System.Windows.Forms.CheckBox();
            this.chkIgnoreTagsUseRegex = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtIgnoredTags = new System.Windows.Forms.TextBox();
            this.txtAutoCompleteLimit = new System.Windows.Forms.TextBox();
            this.txtCircleTagGrouping = new System.Windows.Forms.TextBox();
            this.txtFaultsTagGrouping = new System.Windows.Forms.TextBox();
            this.txtCharaTagGrouping = new System.Windows.Forms.TextBox();
            this.txtTagReplacement = new System.Windows.Forms.TextBox();
            this.txtCopyTagGrouping = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.lblColorBlacklistedTag = new System.Windows.Forms.Label();
            this.chkTagAutoComplete = new System.Windows.Forms.CheckBox();
            this.lblColorFaults = new System.Windows.Forms.Label();
            this.txtArtistTagGrouping = new System.Windows.Forms.TextBox();
            this.lblColorCircle = new System.Windows.Forms.Label();
            this.lblColorChara = new System.Windows.Forms.Label();
            this.lblColorCopy = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.txtTagBlacklist = new System.Windows.Forms.TextBox();
            this.lblColorArtist = new System.Windows.Forms.Label();
            this.lblColorGeneral = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkWriteDownloadedFileTxt = new System.Windows.Forms.CheckBox();
            this.chkDelayIncludeSkip = new System.Windows.Forms.CheckBox();
            this.txtBatchJobDelay = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.chkSaveFolderWhenExit = new System.Windows.Forms.CheckBox();
            this.btnBrowseDefaultSave = new System.Windows.Forms.Button();
            this.txtDefaultSaveFolder = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cbxImageSize = new System.Windows.Forms.ComboBox();
            this.chkRenameJpeg = new System.Windows.Forms.CheckBox();
            this.txtFilenameLength = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.txtFilenameFormat = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEnableCompression = new System.Windows.Forms.CheckBox();
            this.txtAcceptLanguage = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.chkEnableCookie = new System.Windows.Forms.CheckBox();
            this.btnCookie = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.linkUrl = new System.Windows.Forms.LinkLabel();
            this.chkProxyLogin = new System.Windows.Forms.CheckBox();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtProxyUsername = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.chkPadUserAgent = new System.Windows.Forms.CheckBox();
            this.txtRetry = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtUserAgent = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkUseProxy = new System.Windows.Forms.CheckBox();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.txtProxyAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFilenameHelp = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.ctxMenuLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timGifAnimation = new System.Windows.Forms.Timer(this.components);
            this.timLoadNextPage = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTotalCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsProgress2 = new System.Windows.Forms.ToolStripProgressBar();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxMenuSysTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsColumn1 = new DanbooruDownloader3.CustomControl.TagsColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsColumn2 = new DanbooruDownloader3.CustomControl.TagsColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbxSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.gbxDanbooru.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.gbxList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.ctxMenuList.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDownload)).BeginInit();
            this.ctxMenuDownload.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchJob)).BeginInit();
            this.ctxMenuBatch.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.ctxMenuLog.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ctxMenuSysTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1267, 766);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbxAutoComplete);
            this.tabPage1.Controls.Add(this.gbxSearch);
            this.tabPage1.Controls.Add(this.gbxDanbooru);
            this.tabPage1.Controls.Add(this.gbxList);
            this.tabPage1.Controls.Add(this.dgvList);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1259, 737);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbxAutoComplete
            // 
            this.lbxAutoComplete.FormattingEnabled = true;
            this.lbxAutoComplete.ItemHeight = 16;
            this.lbxAutoComplete.Location = new System.Drawing.Point(67, 121);
            this.lbxAutoComplete.Margin = new System.Windows.Forms.Padding(4);
            this.lbxAutoComplete.Name = "lbxAutoComplete";
            this.lbxAutoComplete.Size = new System.Drawing.Size(316, 132);
            this.lbxAutoComplete.TabIndex = 10;
            this.lbxAutoComplete.Visible = false;
            this.lbxAutoComplete.DataSourceChanged += new System.EventHandler(this.lbxAutoComplete_DataSourceChanged);
            this.lbxAutoComplete.DoubleClick += new System.EventHandler(this.lbxAutoComplete_DoubleClick);
            this.lbxAutoComplete.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbxAutoComplete_KeyDown);
            this.lbxAutoComplete.Leave += new System.EventHandler(this.lbxAutoComplete_Leave);
            // 
            // gbxSearch
            // 
            this.gbxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxSearch.Controls.Add(this.btnPrevPage);
            this.gbxSearch.Controls.Add(this.btnNextPage);
            this.gbxSearch.Controls.Add(this.pbIcon);
            this.gbxSearch.Controls.Add(this.btnEdit);
            this.gbxSearch.Controls.Add(this.btnSearchHelp);
            this.gbxSearch.Controls.Add(this.btnListCancel);
            this.gbxSearch.Controls.Add(this.linkLabel1);
            this.gbxSearch.Controls.Add(this.btnGet);
            this.gbxSearch.Controls.Add(this.label1);
            this.gbxSearch.Controls.Add(this.chkNotRating);
            this.gbxSearch.Controls.Add(this.cbxRating);
            this.gbxSearch.Controls.Add(this.cbxProvider);
            this.gbxSearch.Controls.Add(this.cbxOrder);
            this.gbxSearch.Controls.Add(this.txtSource);
            this.gbxSearch.Controls.Add(this.label8);
            this.gbxSearch.Controls.Add(this.label7);
            this.gbxSearch.Controls.Add(this.label6);
            this.gbxSearch.Controls.Add(this.txtPage);
            this.gbxSearch.Controls.Add(this.label5);
            this.gbxSearch.Controls.Add(this.txtLimit);
            this.gbxSearch.Controls.Add(this.label4);
            this.gbxSearch.Controls.Add(this.txtTags);
            this.gbxSearch.Controls.Add(this.label3);
            this.gbxSearch.Controls.Add(this.chkGenerate);
            this.gbxSearch.Controls.Add(this.label2);
            this.gbxSearch.Controls.Add(this.txtQuery);
            this.gbxSearch.Location = new System.Drawing.Point(11, 7);
            this.gbxSearch.Margin = new System.Windows.Forms.Padding(4);
            this.gbxSearch.Name = "gbxSearch";
            this.gbxSearch.Padding = new System.Windows.Forms.Padding(4);
            this.gbxSearch.Size = new System.Drawing.Size(864, 194);
            this.gbxSearch.TabIndex = 0;
            this.gbxSearch.TabStop = false;
            this.gbxSearch.Text = "1. Search";
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(657, 150);
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(27, 28);
            this.btnPrevPage.TabIndex = 20;
            this.btnPrevPage.Text = "<";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextPage.Location = new System.Drawing.Point(832, 150);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(4);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(27, 28);
            this.btnNextPage.TabIndex = 11;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbIcon.Location = new System.Drawing.Point(623, 26);
            this.pbIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(21, 20);
            this.pbIcon.TabIndex = 19;
            this.pbIcon.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(756, 21);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 28);
            this.btnEdit.TabIndex = 13;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSearchHelp
            // 
            this.btnSearchHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchHelp.Location = new System.Drawing.Point(756, 86);
            this.btnSearchHelp.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearchHelp.Name = "btnSearchHelp";
            this.btnSearchHelp.Size = new System.Drawing.Size(100, 28);
            this.btnSearchHelp.TabIndex = 14;
            this.btnSearchHelp.Text = "Search Help";
            this.btnSearchHelp.UseVisualStyleBackColor = true;
            this.btnSearchHelp.Click += new System.EventHandler(this.btnSearchHelp_Click);
            // 
            // btnListCancel
            // 
            this.btnListCancel.Enabled = false;
            this.btnListCancel.Location = new System.Drawing.Point(579, 150);
            this.btnListCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnListCancel.Name = "btnListCancel";
            this.btnListCancel.Size = new System.Drawing.Size(71, 28);
            this.btnListCancel.TabIndex = 15;
            this.btnListCancel.Text = "Cancel";
            this.btnListCancel.UseVisualStyleBackColor = true;
            this.btnListCancel.Click += new System.EventHandler(this.btnListCancel_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(710, 54);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(147, 28);
            this.linkLabel1.TabIndex = 18;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "                                          ";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btnGet
            // 
            this.btnGet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGet.Location = new System.Drawing.Point(692, 150);
            this.btnGet.Margin = new System.Windows.Forms.Padding(4);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(132, 28);
            this.btnGet.TabIndex = 16;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Provider";
            // 
            // chkNotRating
            // 
            this.chkNotRating.AutoSize = true;
            this.chkNotRating.Location = new System.Drawing.Point(511, 155);
            this.chkNotRating.Margin = new System.Windows.Forms.Padding(4);
            this.chkNotRating.Name = "chkNotRating";
            this.chkNotRating.Size = new System.Drawing.Size(52, 21);
            this.chkNotRating.TabIndex = 9;
            this.chkNotRating.Text = "Not";
            this.chkNotRating.UseVisualStyleBackColor = true;
            this.chkNotRating.CheckedChanged += new System.EventHandler(this.chkRating_CheckedChanged);
            // 
            // cbxRating
            // 
            this.cbxRating.FormattingEnabled = true;
            this.cbxRating.Location = new System.Drawing.Point(375, 153);
            this.cbxRating.Margin = new System.Windows.Forms.Padding(4);
            this.cbxRating.Name = "cbxRating";
            this.cbxRating.Size = new System.Drawing.Size(127, 24);
            this.cbxRating.TabIndex = 8;
            this.cbxRating.SelectedIndexChanged += new System.EventHandler(this.cbxRating_SelectedIndexChanged);
            // 
            // cbxProvider
            // 
            this.cbxProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxProvider.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxProvider.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxProvider.FormattingEnabled = true;
            this.cbxProvider.Location = new System.Drawing.Point(77, 23);
            this.cbxProvider.Margin = new System.Windows.Forms.Padding(4);
            this.cbxProvider.Name = "cbxProvider";
            this.cbxProvider.Size = new System.Drawing.Size(536, 24);
            this.cbxProvider.TabIndex = 0;
            this.cbxProvider.SelectedIndexChanged += new System.EventHandler(this.cbxProvider_SelectedIndexChanged);
            // 
            // cbxOrder
            // 
            this.cbxOrder.FormattingEnabled = true;
            this.cbxOrder.Location = new System.Drawing.Point(180, 153);
            this.cbxOrder.Margin = new System.Windows.Forms.Padding(4);
            this.cbxOrder.Name = "cbxOrder";
            this.cbxOrder.Size = new System.Drawing.Size(127, 24);
            this.cbxOrder.TabIndex = 7;
            this.cbxOrder.SelectedIndexChanged += new System.EventHandler(this.cbxOrder_SelectedIndexChanged);
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Location = new System.Drawing.Point(180, 121);
            this.txtSource.Margin = new System.Windows.Forms.Padding(4);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(675, 22);
            this.txtSource.TabIndex = 5;
            this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 124);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 17);
            this.label8.TabIndex = 13;
            this.label8.Text = "Source";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(316, 156);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Rating";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(117, 156);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Order by";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(56, 153);
            this.txtPage.Margin = new System.Windows.Forms.Padding(4);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(52, 22);
            this.txtPage.TabIndex = 6;
            this.txtPage.TextChanged += new System.EventHandler(this.txtPage_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 156);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Page";
            // 
            // txtLimit
            // 
            this.txtLimit.Location = new System.Drawing.Point(56, 121);
            this.txtLimit.Margin = new System.Windows.Forms.Padding(4);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(52, 22);
            this.txtLimit.TabIndex = 4;
            this.txtLimit.TextChanged += new System.EventHandler(this.txtLimit_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 124);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Limit";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtTags.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtTags.Location = new System.Drawing.Point(56, 89);
            this.txtTags.Margin = new System.Windows.Forms.Padding(4);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(691, 22);
            this.txtTags.TabIndex = 3;
            this.txtTags.TextChanged += new System.EventHandler(this.txtTags_TextChanged);
            this.txtTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTags_KeyDown);
            this.txtTags.Leave += new System.EventHandler(this.txtTags_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 92);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tag";
            // 
            // chkGenerate
            // 
            this.chkGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkGenerate.AutoSize = true;
            this.chkGenerate.Checked = true;
            this.chkGenerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerate.Location = new System.Drawing.Point(660, 26);
            this.chkGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.chkGenerate.Name = "chkGenerate";
            this.chkGenerate.Size = new System.Drawing.Size(98, 21);
            this.chkGenerate.TabIndex = 12;
            this.chkGenerate.Text = "Generated";
            this.chkGenerate.UseVisualStyleBackColor = true;
            this.chkGenerate.CheckedChanged += new System.EventHandler(this.chkGenerate_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Query";
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Location = new System.Drawing.Point(77, 57);
            this.txtQuery.Margin = new System.Windows.Forms.Padding(4);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ReadOnly = true;
            this.txtQuery.Size = new System.Drawing.Size(623, 22);
            this.txtQuery.TabIndex = 2;
            this.txtQuery.TextChanged += new System.EventHandler(this.txtQuery_TextChanged);
            // 
            // gbxDanbooru
            // 
            this.gbxDanbooru.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDanbooru.Controls.Add(this.chkAutoLoadNext);
            this.gbxDanbooru.Controls.Add(this.chkAppendList);
            this.gbxDanbooru.Controls.Add(this.pbLoading);
            this.gbxDanbooru.Controls.Add(this.chkSaveQuery);
            this.gbxDanbooru.Controls.Add(this.chkAutoLoadList);
            this.gbxDanbooru.Controls.Add(this.chkLoadPreview);
            this.gbxDanbooru.Location = new System.Drawing.Point(883, 123);
            this.gbxDanbooru.Margin = new System.Windows.Forms.Padding(4);
            this.gbxDanbooru.Name = "gbxDanbooru";
            this.gbxDanbooru.Padding = new System.Windows.Forms.Padding(4);
            this.gbxDanbooru.Size = new System.Drawing.Size(363, 74);
            this.gbxDanbooru.TabIndex = 9;
            this.gbxDanbooru.TabStop = false;
            this.gbxDanbooru.Text = "Danbooru Listing";
            // 
            // chkAutoLoadNext
            // 
            this.chkAutoLoadNext.AutoSize = true;
            this.chkAutoLoadNext.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AutoLoadNextPage;
            this.chkAutoLoadNext.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoLoadNext.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AutoLoadNextPage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAutoLoadNext.Location = new System.Drawing.Point(125, 46);
            this.chkAutoLoadNext.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoLoadNext.Name = "chkAutoLoadNext";
            this.chkAutoLoadNext.Size = new System.Drawing.Size(194, 21);
            this.chkAutoLoadNext.TabIndex = 21;
            this.chkAutoLoadNext.Text = "Auto Load Next StartPage";
            this.chkAutoLoadNext.UseVisualStyleBackColor = true;
            // 
            // chkAppendList
            // 
            this.chkAppendList.AutoSize = true;
            this.chkAppendList.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AppendList;
            this.chkAppendList.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AppendList", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAppendList.Location = new System.Drawing.Point(245, 18);
            this.chkAppendList.Margin = new System.Windows.Forms.Padding(4);
            this.chkAppendList.Name = "chkAppendList";
            this.chkAppendList.Size = new System.Drawing.Size(105, 21);
            this.chkAppendList.TabIndex = 19;
            this.chkAppendList.Text = "Append List";
            this.chkAppendList.UseVisualStyleBackColor = true;
            // 
            // pbLoading
            // 
            this.pbLoading.InitialImage = null;
            this.pbLoading.Location = new System.Drawing.Point(1, 1);
            this.pbLoading.Margin = new System.Windows.Forms.Padding(4);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(1, 1);
            this.pbLoading.TabIndex = 11;
            this.pbLoading.TabStop = false;
            // 
            // chkSaveQuery
            // 
            this.chkSaveQuery.AutoSize = true;
            this.chkSaveQuery.Checked = global::DanbooruDownloader3.Properties.Settings.Default.SaveQuery;
            this.chkSaveQuery.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "SaveQuery", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkSaveQuery.Location = new System.Drawing.Point(8, 20);
            this.chkSaveQuery.Margin = new System.Windows.Forms.Padding(4);
            this.chkSaveQuery.Name = "chkSaveQuery";
            this.chkSaveQuery.Size = new System.Drawing.Size(105, 21);
            this.chkSaveQuery.TabIndex = 17;
            this.chkSaveQuery.Text = "Save Query";
            this.chkSaveQuery.UseVisualStyleBackColor = true;
            // 
            // chkAutoLoadList
            // 
            this.chkAutoLoadList.AutoSize = true;
            this.chkAutoLoadList.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AutoLoadList;
            this.chkAutoLoadList.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AutoLoadList", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAutoLoadList.Location = new System.Drawing.Point(125, 20);
            this.chkAutoLoadList.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoLoadList.Name = "chkAutoLoadList";
            this.chkAutoLoadList.Size = new System.Drawing.Size(121, 21);
            this.chkAutoLoadList.TabIndex = 18;
            this.chkAutoLoadList.Text = "Auto Load List";
            this.chkAutoLoadList.UseVisualStyleBackColor = true;
            // 
            // chkLoadPreview
            // 
            this.chkLoadPreview.AutoSize = true;
            this.chkLoadPreview.Checked = global::DanbooruDownloader3.Properties.Settings.Default.LoadPreview;
            this.chkLoadPreview.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "LoadPreview", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkLoadPreview.Location = new System.Drawing.Point(8, 46);
            this.chkLoadPreview.Margin = new System.Windows.Forms.Padding(4);
            this.chkLoadPreview.Name = "chkLoadPreview";
            this.chkLoadPreview.Size = new System.Drawing.Size(115, 21);
            this.chkLoadPreview.TabIndex = 20;
            this.chkLoadPreview.Text = "Load Preview";
            this.chkLoadPreview.UseVisualStyleBackColor = true;
            // 
            // gbxList
            // 
            this.gbxList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxList.Controls.Add(this.btnSelectAll);
            this.gbxList.Controls.Add(this.btnClear);
            this.gbxList.Controls.Add(this.btnAddDownload);
            this.gbxList.Controls.Add(this.btnList);
            this.gbxList.Controls.Add(this.btnBrowseListFile);
            this.gbxList.Controls.Add(this.label9);
            this.gbxList.Controls.Add(this.txtListFile);
            this.gbxList.Location = new System.Drawing.Point(883, 7);
            this.gbxList.Margin = new System.Windows.Forms.Padding(4);
            this.gbxList.Name = "gbxList";
            this.gbxList.Padding = new System.Windows.Forms.Padding(4);
            this.gbxList.Size = new System.Drawing.Size(363, 108);
            this.gbxList.TabIndex = 5;
            this.gbxList.TabStop = false;
            this.gbxList.Text = "2. Load List";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(164, 60);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(83, 28);
            this.btnSelectAll.TabIndex = 12;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(72, 60);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(84, 28);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear List";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddDownload
            // 
            this.btnAddDownload.Location = new System.Drawing.Point(255, 60);
            this.btnAddDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddDownload.Name = "btnAddDownload";
            this.btnAddDownload.Size = new System.Drawing.Size(100, 28);
            this.btnAddDownload.TabIndex = 10;
            this.btnAddDownload.Text = "Add";
            this.btnAddDownload.UseVisualStyleBackColor = true;
            this.btnAddDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(12, 60);
            this.btnList.Margin = new System.Windows.Forms.Padding(4);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(52, 28);
            this.btnList.TabIndex = 8;
            this.btnList.Text = "List";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // btnBrowseListFile
            // 
            this.btnBrowseListFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseListFile.Location = new System.Drawing.Point(275, 20);
            this.btnBrowseListFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseListFile.Name = "btnBrowseListFile";
            this.btnBrowseListFile.Size = new System.Drawing.Size(80, 28);
            this.btnBrowseListFile.TabIndex = 4;
            this.btnBrowseListFile.Text = "Browse";
            this.btnBrowseListFile.UseVisualStyleBackColor = true;
            this.btnBrowseListFile.Click += new System.EventHandler(this.btnBrowseListFile_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 27);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 17);
            this.label9.TabIndex = 3;
            this.label9.Text = "List File";
            // 
            // txtListFile
            // 
            this.txtListFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtListFile.Location = new System.Drawing.Point(72, 22);
            this.txtListFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtListFile.Name = "txtListFile";
            this.txtListFile.Size = new System.Drawing.Size(193, 22);
            this.txtListFile.TabIndex = 2;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colCheck,
            this.colPreview,
            this.colProvider,
            this.colId,
            this.colRating,
            this.colScore,
            this.colTags,
            this.colTagsE,
            this.colUrl,
            this.colMD5,
            this.colQuery,
            this.colSourceUrl,
            this.colReferer,
            this.colStatus});
            this.dgvList.ContextMenuStrip = this.ctxMenuList;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvList.Location = new System.Drawing.Point(11, 209);
            this.dgvList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvList.Name = "dgvList";
            this.dgvList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvList.RowHeadersVisible = false;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvList.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowTemplate.Height = 24;
            this.dgvList.Size = new System.Drawing.Size(1235, 517);
            this.dgvList.TabIndex = 6;
            this.dgvList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellContentClick);
            this.dgvList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvList_CellMouseDown);
            this.dgvList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            this.dgvList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvList_RowsAdded);
            // 
            // colNumber
            // 
            this.colNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colNumber.FillWeight = 35F;
            this.colNumber.Frozen = true;
            this.colNumber.HeaderText = "#";
            this.colNumber.MinimumWidth = 25;
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Width = 25;
            // 
            // colCheck
            // 
            this.colCheck.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCheck.FillWeight = 25F;
            this.colCheck.Frozen = true;
            this.colCheck.HeaderText = "*";
            this.colCheck.MinimumWidth = 25;
            this.colCheck.Name = "colCheck";
            this.colCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCheck.Width = 25;
            // 
            // colPreview
            // 
            this.colPreview.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPreview.DataPropertyName = "ThumbnailImage";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colPreview.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPreview.FillWeight = 500F;
            this.colPreview.Frozen = true;
            this.colPreview.HeaderText = "Preview";
            this.colPreview.Image = global::DanbooruDownloader3.Properties.Resources.NOT_AVAILABLE;
            this.colPreview.MinimumWidth = 150;
            this.colPreview.Name = "colPreview";
            this.colPreview.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPreview.Width = 150;
            // 
            // colProvider
            // 
            this.colProvider.DataPropertyName = "provider";
            this.colProvider.HeaderText = "Provider";
            this.colProvider.Name = "colProvider";
            this.colProvider.Width = 86;
            // 
            // colId
            // 
            this.colId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colId.DataPropertyName = "Id";
            this.colId.FillWeight = 75F;
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colId.Width = 44;
            // 
            // colRating
            // 
            this.colRating.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRating.DataPropertyName = "Rating";
            this.colRating.FillWeight = 25F;
            this.colRating.HeaderText = "Rating";
            this.colRating.MinimumWidth = 25;
            this.colRating.Name = "colRating";
            this.colRating.Width = 25;
            // 
            // colScore
            // 
            this.colScore.DataPropertyName = "Score";
            this.colScore.HeaderText = "Score";
            this.colScore.Name = "colScore";
            this.colScore.Width = 70;
            // 
            // colTags
            // 
            this.colTags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTags.DataPropertyName = "Tags";
            this.colTags.FillWeight = 300F;
            this.colTags.HeaderText = "Tags";
            this.colTags.MinimumWidth = 110;
            this.colTags.Name = "colTags";
            this.colTags.Visible = false;
            this.colTags.Width = 300;
            // 
            // colTagsE
            // 
            this.colTagsE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTagsE.DataPropertyName = "TagsEntity";
            this.colTagsE.FillWeight = 300F;
            this.colTagsE.HeaderText = "Tags";
            this.colTagsE.MinimumWidth = 100;
            this.colTagsE.Name = "colTagsE";
            this.colTagsE.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTagsE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colTagsE.Width = 300;
            // 
            // colUrl
            // 
            this.colUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colUrl.DataPropertyName = "FileUrl";
            this.colUrl.FillWeight = 250F;
            this.colUrl.HeaderText = "Url";
            this.colUrl.MinimumWidth = 250;
            this.colUrl.Name = "colUrl";
            this.colUrl.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUrl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colUrl.Width = 250;
            // 
            // colMD5
            // 
            this.colMD5.DataPropertyName = "MD5";
            this.colMD5.HeaderText = "MD5";
            this.colMD5.Name = "colMD5";
            this.colMD5.Width = 62;
            // 
            // colQuery
            // 
            this.colQuery.DataPropertyName = "Query";
            this.colQuery.HeaderText = "Query";
            this.colQuery.Name = "colQuery";
            this.colQuery.Width = 72;
            // 
            // colSourceUrl
            // 
            this.colSourceUrl.DataPropertyName = "Source";
            this.colSourceUrl.HeaderText = "Source";
            this.colSourceUrl.MinimumWidth = 25;
            this.colSourceUrl.Name = "colSourceUrl";
            this.colSourceUrl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSourceUrl.Width = 78;
            // 
            // colReferer
            // 
            this.colReferer.DataPropertyName = "Referer";
            this.colReferer.HeaderText = "Referer";
            this.colReferer.Name = "colReferer";
            this.colReferer.ReadOnly = true;
            this.colReferer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colReferer.Width = 81;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 73;
            // 
            // ctxMenuList
            // 
            this.ctxMenuList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchByParentToolStripMenuItem,
            this.addSelectedRowsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.reloadThumbnailToolStripMenuItem,
            this.toolStripMenuItem4,
            this.resetColumnOrderToolStripMenuItem});
            this.ctxMenuList.Name = "contextMenuStrip3";
            this.ctxMenuList.Size = new System.Drawing.Size(212, 112);
            this.ctxMenuList.Text = "ListGrid Menu";
            // 
            // searchByParentToolStripMenuItem
            // 
            this.searchByParentToolStripMenuItem.Name = "searchByParentToolStripMenuItem";
            this.searchByParentToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.searchByParentToolStripMenuItem.Text = "Search by parent";
            this.searchByParentToolStripMenuItem.Click += new System.EventHandler(this.searchByParentToolStripMenuItem_Click);
            // 
            // addSelectedRowsToolStripMenuItem
            // 
            this.addSelectedRowsToolStripMenuItem.Name = "addSelectedRowsToolStripMenuItem";
            this.addSelectedRowsToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.addSelectedRowsToolStripMenuItem.Text = "Add selected rows";
            this.addSelectedRowsToolStripMenuItem.Click += new System.EventHandler(this.addSelectedRowsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(208, 6);
            // 
            // reloadThumbnailToolStripMenuItem
            // 
            this.reloadThumbnailToolStripMenuItem.Name = "reloadThumbnailToolStripMenuItem";
            this.reloadThumbnailToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.reloadThumbnailToolStripMenuItem.Text = "Reload Thumbnail";
            this.reloadThumbnailToolStripMenuItem.Click += new System.EventHandler(this.reloadThumbnailToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(208, 6);
            // 
            // resetColumnOrderToolStripMenuItem
            // 
            this.resetColumnOrderToolStripMenuItem.Name = "resetColumnOrderToolStripMenuItem";
            this.resetColumnOrderToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.resetColumnOrderToolStripMenuItem.Text = "Reset Column Order";
            this.resetColumnOrderToolStripMenuItem.Click += new System.EventHandler(this.resetColumnOrderToolStripMenuItem_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnClearCompletedDownload);
            this.tabPage4.Controls.Add(this.btnBrowseFolder);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.btnLoadDownloadList);
            this.tabPage4.Controls.Add(this.btnSaveDownloadList);
            this.tabPage4.Controls.Add(this.btnCancelDownload);
            this.tabPage4.Controls.Add(this.btnClearDownloadList);
            this.tabPage4.Controls.Add(this.btnPauseDownload);
            this.tabPage4.Controls.Add(this.btnDownloadFiles);
            this.tabPage4.Controls.Add(this.txtSaveFolder);
            this.tabPage4.Controls.Add(this.dgvDownload);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage4.Size = new System.Drawing.Size(1259, 737);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Download List";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnClearCompletedDownload
            // 
            this.btnClearCompletedDownload.Location = new System.Drawing.Point(119, 9);
            this.btnClearCompletedDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearCompletedDownload.Name = "btnClearCompletedDownload";
            this.btnClearCompletedDownload.Size = new System.Drawing.Size(131, 28);
            this.btnClearCompletedDownload.TabIndex = 16;
            this.btnClearCompletedDownload.Text = "Clear Completed";
            this.btnClearCompletedDownload.UseVisualStyleBackColor = true;
            this.btnClearCompletedDownload.Click += new System.EventHandler(this.btnClearCompletedDownload_Click);
            // 
            // btnBrowseFolder
            // 
            this.btnBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFolder.Location = new System.Drawing.Point(842, 9);
            this.btnBrowseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseFolder.Name = "btnBrowseFolder";
            this.btnBrowseFolder.Size = new System.Drawing.Size(80, 28);
            this.btnBrowseFolder.TabIndex = 5;
            this.btnBrowseFolder.Text = "Browse";
            this.btnBrowseFolder.UseVisualStyleBackColor = true;
            this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click_1);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(473, 15);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 17);
            this.label10.TabIndex = 15;
            this.label10.Text = "Save Folder";
            // 
            // btnLoadDownloadList
            // 
            this.btnLoadDownloadList.Location = new System.Drawing.Point(365, 9);
            this.btnLoadDownloadList.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadDownloadList.Name = "btnLoadDownloadList";
            this.btnLoadDownloadList.Size = new System.Drawing.Size(100, 28);
            this.btnLoadDownloadList.TabIndex = 3;
            this.btnLoadDownloadList.Text = "Load List";
            this.btnLoadDownloadList.UseVisualStyleBackColor = true;
            this.btnLoadDownloadList.Click += new System.EventHandler(this.btnLoadDownloadList_Click);
            // 
            // btnSaveDownloadList
            // 
            this.btnSaveDownloadList.Location = new System.Drawing.Point(257, 9);
            this.btnSaveDownloadList.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveDownloadList.Name = "btnSaveDownloadList";
            this.btnSaveDownloadList.Size = new System.Drawing.Size(100, 28);
            this.btnSaveDownloadList.TabIndex = 2;
            this.btnSaveDownloadList.Text = "Save List";
            this.btnSaveDownloadList.UseVisualStyleBackColor = true;
            this.btnSaveDownloadList.Click += new System.EventHandler(this.btnSaveDownloadList_Click);
            // 
            // btnCancelDownload
            // 
            this.btnCancelDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelDownload.Enabled = false;
            this.btnCancelDownload.Location = new System.Drawing.Point(930, 9);
            this.btnCancelDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelDownload.Name = "btnCancelDownload";
            this.btnCancelDownload.Size = new System.Drawing.Size(100, 28);
            this.btnCancelDownload.TabIndex = 6;
            this.btnCancelDownload.Text = "Cancel";
            this.btnCancelDownload.UseVisualStyleBackColor = true;
            this.btnCancelDownload.Click += new System.EventHandler(this.btnCancelDownload_Click);
            // 
            // btnClearDownloadList
            // 
            this.btnClearDownloadList.Location = new System.Drawing.Point(11, 9);
            this.btnClearDownloadList.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearDownloadList.Name = "btnClearDownloadList";
            this.btnClearDownloadList.Size = new System.Drawing.Size(100, 28);
            this.btnClearDownloadList.TabIndex = 1;
            this.btnClearDownloadList.Text = "Clear List";
            this.btnClearDownloadList.UseVisualStyleBackColor = true;
            this.btnClearDownloadList.Click += new System.EventHandler(this.btnClearDownloadList_Click);
            // 
            // btnPauseDownload
            // 
            this.btnPauseDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPauseDownload.Enabled = false;
            this.btnPauseDownload.Location = new System.Drawing.Point(1038, 9);
            this.btnPauseDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnPauseDownload.Name = "btnPauseDownload";
            this.btnPauseDownload.Size = new System.Drawing.Size(100, 28);
            this.btnPauseDownload.TabIndex = 7;
            this.btnPauseDownload.Text = "Pause";
            this.btnPauseDownload.UseVisualStyleBackColor = true;
            this.btnPauseDownload.Click += new System.EventHandler(this.btnPauseDownload_Click);
            // 
            // btnDownloadFiles
            // 
            this.btnDownloadFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadFiles.Location = new System.Drawing.Point(1146, 9);
            this.btnDownloadFiles.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownloadFiles.Name = "btnDownloadFiles";
            this.btnDownloadFiles.Size = new System.Drawing.Size(100, 28);
            this.btnDownloadFiles.TabIndex = 8;
            this.btnDownloadFiles.Text = "Download";
            this.btnDownloadFiles.UseVisualStyleBackColor = true;
            this.btnDownloadFiles.Click += new System.EventHandler(this.btnDownloadFiles_Click);
            // 
            // txtSaveFolder
            // 
            this.txtSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSaveFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "SaveFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtSaveFolder.Location = new System.Drawing.Point(567, 11);
            this.txtSaveFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtSaveFolder.Name = "txtSaveFolder";
            this.txtSaveFolder.Size = new System.Drawing.Size(266, 22);
            this.txtSaveFolder.TabIndex = 4;
            this.txtSaveFolder.Text = global::DanbooruDownloader3.Properties.Settings.Default.SaveFolder;
            // 
            // dgvDownload
            // 
            this.dgvDownload.AllowUserToAddRows = false;
            this.dgvDownload.AllowUserToDeleteRows = false;
            this.dgvDownload.AllowUserToOrderColumns = true;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDownload.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDownload.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDownload.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDownload.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDownload.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDownload.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndex,
            this.colPreview2,
            this.colProgress2,
            this.colProvider2,
            this.colId2,
            this.colRating2,
            this.colTags2,
            this.colUrl2,
            this.colReferer2,
            this.colMD52,
            this.colQuery2,
            this.colFilename,
            this.colDownloadStart2});
            this.dgvDownload.ContextMenuStrip = this.ctxMenuDownload;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDownload.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvDownload.Location = new System.Drawing.Point(11, 43);
            this.dgvDownload.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDownload.Name = "dgvDownload";
            this.dgvDownload.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDownload.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvDownload.RowHeadersVisible = false;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDownload.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvDownload.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDownload.RowTemplate.Height = 24;
            this.dgvDownload.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDownload.Size = new System.Drawing.Size(1238, 683);
            this.dgvDownload.TabIndex = 7;
            this.dgvDownload.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDownload_CellContentClick);
            this.dgvDownload.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvDownload_RowsAdded);
            this.dgvDownload.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvDownload_MouseClick);
            // 
            // colIndex
            // 
            this.colIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colIndex.FillWeight = 35F;
            this.colIndex.Frozen = true;
            this.colIndex.HeaderText = "#";
            this.colIndex.MinimumWidth = 25;
            this.colIndex.Name = "colIndex";
            this.colIndex.Width = 25;
            // 
            // colPreview2
            // 
            this.colPreview2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colPreview2.DataPropertyName = "ThumbnailImage";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPreview2.DefaultCellStyle = dataGridViewCellStyle9;
            this.colPreview2.FillWeight = 300F;
            this.colPreview2.Frozen = true;
            this.colPreview2.HeaderText = "Preview";
            this.colPreview2.Image = global::DanbooruDownloader3.Properties.Resources.NOT_AVAILABLE;
            this.colPreview2.MinimumWidth = 150;
            this.colPreview2.Name = "colPreview2";
            this.colPreview2.Width = 150;
            // 
            // colProgress2
            // 
            this.colProgress2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colProgress2.FillWeight = 200F;
            this.colProgress2.Frozen = true;
            this.colProgress2.HeaderText = "Progress";
            this.colProgress2.MinimumWidth = 150;
            this.colProgress2.Name = "colProgress2";
            this.colProgress2.Width = 150;
            // 
            // colProvider2
            // 
            this.colProvider2.DataPropertyName = "provider";
            this.colProvider2.Frozen = true;
            this.colProvider2.HeaderText = "Provider";
            this.colProvider2.Name = "colProvider2";
            this.colProvider2.Width = 86;
            // 
            // colId2
            // 
            this.colId2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colId2.DataPropertyName = "Id";
            this.colId2.FillWeight = 75F;
            this.colId2.HeaderText = "Id";
            this.colId2.MinimumWidth = 75;
            this.colId2.Name = "colId2";
            this.colId2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colId2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colId2.Width = 75;
            // 
            // colRating2
            // 
            this.colRating2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRating2.DataPropertyName = "Rating";
            this.colRating2.FillWeight = 25F;
            this.colRating2.HeaderText = "Rating";
            this.colRating2.MinimumWidth = 25;
            this.colRating2.Name = "colRating2";
            this.colRating2.Width = 25;
            // 
            // colTags2
            // 
            this.colTags2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTags2.DataPropertyName = "Tags";
            this.colTags2.FillWeight = 300F;
            this.colTags2.HeaderText = "Tags";
            this.colTags2.MinimumWidth = 110;
            this.colTags2.Name = "colTags2";
            this.colTags2.Width = 300;
            // 
            // colUrl2
            // 
            this.colUrl2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colUrl2.DataPropertyName = "FileUrl";
            this.colUrl2.FillWeight = 200F;
            this.colUrl2.HeaderText = "Url";
            this.colUrl2.MinimumWidth = 200;
            this.colUrl2.Name = "colUrl2";
            this.colUrl2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUrl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colUrl2.Width = 200;
            // 
            // colReferer2
            // 
            this.colReferer2.DataPropertyName = "Referer";
            this.colReferer2.HeaderText = "Referer";
            this.colReferer2.Name = "colReferer2";
            this.colReferer2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colReferer2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colReferer2.Width = 81;
            // 
            // colMD52
            // 
            this.colMD52.DataPropertyName = "MD5";
            this.colMD52.HeaderText = "MD5";
            this.colMD52.Name = "colMD52";
            this.colMD52.Width = 62;
            // 
            // colQuery2
            // 
            this.colQuery2.DataPropertyName = "Query";
            this.colQuery2.HeaderText = "Query";
            this.colQuery2.Name = "colQuery2";
            this.colQuery2.Width = 72;
            // 
            // colFilename
            // 
            this.colFilename.DataPropertyName = "Filename";
            this.colFilename.HeaderText = "Filename";
            this.colFilename.Name = "colFilename";
            this.colFilename.Width = 90;
            // 
            // colDownloadStart2
            // 
            this.colDownloadStart2.HeaderText = "Download Start";
            this.colDownloadStart2.Name = "colDownloadStart2";
            this.colDownloadStart2.Width = 118;
            // 
            // ctxMenuDownload
            // 
            this.ctxMenuDownload.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenuDownload.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolveFileUrlToolStripMenuItem,
            this.toolStripMenuItem5,
            this.deleteToolStripMenuItem});
            this.ctxMenuDownload.Name = "contextMenuStrip1";
            this.ctxMenuDownload.Size = new System.Drawing.Size(233, 58);
            // 
            // resolveFileUrlToolStripMenuItem
            // 
            this.resolveFileUrlToolStripMenuItem.Name = "resolveFileUrlToolStripMenuItem";
            this.resolveFileUrlToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.resolveFileUrlToolStripMenuItem.Text = "Resolve File Url";
            this.resolveFileUrlToolStripMenuItem.Click += new System.EventHandler(this.resolveFileUrlToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(229, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.deleteToolStripMenuItem.Text = "&Delete Selected Row(s)";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnLoadList);
            this.tabPage5.Controls.Add(this.btnSaveBatchList);
            this.tabPage5.Controls.Add(this.btnClearAll);
            this.tabPage5.Controls.Add(this.btnClearCompleted);
            this.tabPage5.Controls.Add(this.btnPauseBatchJob);
            this.tabPage5.Controls.Add(this.btnStopBatchJob);
            this.tabPage5.Controls.Add(this.btnStartBatchJob);
            this.tabPage5.Controls.Add(this.btnAddBatchJob);
            this.tabPage5.Controls.Add(this.cbxAbortOnError);
            this.tabPage5.Controls.Add(this.dgvBatchJob);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage5.Size = new System.Drawing.Size(1259, 737);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Full Batch Mode";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnLoadList
            // 
            this.btnLoadList.Location = new System.Drawing.Point(808, 7);
            this.btnLoadList.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadList.Name = "btnLoadList";
            this.btnLoadList.Size = new System.Drawing.Size(93, 28);
            this.btnLoadList.TabIndex = 9;
            this.btnLoadList.Text = "Load List";
            this.btnLoadList.UseVisualStyleBackColor = true;
            this.btnLoadList.Click += new System.EventHandler(this.btnLoadList_Click);
            // 
            // btnSaveBatchList
            // 
            this.btnSaveBatchList.Location = new System.Drawing.Point(707, 7);
            this.btnSaveBatchList.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveBatchList.Name = "btnSaveBatchList";
            this.btnSaveBatchList.Size = new System.Drawing.Size(93, 28);
            this.btnSaveBatchList.TabIndex = 8;
            this.btnSaveBatchList.Text = "Save List";
            this.btnSaveBatchList.UseVisualStyleBackColor = true;
            this.btnSaveBatchList.Click += new System.EventHandler(this.btnSaveBatchList_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(605, 7);
            this.btnClearAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(93, 28);
            this.btnClearAll.TabIndex = 7;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnClearCompleted
            // 
            this.btnClearCompleted.Location = new System.Drawing.Point(449, 7);
            this.btnClearCompleted.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearCompleted.Name = "btnClearCompleted";
            this.btnClearCompleted.Size = new System.Drawing.Size(148, 28);
            this.btnClearCompleted.TabIndex = 6;
            this.btnClearCompleted.Text = "Clear Completed";
            this.btnClearCompleted.UseVisualStyleBackColor = true;
            this.btnClearCompleted.Click += new System.EventHandler(this.btnClearCompleted_Click);
            // 
            // btnPauseBatchJob
            // 
            this.btnPauseBatchJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPauseBatchJob.Enabled = false;
            this.btnPauseBatchJob.Location = new System.Drawing.Point(942, 7);
            this.btnPauseBatchJob.Margin = new System.Windows.Forms.Padding(4);
            this.btnPauseBatchJob.Name = "btnPauseBatchJob";
            this.btnPauseBatchJob.Size = new System.Drawing.Size(148, 28);
            this.btnPauseBatchJob.TabIndex = 5;
            this.btnPauseBatchJob.Text = "Pause Batch Job";
            this.btnPauseBatchJob.UseVisualStyleBackColor = true;
            this.btnPauseBatchJob.Click += new System.EventHandler(this.btnPauseBatchJob_Click);
            // 
            // btnStopBatchJob
            // 
            this.btnStopBatchJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopBatchJob.Enabled = false;
            this.btnStopBatchJob.Location = new System.Drawing.Point(1098, 7);
            this.btnStopBatchJob.Margin = new System.Windows.Forms.Padding(4);
            this.btnStopBatchJob.Name = "btnStopBatchJob";
            this.btnStopBatchJob.Size = new System.Drawing.Size(148, 28);
            this.btnStopBatchJob.TabIndex = 3;
            this.btnStopBatchJob.Text = "Stop Batch Job";
            this.btnStopBatchJob.UseVisualStyleBackColor = true;
            this.btnStopBatchJob.Click += new System.EventHandler(this.btnStopBatchJob_Click);
            // 
            // btnStartBatchJob
            // 
            this.btnStartBatchJob.Location = new System.Drawing.Point(160, 7);
            this.btnStartBatchJob.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartBatchJob.Name = "btnStartBatchJob";
            this.btnStartBatchJob.Size = new System.Drawing.Size(148, 28);
            this.btnStartBatchJob.TabIndex = 2;
            this.btnStartBatchJob.Text = "Start Batch Job";
            this.btnStartBatchJob.UseVisualStyleBackColor = true;
            this.btnStartBatchJob.Click += new System.EventHandler(this.btnStartBatchJob_Click);
            // 
            // btnAddBatchJob
            // 
            this.btnAddBatchJob.Location = new System.Drawing.Point(11, 7);
            this.btnAddBatchJob.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddBatchJob.Name = "btnAddBatchJob";
            this.btnAddBatchJob.Size = new System.Drawing.Size(141, 28);
            this.btnAddBatchJob.TabIndex = 1;
            this.btnAddBatchJob.Text = "Add Batch Job";
            this.btnAddBatchJob.UseVisualStyleBackColor = true;
            this.btnAddBatchJob.Click += new System.EventHandler(this.btnAddBatchJob_Click);
            // 
            // cbxAbortOnError
            // 
            this.cbxAbortOnError.AutoSize = true;
            this.cbxAbortOnError.Checked = global::DanbooruDownloader3.Properties.Settings.Default.batchAbortOnError;
            this.cbxAbortOnError.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "batchAbortOnError", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbxAbortOnError.Location = new System.Drawing.Point(317, 14);
            this.cbxAbortOnError.Margin = new System.Windows.Forms.Padding(4);
            this.cbxAbortOnError.Name = "cbxAbortOnError";
            this.cbxAbortOnError.Size = new System.Drawing.Size(123, 21);
            this.cbxAbortOnError.TabIndex = 4;
            this.cbxAbortOnError.Text = "Abort On Error";
            this.cbxAbortOnError.UseVisualStyleBackColor = true;
            // 
            // dgvBatchJob
            // 
            this.dgvBatchJob.AllowUserToAddRows = false;
            this.dgvBatchJob.AllowUserToDeleteRows = false;
            this.dgvBatchJob.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBatchJob.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBatchJob.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvBatchJob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatchJob.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBatchId,
            this.colBatchTagQuery,
            this.colBatchLimit,
            this.colBatchRating,
            this.colBatchStartPage,
            this.colBatchProviders,
            this.colBatchSaveFolder,
            this.colBatchStatus});
            this.dgvBatchJob.ContextMenuStrip = this.ctxMenuBatch;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBatchJob.DefaultCellStyle = dataGridViewCellStyle16;
            this.dgvBatchJob.Location = new System.Drawing.Point(11, 43);
            this.dgvBatchJob.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBatchJob.Name = "dgvBatchJob";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBatchJob.RowHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvBatchJob.RowHeadersVisible = false;
            this.dgvBatchJob.RowTemplate.Height = 24;
            this.dgvBatchJob.ShowCellErrors = false;
            this.dgvBatchJob.ShowCellToolTips = false;
            this.dgvBatchJob.ShowEditingIcon = false;
            this.dgvBatchJob.ShowRowErrors = false;
            this.dgvBatchJob.Size = new System.Drawing.Size(1235, 683);
            this.dgvBatchJob.TabIndex = 0;
            this.dgvBatchJob.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBatchJob_RowsAdded);
            this.dgvBatchJob.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvBatchJob_MouseDown);
            // 
            // colBatchId
            // 
            this.colBatchId.FillWeight = 25F;
            this.colBatchId.Frozen = true;
            this.colBatchId.HeaderText = "#";
            this.colBatchId.MinimumWidth = 25;
            this.colBatchId.Name = "colBatchId";
            this.colBatchId.ReadOnly = true;
            this.colBatchId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colBatchId.Width = 25;
            // 
            // colBatchTagQuery
            // 
            this.colBatchTagQuery.DataPropertyName = "TagQuery";
            this.colBatchTagQuery.HeaderText = "Tags Query";
            this.colBatchTagQuery.Name = "colBatchTagQuery";
            this.colBatchTagQuery.ReadOnly = true;
            // 
            // colBatchLimit
            // 
            this.colBatchLimit.DataPropertyName = "Limit";
            this.colBatchLimit.FillWeight = 50F;
            this.colBatchLimit.HeaderText = "Limit";
            this.colBatchLimit.Name = "colBatchLimit";
            this.colBatchLimit.ReadOnly = true;
            this.colBatchLimit.Width = 50;
            // 
            // colBatchRating
            // 
            this.colBatchRating.DataPropertyName = "Rating";
            this.colBatchRating.FillWeight = 75F;
            this.colBatchRating.HeaderText = "Rating";
            this.colBatchRating.Name = "colBatchRating";
            this.colBatchRating.Width = 75;
            // 
            // colBatchStartPage
            // 
            this.colBatchStartPage.DataPropertyName = "StartPage";
            this.colBatchStartPage.FillWeight = 50F;
            this.colBatchStartPage.HeaderText = "Start Page";
            this.colBatchStartPage.Name = "colBatchStartPage";
            this.colBatchStartPage.Width = 50;
            // 
            // colBatchProviders
            // 
            this.colBatchProviders.DataPropertyName = "ProviderName";
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colBatchProviders.DefaultCellStyle = dataGridViewCellStyle14;
            this.colBatchProviders.FillWeight = 150F;
            this.colBatchProviders.HeaderText = "Providers";
            this.colBatchProviders.MinimumWidth = 150;
            this.colBatchProviders.Name = "colBatchProviders";
            this.colBatchProviders.ReadOnly = true;
            this.colBatchProviders.Width = 150;
            // 
            // colBatchSaveFolder
            // 
            this.colBatchSaveFolder.DataPropertyName = "SaveFolder";
            this.colBatchSaveFolder.HeaderText = "Save Folder";
            this.colBatchSaveFolder.Name = "colBatchSaveFolder";
            this.colBatchSaveFolder.ReadOnly = true;
            // 
            // colBatchStatus
            // 
            this.colBatchStatus.DataPropertyName = "Status";
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colBatchStatus.DefaultCellStyle = dataGridViewCellStyle15;
            this.colBatchStatus.FillWeight = 300F;
            this.colBatchStatus.HeaderText = "Status";
            this.colBatchStatus.MinimumWidth = 300;
            this.colBatchStatus.Name = "colBatchStatus";
            this.colBatchStatus.ReadOnly = true;
            this.colBatchStatus.Width = 351;
            // 
            // ctxMenuBatch
            // 
            this.ctxMenuBatch.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenuBatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1});
            this.ctxMenuBatch.Name = "contextMenuStrip5";
            this.ctxMenuBatch.Size = new System.Drawing.Size(150, 28);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(149, 24);
            this.deleteToolStripMenuItem1.Text = "Delete Job";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.btnSaveConfig);
            this.tabPage2.Controls.Add(this.btnUpdate);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.txtFilenameHelp);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1259, 737);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkProcessDeletedPost);
            this.groupBox5.Controls.Add(this.chkHideBlaclistedImage);
            this.groupBox5.Controls.Add(this.chkUseGlobalProviderTags);
            this.groupBox5.Controls.Add(this.chkMinimizeTray);
            this.groupBox5.Controls.Add(this.chkAutoFocus);
            this.groupBox5.Controls.Add(this.chkLogging);
            this.groupBox5.Controls.Add(this.chkUseTagColor);
            this.groupBox5.Location = new System.Drawing.Point(772, 7);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(285, 217);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Application Settings";
            // 
            // chkProcessDeletedPost
            // 
            this.chkProcessDeletedPost.AutoSize = true;
            this.chkProcessDeletedPost.Checked = global::DanbooruDownloader3.Properties.Settings.Default.ProcessDeletedPost;
            this.chkProcessDeletedPost.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "ProcessDeletedPost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkProcessDeletedPost.Location = new System.Drawing.Point(8, 181);
            this.chkProcessDeletedPost.Margin = new System.Windows.Forms.Padding(4);
            this.chkProcessDeletedPost.Name = "chkProcessDeletedPost";
            this.chkProcessDeletedPost.Size = new System.Drawing.Size(166, 21);
            this.chkProcessDeletedPost.TabIndex = 21;
            this.chkProcessDeletedPost.Text = "Process Deleted Post";
            this.chkProcessDeletedPost.UseVisualStyleBackColor = true;
            // 
            // chkHideBlaclistedImage
            // 
            this.chkHideBlaclistedImage.AutoSize = true;
            this.chkHideBlaclistedImage.Checked = global::DanbooruDownloader3.Properties.Settings.Default.HideBlacklistedImage;
            this.chkHideBlaclistedImage.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "HideBlacklistedImage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkHideBlaclistedImage.Location = new System.Drawing.Point(8, 155);
            this.chkHideBlaclistedImage.Margin = new System.Windows.Forms.Padding(4);
            this.chkHideBlaclistedImage.Name = "chkHideBlaclistedImage";
            this.chkHideBlaclistedImage.Size = new System.Drawing.Size(172, 21);
            this.chkHideBlaclistedImage.TabIndex = 19;
            this.chkHideBlaclistedImage.Text = "Hide Blacklisted Image";
            this.chkHideBlaclistedImage.UseVisualStyleBackColor = true;
            // 
            // chkUseGlobalProviderTags
            // 
            this.chkUseGlobalProviderTags.AutoSize = true;
            this.chkUseGlobalProviderTags.Checked = global::DanbooruDownloader3.Properties.Settings.Default.UseGlobalProviderTags;
            this.chkUseGlobalProviderTags.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "UseGlobalProviderTags", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkUseGlobalProviderTags.Location = new System.Drawing.Point(8, 129);
            this.chkUseGlobalProviderTags.Margin = new System.Windows.Forms.Padding(4);
            this.chkUseGlobalProviderTags.Name = "chkUseGlobalProviderTags";
            this.chkUseGlobalProviderTags.Size = new System.Drawing.Size(160, 21);
            this.chkUseGlobalProviderTags.TabIndex = 18;
            this.chkUseGlobalProviderTags.Text = "Use Global Tags.xml";
            this.chkUseGlobalProviderTags.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeTray
            // 
            this.chkMinimizeTray.AutoSize = true;
            this.chkMinimizeTray.Checked = global::DanbooruDownloader3.Properties.Settings.Default.MinimizeToTray;
            this.chkMinimizeTray.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "MinimizeToTray", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkMinimizeTray.Location = new System.Drawing.Point(8, 26);
            this.chkMinimizeTray.Margin = new System.Windows.Forms.Padding(4);
            this.chkMinimizeTray.Name = "chkMinimizeTray";
            this.chkMinimizeTray.Size = new System.Drawing.Size(183, 21);
            this.chkMinimizeTray.TabIndex = 7;
            this.chkMinimizeTray.Text = "Minimize to System Tray";
            this.chkMinimizeTray.UseVisualStyleBackColor = true;
            this.chkMinimizeTray.CheckedChanged += new System.EventHandler(this.chkMinimizeTray_CheckedChanged);
            // 
            // chkAutoFocus
            // 
            this.chkAutoFocus.AutoSize = true;
            this.chkAutoFocus.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AutoFocusCurrent;
            this.chkAutoFocus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoFocus.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AutoFocusCurrent", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAutoFocus.Location = new System.Drawing.Point(8, 52);
            this.chkAutoFocus.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoFocus.Name = "chkAutoFocus";
            this.chkAutoFocus.Size = new System.Drawing.Size(244, 21);
            this.chkAutoFocus.TabIndex = 6;
            this.chkAutoFocus.Text = "Auto Focus Currently Downloaded";
            this.chkAutoFocus.UseVisualStyleBackColor = true;
            // 
            // chkLogging
            // 
            this.chkLogging.AutoSize = true;
            this.chkLogging.Checked = global::DanbooruDownloader3.Properties.Settings.Default.EnableLogging;
            this.chkLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogging.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "EnableLogging", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkLogging.Location = new System.Drawing.Point(8, 78);
            this.chkLogging.Margin = new System.Windows.Forms.Padding(4);
            this.chkLogging.Name = "chkLogging";
            this.chkLogging.Size = new System.Drawing.Size(129, 21);
            this.chkLogging.TabIndex = 17;
            this.chkLogging.Text = "Enable Logging";
            this.chkLogging.UseVisualStyleBackColor = true;
            this.chkLogging.CheckedChanged += new System.EventHandler(this.chkLogging_CheckedChanged);
            // 
            // chkUseTagColor
            // 
            this.chkUseTagColor.AutoSize = true;
            this.chkUseTagColor.Checked = global::DanbooruDownloader3.Properties.Settings.Default.UseColoredTag;
            this.chkUseTagColor.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "UseColoredTag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkUseTagColor.Location = new System.Drawing.Point(8, 103);
            this.chkUseTagColor.Margin = new System.Windows.Forms.Padding(4);
            this.chkUseTagColor.Name = "chkUseTagColor";
            this.chkUseTagColor.Size = new System.Drawing.Size(144, 21);
            this.chkUseTagColor.TabIndex = 8;
            this.chkUseTagColor.Text = "Use Colored Tags";
            this.chkUseTagColor.UseVisualStyleBackColor = true;
            this.chkUseTagColor.CheckedChanged += new System.EventHandler(this.chkUseTagColor_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.pictureBox1);
            this.groupBox4.Controls.Add(this.checkBox3);
            this.groupBox4.Controls.Add(this.checkBox4);
            this.groupBox4.Controls.Add(this.checkBox5);
            this.groupBox4.Location = new System.Drawing.Point(11, 616);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(752, 50);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Danbooru Listing";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AutoLoadNextPage;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AutoLoadNextPage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(476, 18);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(194, 21);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "Auto Load Next StartPage";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AppendList;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AppendList", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(245, 18);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(105, 21);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = "Append List";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 1);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = global::DanbooruDownloader3.Properties.Settings.Default.SaveQuery;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "SaveQuery", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3.Location = new System.Drawing.Point(8, 20);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(105, 21);
            this.checkBox3.TabIndex = 17;
            this.checkBox3.Text = "Save Query";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = global::DanbooruDownloader3.Properties.Settings.Default.AutoLoadList;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "AutoLoadList", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4.Location = new System.Drawing.Point(125, 20);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(121, 21);
            this.checkBox4.TabIndex = 18;
            this.checkBox4.Text = "Auto Load List";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = global::DanbooruDownloader3.Properties.Settings.Default.LoadPreview;
            this.checkBox5.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "LoadPreview", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox5.Location = new System.Drawing.Point(359, 18);
            this.checkBox5.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(115, 21);
            this.checkBox5.TabIndex = 20;
            this.checkBox5.Text = "Load Preview";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtNoFault);
            this.groupBox2.Controls.Add(this.txtNoCircle);
            this.groupBox2.Controls.Add(this.txtNoChara);
            this.groupBox2.Controls.Add(this.txtNoCopyright);
            this.groupBox2.Controls.Add(this.txtNoArtist);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.chkBlacklistOnlyGeneral);
            this.groupBox2.Controls.Add(this.chkIgnoreForGeneralTag);
            this.groupBox2.Controls.Add(this.chkReplaceMode);
            this.groupBox2.Controls.Add(this.lblColorUnknown);
            this.groupBox2.Controls.Add(this.chkBlacklistTagsUseRegex);
            this.groupBox2.Controls.Add(this.chkIgnoreTagsUseRegex);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.txtIgnoredTags);
            this.groupBox2.Controls.Add(this.txtAutoCompleteLimit);
            this.groupBox2.Controls.Add(this.txtCircleTagGrouping);
            this.groupBox2.Controls.Add(this.txtFaultsTagGrouping);
            this.groupBox2.Controls.Add(this.txtCharaTagGrouping);
            this.groupBox2.Controls.Add(this.txtTagReplacement);
            this.groupBox2.Controls.Add(this.txtCopyTagGrouping);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.lblColorBlacklistedTag);
            this.groupBox2.Controls.Add(this.chkTagAutoComplete);
            this.groupBox2.Controls.Add(this.lblColorFaults);
            this.groupBox2.Controls.Add(this.txtArtistTagGrouping);
            this.groupBox2.Controls.Add(this.lblColorCircle);
            this.groupBox2.Controls.Add(this.lblColorChara);
            this.groupBox2.Controls.Add(this.lblColorCopy);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.txtTagBlacklist);
            this.groupBox2.Controls.Add(this.lblColorArtist);
            this.groupBox2.Controls.Add(this.lblColorGeneral);
            this.groupBox2.Location = new System.Drawing.Point(11, 358);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(753, 250);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tagging";
            // 
            // txtNoFault
            // 
            this.txtNoFault.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "tagNoFaultValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNoFault.Location = new System.Drawing.Point(123, 168);
            this.txtNoFault.Name = "txtNoFault";
            this.txtNoFault.Size = new System.Drawing.Size(91, 22);
            this.txtNoFault.TabIndex = 41;
            this.txtNoFault.Text = global::DanbooruDownloader3.Properties.Settings.Default.tagNoFaultValue;
            // 
            // txtNoCircle
            // 
            this.txtNoCircle.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "tagNoCircleValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNoCircle.Location = new System.Drawing.Point(123, 138);
            this.txtNoCircle.Name = "txtNoCircle";
            this.txtNoCircle.Size = new System.Drawing.Size(91, 22);
            this.txtNoCircle.TabIndex = 40;
            this.txtNoCircle.Text = global::DanbooruDownloader3.Properties.Settings.Default.tagNoCircleValue;
            // 
            // txtNoChara
            // 
            this.txtNoChara.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "tagNoCharaValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNoChara.Location = new System.Drawing.Point(123, 108);
            this.txtNoChara.Name = "txtNoChara";
            this.txtNoChara.Size = new System.Drawing.Size(91, 22);
            this.txtNoChara.TabIndex = 39;
            this.txtNoChara.Text = global::DanbooruDownloader3.Properties.Settings.Default.tagNoCharaValue;
            // 
            // txtNoCopyright
            // 
            this.txtNoCopyright.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "tagNoCopyrightValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNoCopyright.Location = new System.Drawing.Point(123, 76);
            this.txtNoCopyright.Name = "txtNoCopyright";
            this.txtNoCopyright.Size = new System.Drawing.Size(91, 22);
            this.txtNoCopyright.TabIndex = 38;
            this.txtNoCopyright.Text = global::DanbooruDownloader3.Properties.Settings.Default.tagNoCopyrightValue;
            // 
            // txtNoArtist
            // 
            this.txtNoArtist.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "tagNoArtistValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNoArtist.Location = new System.Drawing.Point(123, 44);
            this.txtNoArtist.Name = "txtNoArtist";
            this.txtNoArtist.Size = new System.Drawing.Size(91, 22);
            this.txtNoArtist.TabIndex = 37;
            this.txtNoArtist.Text = global::DanbooruDownloader3.Properties.Settings.Default.tagNoArtistValue;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(122, 21);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(66, 17);
            this.label30.TabIndex = 36;
            this.label30.Text = "No Value";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(82, 21);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(37, 17);
            this.label29.TabIndex = 35;
            this.label29.Text = "Limit";
            // 
            // chkBlacklistOnlyGeneral
            // 
            this.chkBlacklistOnlyGeneral.AutoSize = true;
            this.chkBlacklistOnlyGeneral.Checked = global::DanbooruDownloader3.Properties.Settings.Default.IsBlacklistOnlyForGeneralTags;
            this.chkBlacklistOnlyGeneral.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "IsBlacklistOnlyForGeneralTags", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBlacklistOnlyGeneral.Location = new System.Drawing.Point(223, 143);
            this.chkBlacklistOnlyGeneral.Name = "chkBlacklistOnlyGeneral";
            this.chkBlacklistOnlyGeneral.Size = new System.Drawing.Size(216, 21);
            this.chkBlacklistOnlyGeneral.TabIndex = 34;
            this.chkBlacklistOnlyGeneral.Text = "Blacklist only for General Tag";
            this.chkBlacklistOnlyGeneral.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreForGeneralTag
            // 
            this.chkIgnoreForGeneralTag.AutoSize = true;
            this.chkIgnoreForGeneralTag.Checked = global::DanbooruDownloader3.Properties.Settings.Default.IsIgnoreForGeneralTagOnly;
            this.chkIgnoreForGeneralTag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreForGeneralTag.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "IsIgnoreForGeneralTagOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkIgnoreForGeneralTag.Location = new System.Drawing.Point(490, 143);
            this.chkIgnoreForGeneralTag.Name = "chkIgnoreForGeneralTag";
            this.chkIgnoreForGeneralTag.Size = new System.Drawing.Size(205, 21);
            this.chkIgnoreForGeneralTag.TabIndex = 33;
            this.chkIgnoreForGeneralTag.Text = "Ignore only for General Tag";
            this.chkIgnoreForGeneralTag.UseVisualStyleBackColor = true;
            // 
            // chkReplaceMode
            // 
            this.chkReplaceMode.AutoSize = true;
            this.chkReplaceMode.Checked = global::DanbooruDownloader3.Properties.Settings.Default.isReplaceMode;
            this.chkReplaceMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceMode.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "isReplaceMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkReplaceMode.Location = new System.Drawing.Point(14, 216);
            this.chkReplaceMode.Name = "chkReplaceMode";
            this.chkReplaceMode.Size = new System.Drawing.Size(177, 21);
            this.chkReplaceMode.TabIndex = 32;
            this.chkReplaceMode.Text = "Replace if over the limit";
            this.chkReplaceMode.UseVisualStyleBackColor = true;
            // 
            // lblColorUnknown
            // 
            this.lblColorUnknown.AutoSize = true;
            this.lblColorUnknown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorUnknown.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorUnknown", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorUnknown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorUnknown.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorUnknown;
            this.lblColorUnknown.Location = new System.Drawing.Point(83, 196);
            this.lblColorUnknown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorUnknown.Name = "lblColorUnknown";
            this.lblColorUnknown.Size = new System.Drawing.Size(37, 17);
            this.lblColorUnknown.TabIndex = 31;
            this.lblColorUnknown.Text = "Unk.";
            this.lblColorUnknown.DoubleClick += new System.EventHandler(this.lblColorUnknown_DoubleClick);
            // 
            // chkBlacklistTagsUseRegex
            // 
            this.chkBlacklistTagsUseRegex.AutoSize = true;
            this.chkBlacklistTagsUseRegex.Checked = global::DanbooruDownloader3.Properties.Settings.Default.BlacklistTagsUseRegex;
            this.chkBlacklistTagsUseRegex.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "BlacklistTagsUseRegex", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBlacklistTagsUseRegex.Location = new System.Drawing.Point(341, 20);
            this.chkBlacklistTagsUseRegex.Margin = new System.Windows.Forms.Padding(4);
            this.chkBlacklistTagsUseRegex.Name = "chkBlacklistTagsUseRegex";
            this.chkBlacklistTagsUseRegex.Size = new System.Drawing.Size(99, 21);
            this.chkBlacklistTagsUseRegex.TabIndex = 30;
            this.chkBlacklistTagsUseRegex.Text = "Use Regex";
            this.chkBlacklistTagsUseRegex.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreTagsUseRegex
            // 
            this.chkIgnoreTagsUseRegex.AutoSize = true;
            this.chkIgnoreTagsUseRegex.Checked = global::DanbooruDownloader3.Properties.Settings.Default.IgnoreTagsUseRegex;
            this.chkIgnoreTagsUseRegex.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "IgnoreTagsUseRegex", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkIgnoreTagsUseRegex.Location = new System.Drawing.Point(609, 20);
            this.chkIgnoreTagsUseRegex.Margin = new System.Windows.Forms.Padding(4);
            this.chkIgnoreTagsUseRegex.Name = "chkIgnoreTagsUseRegex";
            this.chkIgnoreTagsUseRegex.Size = new System.Drawing.Size(99, 21);
            this.chkIgnoreTagsUseRegex.TabIndex = 29;
            this.chkIgnoreTagsUseRegex.Text = "Use Regex";
            this.chkIgnoreTagsUseRegex.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(491, 21);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(92, 17);
            this.label21.TabIndex = 28;
            this.label21.Text = "Ignored Tags";
            // 
            // txtIgnoredTags
            // 
            this.txtIgnoredTags.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagIgnored", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtIgnoredTags.Location = new System.Drawing.Point(491, 48);
            this.txtIgnoredTags.Margin = new System.Windows.Forms.Padding(4);
            this.txtIgnoredTags.Multiline = true;
            this.txtIgnoredTags.Name = "txtIgnoredTags";
            this.txtIgnoredTags.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIgnoredTags.Size = new System.Drawing.Size(223, 52);
            this.txtIgnoredTags.TabIndex = 27;
            this.txtIgnoredTags.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagIgnored;
            this.txtIgnoredTags.WordWrap = false;
            this.txtIgnoredTags.TextChanged += new System.EventHandler(this.txtIgnoredTags_TextChanged);
            // 
            // txtAutoCompleteLimit
            // 
            this.txtAutoCompleteLimit.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "AutoCompleteLimit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtAutoCompleteLimit.Location = new System.Drawing.Point(433, 108);
            this.txtAutoCompleteLimit.Margin = new System.Windows.Forms.Padding(4);
            this.txtAutoCompleteLimit.Name = "txtAutoCompleteLimit";
            this.txtAutoCompleteLimit.Size = new System.Drawing.Size(44, 22);
            this.txtAutoCompleteLimit.TabIndex = 19;
            this.txtAutoCompleteLimit.Text = global::DanbooruDownloader3.Properties.Settings.Default.AutoCompleteLimit;
            // 
            // txtCircleTagGrouping
            // 
            this.txtCircleTagGrouping.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagCircleGrouping", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtCircleTagGrouping.Location = new System.Drawing.Point(83, 138);
            this.txtCircleTagGrouping.Margin = new System.Windows.Forms.Padding(4);
            this.txtCircleTagGrouping.Name = "txtCircleTagGrouping";
            this.txtCircleTagGrouping.Size = new System.Drawing.Size(33, 22);
            this.txtCircleTagGrouping.TabIndex = 26;
            this.txtCircleTagGrouping.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagCircleGrouping;
            this.txtCircleTagGrouping.TextChanged += new System.EventHandler(this.txtCircleTagGrouping_TextChanged);
            // 
            // txtFaultsTagGrouping
            // 
            this.txtFaultsTagGrouping.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagFaultsGrouping", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFaultsTagGrouping.Location = new System.Drawing.Point(83, 168);
            this.txtFaultsTagGrouping.Margin = new System.Windows.Forms.Padding(4);
            this.txtFaultsTagGrouping.Name = "txtFaultsTagGrouping";
            this.txtFaultsTagGrouping.Size = new System.Drawing.Size(33, 22);
            this.txtFaultsTagGrouping.TabIndex = 25;
            this.txtFaultsTagGrouping.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagFaultsGrouping;
            this.txtFaultsTagGrouping.TextChanged += new System.EventHandler(this.txtFaultsTagGrouping_TextChanged);
            // 
            // txtCharaTagGrouping
            // 
            this.txtCharaTagGrouping.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagCharacterGrouping", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtCharaTagGrouping.Location = new System.Drawing.Point(83, 108);
            this.txtCharaTagGrouping.Margin = new System.Windows.Forms.Padding(4);
            this.txtCharaTagGrouping.Name = "txtCharaTagGrouping";
            this.txtCharaTagGrouping.Size = new System.Drawing.Size(33, 22);
            this.txtCharaTagGrouping.TabIndex = 24;
            this.txtCharaTagGrouping.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagCharacterGrouping;
            this.txtCharaTagGrouping.TextChanged += new System.EventHandler(this.txtCharaTagGrouping_TextChanged);
            // 
            // txtTagReplacement
            // 
            this.txtTagReplacement.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "EmptyTagReplacement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtTagReplacement.Location = new System.Drawing.Point(599, 108);
            this.txtTagReplacement.Margin = new System.Windows.Forms.Padding(4);
            this.txtTagReplacement.Name = "txtTagReplacement";
            this.txtTagReplacement.Size = new System.Drawing.Size(136, 22);
            this.txtTagReplacement.TabIndex = 11;
            this.txtTagReplacement.Text = global::DanbooruDownloader3.Properties.Settings.Default.EmptyTagReplacement;
            // 
            // txtCopyTagGrouping
            // 
            this.txtCopyTagGrouping.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagCopyrigthGrouping", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtCopyTagGrouping.Location = new System.Drawing.Point(83, 76);
            this.txtCopyTagGrouping.Margin = new System.Windows.Forms.Padding(4);
            this.txtCopyTagGrouping.Name = "txtCopyTagGrouping";
            this.txtCopyTagGrouping.Size = new System.Drawing.Size(33, 22);
            this.txtCopyTagGrouping.TabIndex = 23;
            this.txtCopyTagGrouping.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagCopyrigthGrouping;
            this.txtCopyTagGrouping.TextChanged += new System.EventHandler(this.txtCopyTagGrouping_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(487, 112);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(113, 17);
            this.label20.TabIndex = 10;
            this.label20.Text = "Empty Tag Repl.";
            // 
            // lblColorBlacklistedTag
            // 
            this.lblColorBlacklistedTag.AutoSize = true;
            this.lblColorBlacklistedTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorBlacklistedTag.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorBlacklistedTag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorBlacklistedTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorBlacklistedTag.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorBlacklistedTag;
            this.lblColorBlacklistedTag.Location = new System.Drawing.Point(122, 196);
            this.lblColorBlacklistedTag.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorBlacklistedTag.Name = "lblColorBlacklistedTag";
            this.lblColorBlacklistedTag.Size = new System.Drawing.Size(104, 17);
            this.lblColorBlacklistedTag.TabIndex = 16;
            this.lblColorBlacklistedTag.Text = "Blacklisted Tag";
            this.lblColorBlacklistedTag.DoubleClick += new System.EventHandler(this.lblColorBlacklistedTag_DoubleClick);
            // 
            // chkTagAutoComplete
            // 
            this.chkTagAutoComplete.AutoSize = true;
            this.chkTagAutoComplete.Checked = global::DanbooruDownloader3.Properties.Settings.Default.TagAutoComplete;
            this.chkTagAutoComplete.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "TagAutoComplete", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkTagAutoComplete.Location = new System.Drawing.Point(224, 111);
            this.chkTagAutoComplete.Margin = new System.Windows.Forms.Padding(4);
            this.chkTagAutoComplete.Name = "chkTagAutoComplete";
            this.chkTagAutoComplete.Size = new System.Drawing.Size(199, 21);
            this.chkTagAutoComplete.TabIndex = 15;
            this.chkTagAutoComplete.Text = "Use Tags Auto Complete #";
            this.chkTagAutoComplete.UseVisualStyleBackColor = true;
            // 
            // lblColorFaults
            // 
            this.lblColorFaults.AutoSize = true;
            this.lblColorFaults.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorFaults.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorFaults", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorFaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorFaults.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorFaults;
            this.lblColorFaults.Location = new System.Drawing.Point(29, 170);
            this.lblColorFaults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorFaults.Name = "lblColorFaults";
            this.lblColorFaults.Size = new System.Drawing.Size(46, 17);
            this.lblColorFaults.TabIndex = 14;
            this.lblColorFaults.Text = "Faults";
            this.lblColorFaults.DoubleClick += new System.EventHandler(this.lblColorFaults_DoubleClick);
            // 
            // txtArtistTagGrouping
            // 
            this.txtArtistTagGrouping.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagGroupingLimit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtArtistTagGrouping.Location = new System.Drawing.Point(83, 44);
            this.txtArtistTagGrouping.Margin = new System.Windows.Forms.Padding(4);
            this.txtArtistTagGrouping.Name = "txtArtistTagGrouping";
            this.txtArtistTagGrouping.Size = new System.Drawing.Size(33, 22);
            this.txtArtistTagGrouping.TabIndex = 22;
            this.txtArtistTagGrouping.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagGroupingLimit;
            this.txtArtistTagGrouping.TextChanged += new System.EventHandler(this.txtArtistTagGrouping_TextChanged);
            // 
            // lblColorCircle
            // 
            this.lblColorCircle.AutoSize = true;
            this.lblColorCircle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorCircle.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorCircle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorCircle.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorCircle;
            this.lblColorCircle.Location = new System.Drawing.Point(32, 140);
            this.lblColorCircle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorCircle.Name = "lblColorCircle";
            this.lblColorCircle.Size = new System.Drawing.Size(43, 17);
            this.lblColorCircle.TabIndex = 13;
            this.lblColorCircle.Text = "Circle";
            this.lblColorCircle.DoubleClick += new System.EventHandler(this.lblColorCircle_DoubleClick);
            // 
            // lblColorChara
            // 
            this.lblColorChara.AutoSize = true;
            this.lblColorChara.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorChara.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorCharacter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorChara.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorChara.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorCharacter;
            this.lblColorChara.Location = new System.Drawing.Point(9, 112);
            this.lblColorChara.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorChara.Name = "lblColorChara";
            this.lblColorChara.Size = new System.Drawing.Size(70, 17);
            this.lblColorChara.TabIndex = 12;
            this.lblColorChara.Text = "Character";
            this.lblColorChara.DoubleClick += new System.EventHandler(this.lblColorChara_DoubleClick);
            // 
            // lblColorCopy
            // 
            this.lblColorCopy.AutoSize = true;
            this.lblColorCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorCopy.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorCopyright", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorCopy.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorCopyright;
            this.lblColorCopy.Location = new System.Drawing.Point(9, 80);
            this.lblColorCopy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorCopy.Name = "lblColorCopy";
            this.lblColorCopy.Size = new System.Drawing.Size(68, 17);
            this.lblColorCopy.TabIndex = 11;
            this.lblColorCopy.Text = "Copyright";
            this.lblColorCopy.DoubleClick += new System.EventHandler(this.lblColorCopy_DoubleClick);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(7, 21);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(67, 17);
            this.label25.TabIndex = 21;
            this.label25.Text = "Grouping";
            // 
            // txtTagBlacklist
            // 
            this.txtTagBlacklist.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "TagBlacklist", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtTagBlacklist.Location = new System.Drawing.Point(223, 48);
            this.txtTagBlacklist.Margin = new System.Windows.Forms.Padding(4);
            this.txtTagBlacklist.Multiline = true;
            this.txtTagBlacklist.Name = "txtTagBlacklist";
            this.txtTagBlacklist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTagBlacklist.Size = new System.Drawing.Size(223, 52);
            this.txtTagBlacklist.TabIndex = 12;
            this.txtTagBlacklist.Text = global::DanbooruDownloader3.Properties.Settings.Default.TagBlacklist;
            this.txtTagBlacklist.WordWrap = false;
            this.txtTagBlacklist.TextChanged += new System.EventHandler(this.txtTagBlacklist_TextChanged);
            // 
            // lblColorArtist
            // 
            this.lblColorArtist.AutoSize = true;
            this.lblColorArtist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorArtist.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorArtist", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorArtist.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorArtist;
            this.lblColorArtist.Location = new System.Drawing.Point(35, 48);
            this.lblColorArtist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorArtist.Name = "lblColorArtist";
            this.lblColorArtist.Size = new System.Drawing.Size(40, 17);
            this.lblColorArtist.TabIndex = 10;
            this.lblColorArtist.Text = "Artist";
            this.lblColorArtist.DoubleClick += new System.EventHandler(this.lblColorArtist_DoubleClick);
            // 
            // lblColorGeneral
            // 
            this.lblColorGeneral.AutoSize = true;
            this.lblColorGeneral.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblColorGeneral.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::DanbooruDownloader3.Properties.Settings.Default, "ColorGeneral", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblColorGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorGeneral.ForeColor = global::DanbooruDownloader3.Properties.Settings.Default.ColorGeneral;
            this.lblColorGeneral.Location = new System.Drawing.Point(36, 196);
            this.lblColorGeneral.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorGeneral.Name = "lblColorGeneral";
            this.lblColorGeneral.Size = new System.Drawing.Size(39, 17);
            this.lblColorGeneral.TabIndex = 9;
            this.lblColorGeneral.Text = "Gen.";
            this.lblColorGeneral.DoubleClick += new System.EventHandler(this.lblColorGeneral_DoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkWriteDownloadedFileTxt);
            this.groupBox3.Controls.Add(this.chkDelayIncludeSkip);
            this.groupBox3.Controls.Add(this.txtBatchJobDelay);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.chkSaveFolderWhenExit);
            this.groupBox3.Controls.Add(this.btnBrowseDefaultSave);
            this.groupBox3.Controls.Add(this.txtDefaultSaveFolder);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.cbxImageSize);
            this.groupBox3.Controls.Add(this.chkRenameJpeg);
            this.groupBox3.Controls.Add(this.txtFilenameLength);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.chkOverwrite);
            this.groupBox3.Controls.Add(this.txtFilenameFormat);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(11, 204);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(753, 146);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Download";
            // 
            // chkWriteDownloadedFileTxt
            // 
            this.chkWriteDownloadedFileTxt.AutoSize = true;
            this.chkWriteDownloadedFileTxt.Checked = global::DanbooruDownloader3.Properties.Settings.Default.WriteDownloadedFile;
            this.chkWriteDownloadedFileTxt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteDownloadedFileTxt.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "WriteDownloadedFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkWriteDownloadedFileTxt.Location = new System.Drawing.Point(456, 118);
            this.chkWriteDownloadedFileTxt.Margin = new System.Windows.Forms.Padding(4);
            this.chkWriteDownloadedFileTxt.Name = "chkWriteDownloadedFileTxt";
            this.chkWriteDownloadedFileTxt.Size = new System.Drawing.Size(244, 21);
            this.chkWriteDownloadedFileTxt.TabIndex = 24;
            this.chkWriteDownloadedFileTxt.Text = "Write Downloaded File to Text File";
            this.chkWriteDownloadedFileTxt.UseVisualStyleBackColor = true;
            // 
            // chkDelayIncludeSkip
            // 
            this.chkDelayIncludeSkip.AutoSize = true;
            this.chkDelayIncludeSkip.Checked = global::DanbooruDownloader3.Properties.Settings.Default.DelayIncludeSkipped;
            this.chkDelayIncludeSkip.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "DelayIncludeSkipped", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkDelayIncludeSkip.Location = new System.Drawing.Point(192, 118);
            this.chkDelayIncludeSkip.Margin = new System.Windows.Forms.Padding(4);
            this.chkDelayIncludeSkip.Name = "chkDelayIncludeSkip";
            this.chkDelayIncludeSkip.Size = new System.Drawing.Size(139, 21);
            this.chkDelayIncludeSkip.TabIndex = 23;
            this.chkDelayIncludeSkip.Text = "Including skipped";
            this.chkDelayIncludeSkip.UseVisualStyleBackColor = true;
            // 
            // txtBatchJobDelay
            // 
            this.txtBatchJobDelay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "BatchJobDelay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtBatchJobDelay.Location = new System.Drawing.Point(133, 116);
            this.txtBatchJobDelay.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchJobDelay.Name = "txtBatchJobDelay";
            this.txtBatchJobDelay.Size = new System.Drawing.Size(45, 22);
            this.txtBatchJobDelay.TabIndex = 22;
            this.txtBatchJobDelay.Text = global::DanbooruDownloader3.Properties.Settings.Default.BatchJobDelay;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(11, 119);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(116, 17);
            this.label28.TabIndex = 21;
            this.label28.Text = "Batch Delay (ms)";
            // 
            // chkSaveFolderWhenExit
            // 
            this.chkSaveFolderWhenExit.AutoSize = true;
            this.chkSaveFolderWhenExit.Checked = global::DanbooruDownloader3.Properties.Settings.Default.SaveFolderWhenExit;
            this.chkSaveFolderWhenExit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "SaveFolderWhenExit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkSaveFolderWhenExit.Location = new System.Drawing.Point(600, 26);
            this.chkSaveFolderWhenExit.Margin = new System.Windows.Forms.Padding(4);
            this.chkSaveFolderWhenExit.Name = "chkSaveFolderWhenExit";
            this.chkSaveFolderWhenExit.Size = new System.Drawing.Size(129, 21);
            this.chkSaveFolderWhenExit.TabIndex = 20;
            this.chkSaveFolderWhenExit.Text = "Save When Exit";
            this.chkSaveFolderWhenExit.UseVisualStyleBackColor = true;
            // 
            // btnBrowseDefaultSave
            // 
            this.btnBrowseDefaultSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDefaultSave.Location = new System.Drawing.Point(504, 20);
            this.btnBrowseDefaultSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseDefaultSave.Name = "btnBrowseDefaultSave";
            this.btnBrowseDefaultSave.Size = new System.Drawing.Size(88, 28);
            this.btnBrowseDefaultSave.TabIndex = 19;
            this.btnBrowseDefaultSave.Text = "Browse";
            this.btnBrowseDefaultSave.UseVisualStyleBackColor = true;
            this.btnBrowseDefaultSave.Click += new System.EventHandler(this.btnBrowseDefaultSave_Click);
            // 
            // txtDefaultSaveFolder
            // 
            this.txtDefaultSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultSaveFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "SaveFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtDefaultSaveFolder.Location = new System.Drawing.Point(147, 23);
            this.txtDefaultSaveFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtDefaultSaveFolder.Name = "txtDefaultSaveFolder";
            this.txtDefaultSaveFolder.Size = new System.Drawing.Size(348, 22);
            this.txtDefaultSaveFolder.TabIndex = 17;
            this.txtDefaultSaveFolder.Text = global::DanbooruDownloader3.Properties.Settings.Default.SaveFolder;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(299, 91);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 17);
            this.label22.TabIndex = 16;
            this.label22.Text = "Image Size";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(11, 27);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(133, 17);
            this.label23.TabIndex = 18;
            this.label23.Text = "Default Save Folder";
            // 
            // cbxImageSize
            // 
            this.cbxImageSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "ImageSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbxImageSize.FormattingEnabled = true;
            this.cbxImageSize.Items.AddRange(new object[] {
            "Full",
            "Sample",
            "Jpeg",
            "Thumb"});
            this.cbxImageSize.Location = new System.Drawing.Point(385, 87);
            this.cbxImageSize.Margin = new System.Windows.Forms.Padding(4);
            this.cbxImageSize.Name = "cbxImageSize";
            this.cbxImageSize.Size = new System.Drawing.Size(61, 24);
            this.cbxImageSize.TabIndex = 15;
            this.cbxImageSize.Text = global::DanbooruDownloader3.Properties.Settings.Default.ImageSize;
            this.cbxImageSize.TextChanged += new System.EventHandler(this.cbxImageSize_TextChanged);
            // 
            // chkRenameJpeg
            // 
            this.chkRenameJpeg.AutoSize = true;
            this.chkRenameJpeg.Checked = global::DanbooruDownloader3.Properties.Settings.Default.RenameJpeg;
            this.chkRenameJpeg.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "RenameJpeg", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkRenameJpeg.Location = new System.Drawing.Point(572, 90);
            this.chkRenameJpeg.Margin = new System.Windows.Forms.Padding(4);
            this.chkRenameJpeg.Name = "chkRenameJpeg";
            this.chkRenameJpeg.Size = new System.Drawing.Size(161, 21);
            this.chkRenameJpeg.TabIndex = 14;
            this.chkRenameJpeg.Text = "Rename .jpeg to .jpg";
            this.chkRenameJpeg.UseVisualStyleBackColor = true;
            // 
            // txtFilenameLength
            // 
            this.txtFilenameLength.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "filenameLengthLimit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFilenameLength.Location = new System.Drawing.Point(133, 87);
            this.txtFilenameLength.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilenameLength.Name = "txtFilenameLength";
            this.txtFilenameLength.Size = new System.Drawing.Size(45, 22);
            this.txtFilenameLength.TabIndex = 5;
            this.txtFilenameLength.Text = global::DanbooruDownloader3.Properties.Settings.Default.filenameLengthLimit;
            this.txtFilenameLength.TextChanged += new System.EventHandler(this.txtFilenameLength_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 91);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 17);
            this.label14.TabIndex = 4;
            this.label14.Text = "Filename Limit";
            // 
            // chkOverwrite
            // 
            this.chkOverwrite.AutoSize = true;
            this.chkOverwrite.Checked = global::DanbooruDownloader3.Properties.Settings.Default.overwrite;
            this.chkOverwrite.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "overwrite", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkOverwrite.Location = new System.Drawing.Point(456, 90);
            this.chkOverwrite.Margin = new System.Windows.Forms.Padding(4);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new System.Drawing.Size(116, 21);
            this.chkOverwrite.TabIndex = 3;
            this.chkOverwrite.Text = "Overwrite File";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            // 
            // txtFilenameFormat
            // 
            this.txtFilenameFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "filenameFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFilenameFormat.Location = new System.Drawing.Point(147, 55);
            this.txtFilenameFormat.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilenameFormat.Name = "txtFilenameFormat";
            this.txtFilenameFormat.Size = new System.Drawing.Size(589, 22);
            this.txtFilenameFormat.TabIndex = 1;
            this.txtFilenameFormat.Text = global::DanbooruDownloader3.Properties.Settings.Default.filenameFormat;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 58);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "Filename Format";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(920, 543);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(136, 41);
            this.btnSaveConfig.TabIndex = 2;
            this.btnSaveConfig.Text = "Save";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(772, 543);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(141, 41);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update tags.xml";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkEnableCompression);
            this.groupBox1.Controls.Add(this.txtAcceptLanguage);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.chkEnableCookie);
            this.groupBox1.Controls.Add(this.btnCookie);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.txtDelay);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.linkUrl);
            this.groupBox1.Controls.Add(this.chkProxyLogin);
            this.groupBox1.Controls.Add(this.txtProxyPassword);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.txtProxyUsername);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.chkPadUserAgent);
            this.groupBox1.Controls.Add(this.txtRetry);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtTimeout);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtUserAgent);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.chkUseProxy);
            this.groupBox1.Controls.Add(this.txtProxyPort);
            this.groupBox1.Controls.Add(this.txtProxyAddress);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(11, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(753, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Internet Settings";
            // 
            // chkEnableCompression
            // 
            this.chkEnableCompression.AutoSize = true;
            this.chkEnableCompression.Checked = global::DanbooruDownloader3.Properties.Settings.Default.EnableCompression;
            this.chkEnableCompression.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableCompression.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "EnableCompression", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkEnableCompression.Location = new System.Drawing.Point(385, 154);
            this.chkEnableCompression.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnableCompression.Name = "chkEnableCompression";
            this.chkEnableCompression.Size = new System.Drawing.Size(160, 21);
            this.chkEnableCompression.TabIndex = 22;
            this.chkEnableCompression.Text = "Enable Compression";
            this.chkEnableCompression.UseVisualStyleBackColor = true;
            this.chkEnableCompression.CheckedChanged += new System.EventHandler(this.chkEnableCompression_CheckedChanged);
            // 
            // txtAcceptLanguage
            // 
            this.txtAcceptLanguage.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "AcceptLanguage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtAcceptLanguage.Location = new System.Drawing.Point(135, 151);
            this.txtAcceptLanguage.Margin = new System.Windows.Forms.Padding(4);
            this.txtAcceptLanguage.Name = "txtAcceptLanguage";
            this.txtAcceptLanguage.Size = new System.Drawing.Size(237, 22);
            this.txtAcceptLanguage.TabIndex = 21;
            this.txtAcceptLanguage.Text = global::DanbooruDownloader3.Properties.Settings.Default.AcceptLanguage;
            this.txtAcceptLanguage.TextChanged += new System.EventHandler(this.txtAcceptLanguage_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(8, 155);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(119, 17);
            this.label27.TabIndex = 20;
            this.label27.Text = "Accept Language";
            // 
            // chkEnableCookie
            // 
            this.chkEnableCookie.AutoSize = true;
            this.chkEnableCookie.Checked = global::DanbooruDownloader3.Properties.Settings.Default.enableCookie;
            this.chkEnableCookie.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableCookie.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "enableCookie", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkEnableCookie.Location = new System.Drawing.Point(619, 26);
            this.chkEnableCookie.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnableCookie.Name = "chkEnableCookie";
            this.chkEnableCookie.Size = new System.Drawing.Size(121, 21);
            this.chkEnableCookie.TabIndex = 18;
            this.chkEnableCookie.Text = "Enable Cookie";
            this.chkEnableCookie.UseVisualStyleBackColor = true;
            this.chkEnableCookie.CheckedChanged += new System.EventHandler(this.chkEnableCookie_CheckedChanged);
            // 
            // btnCookie
            // 
            this.btnCookie.Location = new System.Drawing.Point(645, 57);
            this.btnCookie.Margin = new System.Windows.Forms.Padding(4);
            this.btnCookie.Name = "btnCookie";
            this.btnCookie.Size = new System.Drawing.Size(100, 28);
            this.btnCookie.TabIndex = 19;
            this.btnCookie.Text = "Cookie";
            this.btnCookie.UseVisualStyleBackColor = true;
            this.btnCookie.Click += new System.EventHandler(this.btnCookie_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(303, 124);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(15, 17);
            this.label26.TabIndex = 18;
            this.label26.Text = "s";
            // 
            // txtDelay
            // 
            this.txtDelay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "delay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtDelay.Location = new System.Drawing.Point(272, 121);
            this.txtDelay.Margin = new System.Windows.Forms.Padding(4);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(29, 22);
            this.txtDelay.TabIndex = 17;
            this.txtDelay.Text = global::DanbooruDownloader3.Properties.Settings.Default.delay;
            this.txtDelay.TextChanged += new System.EventHandler(this.txtDelay_TextChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(224, 124);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(44, 17);
            this.label24.TabIndex = 16;
            this.label24.Text = "Delay";
            // 
            // linkUrl
            // 
            this.linkUrl.AutoSize = true;
            this.linkUrl.Location = new System.Drawing.Point(335, 124);
            this.linkUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkUrl.Name = "linkUrl";
            this.linkUrl.Size = new System.Drawing.Size(396, 17);
            this.linkUrl.TabIndex = 15;
            this.linkUrl.TabStop = true;
            this.linkUrl.Text = "http://nandaka.wordpress.com/tag/danbooru-batch-download/";
            this.linkUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUrl_LinkClicked);
            // 
            // chkProxyLogin
            // 
            this.chkProxyLogin.AutoSize = true;
            this.chkProxyLogin.Checked = global::DanbooruDownloader3.Properties.Settings.Default.UseProxyLogin;
            this.chkProxyLogin.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "UseProxyLogin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkProxyLogin.Location = new System.Drawing.Point(504, 62);
            this.chkProxyLogin.Margin = new System.Windows.Forms.Padding(4);
            this.chkProxyLogin.Name = "chkProxyLogin";
            this.chkProxyLogin.Size = new System.Drawing.Size(133, 21);
            this.chkProxyLogin.TabIndex = 14;
            this.chkProxyLogin.Text = "Use Proxy Login";
            this.chkProxyLogin.UseVisualStyleBackColor = true;
            this.chkProxyLogin.CheckedChanged += new System.EventHandler(this.chkProxyLogin_CheckedChanged);
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "ProxyPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtProxyPassword.Enabled = false;
            this.txtProxyPassword.Location = new System.Drawing.Point(371, 59);
            this.txtProxyPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(124, 22);
            this.txtProxyPassword.TabIndex = 13;
            this.txtProxyPassword.Text = global::DanbooruDownloader3.Properties.Settings.Default.ProxyPassword;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            this.txtProxyPassword.TextChanged += new System.EventHandler(this.txtProxyPassword_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(253, 63);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(108, 17);
            this.label19.TabIndex = 12;
            this.label19.Text = "Proxy Password";
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "ProxyUsername", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtProxyUsername.Enabled = false;
            this.txtProxyUsername.Location = new System.Drawing.Point(128, 59);
            this.txtProxyUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(124, 22);
            this.txtProxyUsername.TabIndex = 11;
            this.txtProxyUsername.Text = global::DanbooruDownloader3.Properties.Settings.Default.ProxyUsername;
            this.txtProxyUsername.TextChanged += new System.EventHandler(this.txtProxyUsername_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 63);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(112, 17);
            this.label18.TabIndex = 10;
            this.label18.Text = "Proxy Username";
            // 
            // chkPadUserAgent
            // 
            this.chkPadUserAgent.AutoSize = true;
            this.chkPadUserAgent.Checked = global::DanbooruDownloader3.Properties.Settings.Default.PadUserAgent;
            this.chkPadUserAgent.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "PadUserAgent", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkPadUserAgent.Location = new System.Drawing.Point(12, 123);
            this.chkPadUserAgent.Margin = new System.Windows.Forms.Padding(4);
            this.chkPadUserAgent.Name = "chkPadUserAgent";
            this.chkPadUserAgent.Size = new System.Drawing.Size(130, 21);
            this.chkPadUserAgent.TabIndex = 9;
            this.chkPadUserAgent.Text = "Pad User Agent";
            this.chkPadUserAgent.UseVisualStyleBackColor = true;
            // 
            // txtRetry
            // 
            this.txtRetry.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "retry", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtRetry.Location = new System.Drawing.Point(193, 121);
            this.txtRetry.Margin = new System.Windows.Forms.Padding(4);
            this.txtRetry.Name = "txtRetry";
            this.txtRetry.Size = new System.Drawing.Size(21, 22);
            this.txtRetry.TabIndex = 8;
            this.txtRetry.Text = global::DanbooruDownloader3.Properties.Settings.Default.retry;
            this.txtRetry.TextChanged += new System.EventHandler(this.txtRetry_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(143, 124);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(42, 17);
            this.label17.TabIndex = 7;
            this.label17.Text = "Retry";
            // 
            // txtTimeout
            // 
            this.txtTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "Timeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtTimeout.Location = new System.Drawing.Point(528, 23);
            this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(79, 22);
            this.txtTimeout.TabIndex = 8;
            this.txtTimeout.Text = global::DanbooruDownloader3.Properties.Settings.Default.Timeout;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(431, 27);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(91, 17);
            this.label16.TabIndex = 7;
            this.label16.Text = "Timeout (ms)";
            // 
            // txtUserAgent
            // 
            this.txtUserAgent.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "UserAgent", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtUserAgent.Location = new System.Drawing.Point(115, 91);
            this.txtUserAgent.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserAgent.Name = "txtUserAgent";
            this.txtUserAgent.Size = new System.Drawing.Size(629, 22);
            this.txtUserAgent.TabIndex = 7;
            this.txtUserAgent.Text = global::DanbooruDownloader3.Properties.Settings.Default.UserAgent;
            this.txtUserAgent.TextChanged += new System.EventHandler(this.txtUserAgent_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 95);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 17);
            this.label15.TabIndex = 6;
            this.label15.Text = "User Agents";
            // 
            // chkUseProxy
            // 
            this.chkUseProxy.AutoSize = true;
            this.chkUseProxy.Checked = global::DanbooruDownloader3.Properties.Settings.Default.UseProxy;
            this.chkUseProxy.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "UseProxy", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkUseProxy.Location = new System.Drawing.Point(324, 26);
            this.chkUseProxy.Margin = new System.Windows.Forms.Padding(4);
            this.chkUseProxy.Name = "chkUseProxy";
            this.chkUseProxy.Size = new System.Drawing.Size(94, 21);
            this.chkUseProxy.TabIndex = 2;
            this.chkUseProxy.Text = "Use Proxy";
            this.chkUseProxy.UseVisualStyleBackColor = true;
            this.chkUseProxy.CheckedChanged += new System.EventHandler(this.chkUseProxy_CheckedChanged);
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "ProxyPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtProxyPort.Location = new System.Drawing.Point(261, 23);
            this.txtProxyPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(53, 22);
            this.txtProxyPort.TabIndex = 4;
            this.txtProxyPort.Text = global::DanbooruDownloader3.Properties.Settings.Default.ProxyPort;
            this.txtProxyPort.TextChanged += new System.EventHandler(this.txtProxyPort_TextChanged);
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "ProxyAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtProxyAddress.Location = new System.Drawing.Point(128, 23);
            this.txtProxyAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.Size = new System.Drawing.Size(124, 22);
            this.txtProxyAddress.TabIndex = 3;
            this.txtProxyAddress.Text = global::DanbooruDownloader3.Properties.Settings.Default.ProxyAddress;
            this.txtProxyAddress.TextChanged += new System.EventHandler(this.txtProxyAddress_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 27);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 17);
            this.label11.TabIndex = 1;
            this.label11.Text = "Proxy Address";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(252, 27);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = ":";
            // 
            // txtFilenameHelp
            // 
            this.txtFilenameHelp.Location = new System.Drawing.Point(772, 231);
            this.txtFilenameHelp.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilenameHelp.Multiline = true;
            this.txtFilenameHelp.Name = "txtFilenameHelp";
            this.txtFilenameHelp.ReadOnly = true;
            this.txtFilenameHelp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFilenameHelp.Size = new System.Drawing.Size(284, 298);
            this.txtFilenameHelp.TabIndex = 2;
            this.txtFilenameHelp.WordWrap = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtLog);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(1259, 737);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Log";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.ContextMenuStrip = this.ctxMenuLog;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(4, 4);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(1251, 729);
            this.txtLog.TabIndex = 0;
            // 
            // ctxMenuLog
            // 
            this.ctxMenuLog.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.clearLogToolStripMenuItem});
            this.ctxMenuLog.Name = "contextMenuStrip2";
            this.ctxMenuLog.Size = new System.Drawing.Size(142, 52);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "JSON or XML|*.json;*.xml";
            // 
            // timGifAnimation
            // 
            this.timGifAnimation.Tick += new System.EventHandler(this.timGifAnimation_Tick);
            // 
            // timLoadNextPage
            // 
            this.timLoadNextPage.Interval = 500;
            this.timLoadNextPage.Tick += new System.EventHandler(this.timLoadNextPage_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.tsCount,
            this.tsTotalCount,
            this.tsProgressBar,
            this.tsProgress2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 771);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1267, 25);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(49, 20);
            this.tsStatus.Text = "Status";
            // 
            // tsCount
            // 
            this.tsCount.Name = "tsCount";
            this.tsCount.Size = new System.Drawing.Size(82, 20);
            this.tsCount.Text = "| Count = 0";
            // 
            // tsTotalCount
            // 
            this.tsTotalCount.Name = "tsTotalCount";
            this.tsTotalCount.Size = new System.Drawing.Size(120, 20);
            this.tsTotalCount.Text = "| Total Count = 0";
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsProgressBar.Name = "tsProgressBar";
            this.tsProgressBar.Size = new System.Drawing.Size(267, 20);
            this.tsProgressBar.Visible = false;
            // 
            // tsProgress2
            // 
            this.tsProgress2.Name = "tsProgress2";
            this.tsProgress2.Size = new System.Drawing.Size(133, 20);
            this.tsProgress2.Visible = false;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.DefaultExt = "xml";
            this.openFileDialog2.Filter = "XML|*.xml";
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.DefaultExt = "xml";
            this.saveFileDialog2.Filter = "XML|*.xml";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.ctxMenuSysTray;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Danbooru Downloader";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // ctxMenuSysTray
            // 
            this.ctxMenuSysTray.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxMenuSysTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.toolStripMenuItem2});
            this.ctxMenuSysTray.Name = "contextMenuStrip4";
            this.ctxMenuSysTray.Size = new System.Drawing.Size(115, 58);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(114, 24);
            this.toolStripMenuItem1.Text = "Show";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 24);
            this.toolStripMenuItem2.Text = "Close";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.DataPropertyName = "ThumbnailImage";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle18;
            this.dataGridViewImageColumn1.FillWeight = 150F;
            this.dataGridViewImageColumn1.Frozen = true;
            this.dataGridViewImageColumn1.HeaderText = "Preview";
            this.dataGridViewImageColumn1.Image = global::DanbooruDownloader3.Properties.Resources.AJAX_LOADING;
            this.dataGridViewImageColumn1.MinimumWidth = 150;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 150;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn2.DataPropertyName = "ThumbnailImage";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle19;
            this.dataGridViewImageColumn2.FillWeight = 150F;
            this.dataGridViewImageColumn2.Frozen = true;
            this.dataGridViewImageColumn2.HeaderText = "Preview";
            this.dataGridViewImageColumn2.Image = global::DanbooruDownloader3.Properties.Resources.NOT_AVAILABLE;
            this.dataGridViewImageColumn2.MinimumWidth = 150;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Width = 150;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.FillWeight = 35F;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "#";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "provider";
            this.dataGridViewTextBoxColumn2.HeaderText = "provider";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn3.FillWeight = 75F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Rating";
            this.dataGridViewTextBoxColumn4.FillWeight = 25F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Rating";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 25;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Tags";
            this.dataGridViewTextBoxColumn5.FillWeight = 300F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Tags";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 110;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            this.dataGridViewTextBoxColumn5.Width = 300;
            // 
            // tagsColumn1
            // 
            this.tagsColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tagsColumn1.DataPropertyName = "TagsEntity";
            this.tagsColumn1.FillWeight = 300F;
            this.tagsColumn1.HeaderText = "Tags";
            this.tagsColumn1.MinimumWidth = 100;
            this.tagsColumn1.Name = "tagsColumn1";
            this.tagsColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagsColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.tagsColumn1.Width = 300;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "MD5";
            this.dataGridViewTextBoxColumn6.HeaderText = "MD5";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 55;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Query";
            this.dataGridViewTextBoxColumn7.HeaderText = "Query";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 60;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn8.FillWeight = 35F;
            this.dataGridViewTextBoxColumn8.Frozen = true;
            this.dataGridViewTextBoxColumn8.HeaderText = "#";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 25;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn9.FillWeight = 200F;
            this.dataGridViewTextBoxColumn9.Frozen = true;
            this.dataGridViewTextBoxColumn9.HeaderText = "Progress";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 150;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "provider";
            this.dataGridViewTextBoxColumn10.Frozen = true;
            this.dataGridViewTextBoxColumn10.HeaderText = "provider";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 70;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn11.FillWeight = 75F;
            this.dataGridViewTextBoxColumn11.HeaderText = "Id";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 75;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn12.DataPropertyName = "Rating";
            this.dataGridViewTextBoxColumn12.FillWeight = 25F;
            this.dataGridViewTextBoxColumn12.HeaderText = "Rating";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Width = 25;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Tags";
            this.dataGridViewTextBoxColumn13.FillWeight = 300F;
            this.dataGridViewTextBoxColumn13.HeaderText = "Tags";
            this.dataGridViewTextBoxColumn13.MinimumWidth = 110;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Width = 300;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "MD5";
            this.dataGridViewTextBoxColumn14.HeaderText = "MD5";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Width = 55;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "Query";
            this.dataGridViewTextBoxColumn15.HeaderText = "Query";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Width = 60;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "Filename";
            this.dataGridViewTextBoxColumn16.FillWeight = 25F;
            this.dataGridViewTextBoxColumn16.Frozen = true;
            this.dataGridViewTextBoxColumn16.HeaderText = "#";
            this.dataGridViewTextBoxColumn16.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Width = 25;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "SearchTags";
            this.dataGridViewTextBoxColumn17.FillWeight = 25F;
            this.dataGridViewTextBoxColumn17.Frozen = true;
            this.dataGridViewTextBoxColumn17.HeaderText = "Tags Query";
            this.dataGridViewTextBoxColumn17.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn17.Width = 25;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "Limit";
            this.dataGridViewTextBoxColumn18.FillWeight = 25F;
            this.dataGridViewTextBoxColumn18.Frozen = true;
            this.dataGridViewTextBoxColumn18.HeaderText = "Limit";
            this.dataGridViewTextBoxColumn18.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn18.Width = 25;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "ProviderName";
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn19.DefaultCellStyle = dataGridViewCellStyle20;
            this.dataGridViewTextBoxColumn19.FillWeight = 150F;
            this.dataGridViewTextBoxColumn19.HeaderText = "Providers";
            this.dataGridViewTextBoxColumn19.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            this.dataGridViewTextBoxColumn19.Width = 150;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "SaveFolder";
            this.dataGridViewTextBoxColumn20.FillWeight = 75F;
            this.dataGridViewTextBoxColumn20.HeaderText = "Save Folder";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            this.dataGridViewTextBoxColumn20.Width = 75;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn21.DataPropertyName = "Status";
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn21.DefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridViewTextBoxColumn21.FillWeight = 300F;
            this.dataGridViewTextBoxColumn21.HeaderText = "Status";
            this.dataGridViewTextBoxColumn21.MinimumWidth = 300;
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.DataPropertyName = "ProviderName";
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn22.DefaultCellStyle = dataGridViewCellStyle22;
            this.dataGridViewTextBoxColumn22.FillWeight = 150F;
            this.dataGridViewTextBoxColumn22.HeaderText = "Providers";
            this.dataGridViewTextBoxColumn22.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            this.dataGridViewTextBoxColumn22.Width = 150;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.DataPropertyName = "SaveFolder";
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn23.DefaultCellStyle = dataGridViewCellStyle23;
            this.dataGridViewTextBoxColumn23.FillWeight = 150F;
            this.dataGridViewTextBoxColumn23.HeaderText = "Save Folder";
            this.dataGridViewTextBoxColumn23.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.ReadOnly = true;
            this.dataGridViewTextBoxColumn23.Width = 150;
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.DataPropertyName = "Status";
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn24.DefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridViewTextBoxColumn24.FillWeight = 300F;
            this.dataGridViewTextBoxColumn24.HeaderText = "Status";
            this.dataGridViewTextBoxColumn24.MinimumWidth = 300;
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            this.dataGridViewTextBoxColumn24.ReadOnly = true;
            this.dataGridViewTextBoxColumn24.Width = 351;
            // 
            // dataGridViewTextBoxColumn25
            // 
            this.dataGridViewTextBoxColumn25.DataPropertyName = "Status";
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn25.DefaultCellStyle = dataGridViewCellStyle25;
            this.dataGridViewTextBoxColumn25.FillWeight = 300F;
            this.dataGridViewTextBoxColumn25.HeaderText = "Status";
            this.dataGridViewTextBoxColumn25.MinimumWidth = 300;
            this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
            this.dataGridViewTextBoxColumn25.ReadOnly = true;
            this.dataGridViewTextBoxColumn25.Width = 351;
            // 
            // tagsColumn2
            // 
            this.tagsColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tagsColumn2.DataPropertyName = "TagsEntity";
            this.tagsColumn2.FillWeight = 300F;
            this.tagsColumn2.HeaderText = "Tags";
            this.tagsColumn2.MinimumWidth = 100;
            this.tagsColumn2.Name = "tagsColumn2";
            this.tagsColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagsColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.tagsColumn2.Width = 300;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(800, 600);
            this.ClientSize = new System.Drawing.Size(1267, 796);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "Danbooru Downloader ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbxSearch.ResumeLayout(false);
            this.gbxSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.gbxDanbooru.ResumeLayout(false);
            this.gbxDanbooru.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.gbxList.ResumeLayout(false);
            this.gbxList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ctxMenuList.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDownload)).EndInit();
            this.ctxMenuDownload.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchJob)).EndInit();
            this.ctxMenuBatch.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ctxMenuLog.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ctxMenuSysTray.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gbxSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.CheckBox chkGenerate;
        private System.Windows.Forms.CheckBox chkNotRating;
        private System.Windows.Forms.ComboBox cbxRating;
        private System.Windows.Forms.ComboBox cbxOrder;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox gbxList;
        private System.Windows.Forms.Button btnList;
        private System.Windows.Forms.Button btnBrowseListFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtListFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseProxy;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.TextBox txtProxyAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnAddDownload;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtFilenameFormat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtFilenameHelp;
        private System.Windows.Forms.CheckBox chkOverwrite;
        private System.Windows.Forms.TextBox txtFilenameLength;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Timer timGifAnimation;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Timer timLoadNextPage;
        private System.Windows.Forms.TextBox txtUserAgent;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox chkAutoFocus;
        private System.Windows.Forms.Button btnListCancel;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsCount;
        private System.Windows.Forms.ToolStripStatusLabel tsTotalCount;
        private System.Windows.Forms.ToolStripProgressBar tsProgressBar;
        private System.Windows.Forms.Button btnClearDownloadList;
        private System.Windows.Forms.Button btnPauseDownload;
        private System.Windows.Forms.Button btnDownloadFiles;
        private System.Windows.Forms.Button btnCancelDownload;
        private System.Windows.Forms.Button btnLoadDownloadList;
        private System.Windows.Forms.Button btnSaveDownloadList;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkMinimizeTray;
        private System.Windows.Forms.Button btnBrowseFolder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSaveFolder;
        private System.Windows.Forms.ContextMenuStrip ctxMenuDownload;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar tsProgress2;
        private System.Windows.Forms.TextBox txtRetry;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkPadUserAgent;
        private System.Windows.Forms.ContextMenuStrip ctxMenuLog;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.Button btnSearchHelp;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.ContextMenuStrip ctxMenuList;
        private System.Windows.Forms.ToolStripMenuItem searchByParentToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxMenuSysTray;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem addSelectedRowsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnAddBatchJob;
        private System.Windows.Forms.Button btnStartBatchJob;
        private System.Windows.Forms.Button btnStopBatchJob;
        private System.Windows.Forms.CheckBox chkProxyLogin;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtProxyUsername;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox cbxAbortOnError;
        private System.Windows.Forms.Button btnPauseBatchJob;
        private System.Windows.Forms.Button btnClearCompleted;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.ContextMenuStrip ctxMenuBatch;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.LinkLabel linkUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DanbooruDownloader3.CustomControl.TagsColumn tagsColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.CheckBox chkUseTagColor;
        private System.Windows.Forms.Label lblColorGeneral;
        private System.Windows.Forms.Label lblColorArtist;
        private System.Windows.Forms.Label lblColorCopy;
        private System.Windows.Forms.Label lblColorChara;
        private System.Windows.Forms.Label lblColorCircle;
        private System.Windows.Forms.Label lblColorFaults;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.CheckBox chkTagAutoComplete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtTagReplacement;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtTagBlacklist;
        private System.Windows.Forms.Label lblColorBlacklistedTag;
        private System.Windows.Forms.CheckBox chkRenameJpeg;
        private DanbooruDownloader3.CustomControl.TagsColumn tagsColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private System.Windows.Forms.CheckBox chkLogging;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox cbxImageSize;
        private System.Windows.Forms.GroupBox gbxDanbooru;
        private System.Windows.Forms.CheckBox chkAutoLoadNext;
        private System.Windows.Forms.CheckBox chkAppendList;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.CheckBox chkSaveQuery;
        private System.Windows.Forms.CheckBox chkAutoLoadList;
        private System.Windows.Forms.CheckBox chkLoadPreview;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Button btnBrowseDefaultSave;
        private System.Windows.Forms.TextBox txtDefaultSaveFolder;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ListBox lbxAutoComplete;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
        private System.Windows.Forms.TextBox txtAutoCompleteLimit;
        private System.Windows.Forms.CheckBox chkSaveFolderWhenExit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem reloadThumbnailToolStripMenuItem;
        private CustomControl.gfDataGridView dgvList;
        private CustomControl.gfDataGridView dgvDownload;
        private CustomControl.gfDataGridView dgvBatchJob;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem resetColumnOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolveFileUrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.TextBox txtArtistTagGrouping;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtCircleTagGrouping;
        private System.Windows.Forms.TextBox txtFaultsTagGrouping;
        private System.Windows.Forms.TextBox txtCharaTagGrouping;
        private System.Windows.Forms.TextBox txtCopyTagGrouping;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtIgnoredTags;
        private System.Windows.Forms.CheckBox chkIgnoreTagsUseRegex;
        private System.Windows.Forms.CheckBox chkBlacklistTagsUseRegex;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.TextBox txtDelay;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnSaveBatchList;
        private System.Windows.Forms.Button btnLoadList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchTagQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchRating;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchStartPage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchProviders;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchSaveFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchStatus;
        private System.Windows.Forms.Button btnClearCompletedDownload;
        private System.Windows.Forms.CheckBox chkEnableCookie;
        private System.Windows.Forms.Button btnCookie;
        private System.Windows.Forms.Label lblColorUnknown;
        private System.Windows.Forms.CheckBox chkUseGlobalProviderTags;
        private System.Windows.Forms.TextBox txtAcceptLanguage;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chkEnableCompression;
        private System.Windows.Forms.CheckBox chkHideBlaclistedImage;
        private System.Windows.Forms.CheckBox chkProcessDeletedPost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
        private System.Windows.Forms.DataGridViewImageColumn colPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRating;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTags;
        private CustomControl.TagsColumn colTagsE;
        private System.Windows.Forms.DataGridViewLinkColumn colUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMD5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuery;
        private System.Windows.Forms.DataGridViewLinkColumn colSourceUrl;
        private System.Windows.Forms.DataGridViewLinkColumn colReferer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.TextBox txtBatchJobDelay;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.CheckBox chkDelayIncludeSkip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewImageColumn colPreview2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgress2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProvider2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRating2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTags2;
        private System.Windows.Forms.DataGridViewLinkColumn colUrl2;
        private System.Windows.Forms.DataGridViewLinkColumn colReferer2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMD52;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuery2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDownloadStart2;
        private System.Windows.Forms.CheckBox chkWriteDownloadedFileTxt;
        private System.Windows.Forms.CheckBox chkReplaceMode;
        private System.Windows.Forms.CheckBox chkIgnoreForGeneralTag;
        private System.Windows.Forms.CheckBox chkBlacklistOnlyGeneral;
        private System.Windows.Forms.TextBox txtNoFault;
        private System.Windows.Forms.TextBox txtNoCircle;
        private System.Windows.Forms.TextBox txtNoChara;
        private System.Windows.Forms.TextBox txtNoCopyright;
        private System.Windows.Forms.TextBox txtNoArtist;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
    }
}

