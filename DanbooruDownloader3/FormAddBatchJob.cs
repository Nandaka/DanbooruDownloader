using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;

namespace DanbooruDownloader3
{
    public partial class FormAddBatchJob : Form
    {
        public DanbooruBatchJob Job { get; set; }

        private List<CheckBox> chkList;
        private List<DanbooruProvider> providerList;
        
        public FormAddBatchJob()
        {
            InitializeComponent();
            FillProvider();

            //Auto populate Rating
            cbxRating.DataSource = new BindingSource(Constants.Rating, null);
            cbxRating.DisplayMember = "Key";
            cbxRating.ValueMember = "Value";
            cbxRating.SelectedIndex = 0;
        }

        private void FillProvider()
        {
            DanbooruProviderDao dao = new DanbooruProviderDao();
            providerList = dao.GetAllProvider();

            chkList = new List<CheckBox>();
            foreach (DanbooruProvider p in providerList)
            {
                CheckBox chk = new CheckBox();
                chk.Text = p.Name;
                chk.AutoSize = true;
                chkList.Add(chk);
            }

            foreach (CheckBox c in chkList)
            {
                this.pnlProvider.Controls.Add(c);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Job = new DanbooruBatchJob();
            
            try
            {
                if(txtLimit.Text.Length > 0) Job.Limit = Convert.ToInt32(txtLimit.Text);
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error at Limit." + Environment.NewLine + ex.Message);
                txtLimit.Focus();
                txtLimit.SelectAll();
                return;
            }

            try
            {
                if (txtPage.Text.Length > 0) Job.Page = Convert.ToInt16(txtPage.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error at Page." + Environment.NewLine + ex.Message);
                txtPage.Focus();
                txtPage.SelectAll();
                return;
            }

            if (cbxRating.SelectedValue != null && chkNotRating.Checked) Job.Rating = "-" + cbxRating.SelectedValue;
            else Job.Rating = (string) cbxRating.SelectedValue;

            Job.TagQuery = txtTagQuery.Text.Replace(" ","_");

            if (txtSave.Text.Length == 0)
            {
                MessageBox.Show("Save destination is empty!");
                txtSave.Focus();
                return;
            }
            Job.SaveFolder = txtSave.Text;

            Job.ProviderList = new List<DanbooruProvider>();
            bool providerFlag = false;
            foreach (CheckBox c in chkList)
            {
                if (c.Checked)
                {
                    foreach (DanbooruProvider p in providerList)
                    {
                        if(c.Text.Equals(p.Name))
                        {
                            Job.ProviderList.Add(p);
                            providerFlag = true;
                            break;
                        }
                    }
                }
            }
            if (!providerFlag)
            {
                MessageBox.Show("Please select at least 1 provider.");
                pnlProvider.Focus();
                this.DialogResult = DialogResult.None;
                //this.Job = null;
                //this.btnCancel_Click(sender, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Job = null;
            //this.Close();
            this.Hide();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox chk in chkList)
            {
                chk.Checked = chk.Checked == true ? false : true;
            }
        }
    }
}
