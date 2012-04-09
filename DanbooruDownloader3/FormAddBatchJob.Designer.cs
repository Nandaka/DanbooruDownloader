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
            this.txtSave = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkNotRating = new System.Windows.Forms.CheckBox();
            this.cbxRating = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlProvider.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tag Query";
            // 
            // txtTagQuery
            // 
            this.txtTagQuery.Location = new System.Drawing.Point(82, 12);
            this.txtTagQuery.Name = "txtTagQuery";
            this.txtTagQuery.Size = new System.Drawing.Size(385, 20);
            this.txtTagQuery.TabIndex = 1;
            // 
            // txtLimit
            // 
            this.txtLimit.Location = new System.Drawing.Point(82, 38);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(75, 20);
            this.txtLimit.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
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
            this.pnlProvider.Controls.Add(this.btnSelectAll);
            this.pnlProvider.Location = new System.Drawing.Point(82, 64);
            this.pnlProvider.Name = "pnlProvider";
            this.pnlProvider.Size = new System.Drawing.Size(600, 90);
            this.pnlProvider.TabIndex = 4;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(3, 3);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(608, 190);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(527, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "provider";
            // 
            // txtSave
            // 
            this.txtSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSave.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DanbooruDownloader3.Properties.Settings.Default, "filenameFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtSave.Location = new System.Drawing.Point(83, 164);
            this.txtSave.Name = "txtSave";
            this.txtSave.Size = new System.Drawing.Size(600, 20);
            this.txtSave.TabIndex = 9;
            this.txtSave.Text = global::DanbooruDownloader3.Properties.Settings.Default.filenameFormat;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Save Folder";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(197, 38);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(75, 20);
            this.txtPage.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(163, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Page";
            // 
            // chkNotRating
            // 
            this.chkNotRating.AutoSize = true;
            this.chkNotRating.Location = new System.Drawing.Point(424, 40);
            this.chkNotRating.Name = "chkNotRating";
            this.chkNotRating.Size = new System.Drawing.Size(43, 17);
            this.chkNotRating.TabIndex = 19;
            this.chkNotRating.Text = "Not";
            this.chkNotRating.UseVisualStyleBackColor = true;
            // 
            // cbxRating
            // 
            this.cbxRating.FormattingEnabled = true;
            this.cbxRating.Location = new System.Drawing.Point(322, 38);
            this.cbxRating.Name = "cbxRating";
            this.cbxRating.Size = new System.Drawing.Size(96, 21);
            this.cbxRating.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(278, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Rating";
            // 
            // FormAddBatchJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 225);
            this.Controls.Add(this.chkNotRating);
            this.Controls.Add(this.cbxRating);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlProvider);
            this.Controls.Add(this.txtLimit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTagQuery);
            this.Controls.Add(this.label1);
            this.Name = "FormAddBatchJob";
            this.Text = "FormAddBatchJob";
            this.pnlProvider.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txtSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkNotRating;
        private System.Windows.Forms.ComboBox cbxRating;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSelectAll;
    }
}