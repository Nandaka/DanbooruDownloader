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
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SiteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CookieNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CookieValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dgvCookie.Location = new System.Drawing.Point(12, 12);
            this.dgvCookie.Name = "dgvCookie";
            this.dgvCookie.Size = new System.Drawing.Size(606, 238);
            this.dgvCookie.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Domain";
            this.dataGridViewTextBoxColumn1.HeaderText = "Site";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Cookie Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn3.HeaderText = "Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // SiteColumn
            // 
            this.SiteColumn.DataPropertyName = "Domain";
            this.SiteColumn.HeaderText = "Site";
            this.SiteColumn.Name = "SiteColumn";
            this.SiteColumn.Width = 50;
            // 
            // CookieNameColumn
            // 
            this.CookieNameColumn.DataPropertyName = "Name";
            this.CookieNameColumn.HeaderText = "Cookie Name";
            this.CookieNameColumn.Name = "CookieNameColumn";
            this.CookieNameColumn.Width = 96;
            // 
            // CookieValueColumn
            // 
            this.CookieValueColumn.DataPropertyName = "Value";
            this.CookieValueColumn.HeaderText = "Value";
            this.CookieValueColumn.Name = "CookieValueColumn";
            this.CookieValueColumn.Width = 59;
            // 
            // FormCookie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 262);
            this.Controls.Add(this.dgvCookie);
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
    }
}