namespace DanbooruDownloader3
{
    partial class FormCookie
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
            this.dgvCookie = new System.Windows.Forms.DataGridView();
            this.SiteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CookieNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CookieValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClearCookie = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCookie)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCookie
            // 
            this.dgvCookie.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCookie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvCookie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCookie.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SiteColumn,
            this.CookieNameColumn,
            this.CookieValueColumn});
            this.dgvCookie.Location = new System.Drawing.Point(18, 18);
            this.dgvCookie.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvCookie.Name = "dgvCookie";
            this.dgvCookie.RowHeadersWidth = 62;
            this.dgvCookie.Size = new System.Drawing.Size(909, 389);
            this.dgvCookie.TabIndex = 0;
            // 
            // SiteColumn
            // 
            this.SiteColumn.DataPropertyName = "Domain";
            this.SiteColumn.HeaderText = "Site";
            this.SiteColumn.MinimumWidth = 8;
            this.SiteColumn.Name = "SiteColumn";
            this.SiteColumn.Width = 73;
            // 
            // CookieNameColumn
            // 
            this.CookieNameColumn.DataPropertyName = "Name";
            this.CookieNameColumn.HeaderText = "Cookie Name";
            this.CookieNameColumn.MinimumWidth = 8;
            this.CookieNameColumn.Name = "CookieNameColumn";
            this.CookieNameColumn.Width = 140;
            // 
            // CookieValueColumn
            // 
            this.CookieValueColumn.DataPropertyName = "Value";
            this.CookieValueColumn.HeaderText = "Value";
            this.CookieValueColumn.MinimumWidth = 8;
            this.CookieValueColumn.Name = "CookieValueColumn";
            this.CookieValueColumn.Width = 86;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Domain";
            this.dataGridViewTextBoxColumn1.HeaderText = "Site";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Cookie Name";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn3.HeaderText = "Value";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // btnClearCookie
            // 
            this.btnClearCookie.Location = new System.Drawing.Point(766, 415);
            this.btnClearCookie.Name = "btnClearCookie";
            this.btnClearCookie.Size = new System.Drawing.Size(161, 37);
            this.btnClearCookie.TabIndex = 1;
            this.btnClearCookie.Text = "Clear Cookie";
            this.btnClearCookie.UseVisualStyleBackColor = true;
            this.btnClearCookie.Click += new System.EventHandler(this.btnClearCookie_Click);
            // 
            // FormCookie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 464);
            this.Controls.Add(this.btnClearCookie);
            this.Controls.Add(this.dgvCookie);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormCookie";
            this.Text = "Cookie Viewer";
            this.Load += new System.EventHandler(this.FormCookie_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCookie)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCookie;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn SiteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CookieNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CookieValueColumn;
        private System.Windows.Forms.Button btnClearCookie;
    }
}