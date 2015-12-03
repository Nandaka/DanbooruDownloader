using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DanbooruDownloader3.CustomControl
{
    /// <summary>
    /// http://www.codeproject.com/Articles/37087/DataGridView-that-Saves-Column-Order-Width-and-Vis
    /// </summary>
    [Description("Extension of the System.Windows.Forms.DataGridView")]
    [ToolboxBitmap(typeof(System.Windows.Forms.DataGridView))]
    public class gfDataGridView : System.Windows.Forms.DataGridView
    {
        private int maxWidth;

        [Description("MaxWidth for column"), Category("Data"), DefaultValue(300)]
        public int MaxWidth
        {
            get
            {
                return maxWidth;
            }
            set
            {
                if (value < 0) value = 300;
                this.maxWidth = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            SaveColumnOrder();
            base.Dispose(disposing);
        }

        private void SaveColumnOrder()
        {
            if (this.AllowUserToOrderColumns)
            {
                List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
                DataGridViewColumnCollection columns = this.Columns;
                for (int i = 0; i < columns.Count; i++)
                {
                    columnOrder.Add(new ColumnOrderItem
                    {
                        ColumnIndex = i,
                        DisplayIndex = columns[i].DisplayIndex,
                        Visible = columns[i].Visible,
                        Width = columns[i].Width
                    });
                }

                gfDataGridViewSetting.Default.ColumnOrder[this.Name] = columnOrder;
                gfDataGridViewSetting.Default.Save();
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetColumnOrder();
        }

        private void SetColumnOrder()
        {
            if (!gfDataGridViewSetting.Default.ColumnOrder.ContainsKey(this.Name))
                return;

            List<ColumnOrderItem> columnOrder = gfDataGridViewSetting.Default.ColumnOrder[this.Name];

            if (columnOrder != null)
            {
                var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
                foreach (var item in sorted)
                {
                    this.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                    this.Columns[item.ColumnIndex].Visible = item.Visible;
                    this.Columns[item.ColumnIndex].Width = item.Width > MaxWidth ? item.Width : MaxWidth;
                }
            }
        }

        public void ResetColumnOrder()
        {
            SetColumnOrder();
        }

        internal sealed class gfDataGridViewSetting : ApplicationSettingsBase
        {
            private static gfDataGridViewSetting _defaultInstace = (gfDataGridViewSetting)ApplicationSettingsBase.Synchronized(new gfDataGridViewSetting());

            //---------------------------------------------------------------------
            public static gfDataGridViewSetting Default
            {
                get { return _defaultInstace; }
            }

            //---------------------------------------------------------------------
            // Because there can be more than one DGV in the user-application
            // a dictionary is used to save the settings for this DGV.
            // As key the name of the control is used.
            [UserScopedSetting]
            [SettingsSerializeAs(SettingsSerializeAs.Binary)]
            [DefaultSettingValue("")]
            public Dictionary<string, List<ColumnOrderItem>> ColumnOrder
            {
                get
                {
                    return this["ColumnOrder"] as Dictionary<string, List<ColumnOrderItem>>;
                }
                set { this["ColumnOrder"] = value; }
            }
        }

        [Serializable]
        public sealed class ColumnOrderItem
        {
            public int DisplayIndex { get; set; }

            public int Width { get; set; }

            public bool Visible { get; set; }

            public int ColumnIndex { get; set; }
        }
    }
}