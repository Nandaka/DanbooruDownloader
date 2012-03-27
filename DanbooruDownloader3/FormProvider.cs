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

        public FormProvider()
        {
            InitializeComponent();
            CreateControls();
        }

        private void FormProvider_Load(object sender, EventArgs e)
        {
            lastSelectedIndex = SelectedIndex;
            cbxProviders.DataSource = Providers;
            cbxProviders.DisplayMember = "Name";
            cbxProviders.SelectedIndex = SelectedIndex;
            Fill(SelectedIndex);            
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
                    tableLayoutPanel1.Controls.Add(_cbx);
                }
                else
                {
                    TextBox _txt = new TextBox();
                    _txt.Name = info.Name;
                    _txt.Dock = DockStyle.Fill;

                    tableLayoutPanel1.Controls.Add(_txt);
                }
            }
        }

        private void Fill(int index)
        {
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
                        _txt.Text = info.GetValue(Providers[index], null).ToString();
                    }
                }
            }
        }

        private void GetValues(int index)
        {
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
            GetValues(lastSelectedIndex);
            Fill(cbxProviders.SelectedIndex);
            lastSelectedIndex = cbxProviders.SelectedIndex;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SelectedIndex = cbxProviders.SelectedIndex;
            GetValues(cbxProviders.SelectedIndex);
        }

    }
}
