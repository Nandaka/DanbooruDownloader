using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.Entity;
using System.Reflection;

namespace DanbooruDownloader3
{
    public partial class FormProvider : Form
    {
        public List<DanbooruProvider> Providers { get; set; }
        public int SelectedIndex { get; set; }
        private int lastSelectedIndex = -1;
        public bool IsModified { get; set; }

        public FormProvider()
        {
            InitializeComponent();
            CreateControls();
            IsModified = false;
        }

        private void FormProvider_Load(object sender, EventArgs e)
        {
            lastSelectedIndex = SelectedIndex;
            LoadProviders();
            cbxProviders.SelectedIndex = SelectedIndex;
        }

        private void LoadProviders()
        {
            cbxProviders.DataSource = null;
            cbxProviders.DataSource = Providers;
            cbxProviders.DisplayMember = "Name";            
        }

        private void CreateControls()
        {
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(DanbooruProvider).GetProperties();
            tableLayoutPanel1.RowCount = propertyInfos.Length;

            foreach (var info in propertyInfos)
            {
                Label lbl = new Label();
                lbl.Text = info.Name;
                tableLayoutPanel1.Controls.Add(lbl);

                if (info.PropertyType.Name == "Boolean")
                {
                    ComboBox _cbx = new ComboBox();
                    _cbx.Name = info.Name;
                    _cbx.Items.AddRange(new object[] { true, false });
                    
                    tableLayoutPanel1.Controls.Add(_cbx);
                }
                else if (info.PropertyType.IsEnum)
                {
                    ComboBox _cbx = new ComboBox();
                    _cbx.Name = info.Name;
                    foreach (var item in Enum.GetValues(info.PropertyType))
                    {
                        _cbx.Items.Add(item);
                    }
                    _cbx.SelectedIndexChanged += new EventHandler(_cbx_SelectedIndexChanged);
                    tableLayoutPanel1.Controls.Add(_cbx);
                }
                else
                {
                    TextBox _txt = new TextBox();
                    _txt.Name = info.Name;
                    _txt.Dock = DockStyle.Fill;
                    if (info.Name.ToLowerInvariant().Contains("password")) _txt.UseSystemPasswordChar = true;
                    tableLayoutPanel1.Controls.Add(_txt);
                }
            }
        }

        void _cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbx = (ComboBox)sender;
            switch (cbx.Name)
            {
                case "BoardType":
                    var xmlBox = tableLayoutPanel1.Controls.Find("QueryStringXml", true);
                    var jsonBox = tableLayoutPanel1.Controls.Find("QueryStringJson", true);
                    var item = cbx.SelectedItem;
                    if(item.Equals(BoardType.Danbooru)) 
                    {
                        xmlBox[0].Text = "/post/index.xml?%_query%";
                        jsonBox[0].Text = "/post/index.json?%_query%";
                    }
                    else if (item.Equals(BoardType.Gelbooru))
                    {
                        xmlBox[0].Text = "/index.php?page=dapi&amp;s=post&amp;q=index&amp;%_query%";
                        jsonBox[0].Text = "";
                    }
                    else if (item.Equals(BoardType.Shimmie2))
                    {
                        xmlBox[0].Text = "/index.php?q=/rss/images/%_query%";
                        jsonBox[0].Text = "";
                    }
                    break;
                default:
                    break;
            }
        }

        private void Fill(int index)
        {
            if (index == -1 || index >= Providers.Count ) return;
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(DanbooruProvider).GetProperties();

            foreach (var info in propertyInfos)
            {
                var controls = tableLayoutPanel1.Controls.Find(info.Name, true);
                if (controls.Length > 0)
                {
                    if (info.PropertyType.Name == "Boolean")
                    {
                        ComboBox _cbx = (ComboBox)controls[0];
                        string value = info.GetValue(Providers[index], null).ToString().ToLowerInvariant();
                        _cbx.SelectedIndex = value == "true" ? 0 : 1;
                    }
                    else if (info.PropertyType.IsEnum)
                    {
                        ComboBox _cbx = (ComboBox)controls[0];
                        _cbx.SelectedIndex = _cbx.FindStringExact(info.GetValue(Providers[index], null).ToString());
                    }
                    else
                    {
                        TextBox _txt = (TextBox)controls[0];
                        var value = info.GetValue(Providers[index], null);
                        if (value != null) _txt.Text = value.ToString();
                        else _txt.Text = "";
                    }
                }
            }
        }

        private void GetValues(int index)
        {
            if (index == -1 || index >= Providers.Count) return;
            DanbooruProvider temp = new DanbooruProvider();
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(DanbooruProvider).GetProperties();

            foreach (var info in propertyInfos)
            {
                try
                {
                    var controls = tableLayoutPanel1.Controls.Find(info.Name, true);
                    if (controls.Length > 0)
                    {
                        if (controls[0].GetType().Name == "TextBox")
                        {
                            var value = controls[0].Text;
                            info.SetValue(temp, Convert.ChangeType(value, info.PropertyType), null);
                        }
                        else
                        {
                            ComboBox c = (ComboBox)controls[0];
                            var value = c.SelectedItem;
                            info.SetValue(temp, value, null);
                        }
                    }
                }
                catch (Exception) { return;  }
            }

            Providers[index] = temp;
        }

        private void cbxProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProviders.SelectedIndex != -1)
            {
                if(cbxProviders.SelectedIndex!= lastSelectedIndex) GetValues(lastSelectedIndex);
                Fill(cbxProviders.SelectedIndex);
                lastSelectedIndex = cbxProviders.SelectedIndex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetValues(cbxProviders.SelectedIndex);
            LoadProviders();
            IsModified = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DanbooruProvider newP = new DanbooruProvider();
            newP.Url = "http://";
            Providers.Add(newP);
            LoadProviders();
            cbxProviders.SelectedIndex = cbxProviders.Items.Count - 1;            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedIndex = cbxProviders.SelectedIndex;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbxProviders.SelectedIndex != -1)
            {
                Providers.RemoveAt(cbxProviders.SelectedIndex);
                LoadProviders();
                cbxProviders.SelectedIndex = 0;
            }
        }

    }
}
