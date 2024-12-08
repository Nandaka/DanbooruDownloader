namespace DanbooruDownloader3
{
    partial class FormAddBatchJob
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtTagQuery = new System.Windows.Forms.TextBox();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlProvider = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkNotRating = new System.Windows.Forms.CheckBox();
            this.cbxRating = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.txtFilenameFormat = new System.Windows.Forms.TextBox();
            this.chkIsExclude = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tag Query";
            // 
            // txtTagQuery
            // 
            this.txtTagQuery.Location = new System.Drawing.Point(123, 18);
            this.txtTagQuery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTagQuery.Multiline = true;
            this.txtTagQuery.Name = "txtTagQuery";
            this.txtTagQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTagQuery.Size = new System.Drawing.Size(619, 95);
            this.txtTagQuery.TabIndex = 1;
            this.txtTagQuery.WordWrap = false;
            // 
            // txtLimit
            // 
            this.txtLimit.Location = new System.Drawing.Point(123, 125);
            this.txtLimit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(110, 26);
            this.txtLimit.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Limit";
            // 
            // pnlProvider
            // 
            this.pnlProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProvider.AutoScroll = true;
            this.pnlProvider.AutoSize = true;
            this.pnlProvider.Location = new System.Drawing.Point(123, 166);
            this.pnlProvider.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlProvider.Name = "pnlProvider";
            this.pnlProvider.Size = new System.Drawing.Size(900, 198);
            this.pnlProvider.TabIndex = 4;
            this.pnlProvider.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.pnlProvider_ControlAdded);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(18, 191);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(96, 108);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(912, 420);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 35);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(790, 420);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "provider";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 385);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Filename Format";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(339, 125);
            this.txtPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(110, 26);
            this.txtPage.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(244, 129);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Start Page";
            // 
            // chkNotRating
            // 
            this.chkNotRating.AutoSize = true;
            this.chkNotRating.Location = new System.Drawing.Point(680, 128);
            this.chkNotRating.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkNotRating.Name = "chkNotRating";
            this.chkNotRating.Size = new System.Drawing.Size(60, 24);
            this.chkNotRating.TabIndex = 19;
            this.chkNotRating.Text = "Not";
            this.chkNotRating.UseVisualStyleBackColor = true;
            // 
            // cbxRating
            // 
            this.cbxRating.FormattingEnabled = true;
            this.cbxRating.Location = new System.Drawing.Point(526, 125);
            this.cbxRating.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxRating.Name = "cbxRating";
            this.cbxRating.Size = new System.Drawing.Size(142, 28);
            this.cbxRating.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(460, 129);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "Rating";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 420);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "Filter Extension";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "filterExtensions", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFilter.Location = new System.Drawing.Point(154, 417);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(234, 26);
            this.txtFilter.TabIndex = 21;
            this.txtFilter.Text = global::DanbooruDownloader3.Properties.Settings.Default.filterExtensions;
            // 
            // txtFilenameFormat
            // 
            this.txtFilenameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilenameFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "filenameFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFilenameFormat.Location = new System.Drawing.Point(154, 380);
            this.txtFilenameFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFilenameFormat.Name = "txtFilenameFormat";
            this.txtFilenameFormat.Size = new System.Drawing.Size(870, 26);
            this.txtFilenameFormat.TabIndex = 9;
            this.txtFilenameFormat.Text = global::DanbooruDownloader3.Properties.Settings.Default.filenameFormat;
            // 
            // chkIsExclude
            // 
            this.chkIsExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIsExclude.AutoSize = true;
            this.chkIsExclude.Checked = global::DanbooruDownloader3.Properties.Settings.Default.ExtExcludeMode;
            this.chkIsExclude.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DanbooruDownloader3.Properties.Settings.Default, "ExtExcludeMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkIsExclude.Location = new System.Drawing.Point(394, 420);
            this.chkIsExclude.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkIsExclude.Name = "chkIsExclude";
            this.chkIsExclude.Size = new System.Drawing.Size(135, 24);
            this.chkIsExclude.TabIndex = 22;
            this.chkIsExclude.Text = "Exclude Mode";
            this.chkIsExclude.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(754, 22);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(224, 40);
            this.label8.TabIndex = 23;
            this.label8.Text = "Separate tags by space.\r\nSeparate each job by new line.";
            // 
            // FormAddBatchJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 474);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkIsExclude);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.chkNotRating);
            this.Controls.Add(this.cbxRating);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtFilenameFormat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlProvider);
            this.Controls.Add(this.txtLimit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTagQuery);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormAddBatchJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormAddBatchJob";
            this.Load += new System.EventHandler(this.FormAddBatchJob_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTagQuery;
        private System.Windows.Forms.TextBox txtLimit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel pnlProvider;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFilenameFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkNotRating;
        private System.Windows.Forms.ComboBox cbxRating;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkIsExclude;
        private System.Windows.Forms.Label label8;
    }
}