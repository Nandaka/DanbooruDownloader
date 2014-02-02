/**
 * Modified from http://www.codeproject.com/Articles/37087/DataGridView-that-Saves-Column-Order-Width-and-Vis
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace DanbooruDownloader3
{
    public static class DataGridViewExtensions
    {
        public static void SaveColumnOrder(this DataGridView grid)
        {
            List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
            DataGridViewColumnCollection columns = grid.Columns;
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

            gfDataGridViewSetting.Default.ColumnOrder[grid.Name] = columnOrder;
            gfDataGridViewSetting.Default.Save();
        }


        public static void SetColumnOrder(this DataGridView grid)
        {
            if (!gfDataGridViewSetting.Default.ColumnOrder.ContainsKey(grid.Name))
                return;

            List<ColumnOrderItem> columnOrder = gfDataGridViewSetting.Default.ColumnOrder[grid.Name];

            if (columnOrder != null)
            {
                var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
                foreach (var item in sorted)
                {
                    grid.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                    grid.Columns[item.ColumnIndex].Visible = item.Visible;
                    grid.Columns[item.ColumnIndex].Width = item.Width;
                }
            }
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

    internal sealed class gfDataGridViewSetting : ApplicationSettingsBase
    {
        private static gfDataGridViewSetting _defaultInstace =
            (gfDataGridViewSetting)ApplicationSettingsBase
            .Synchronized(new gfDataGridViewSetting());
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
                return this["ColumnOrder"] as Dictionary<string,
                         List<ColumnOrderItem>>;
            }
            set { this["ColumnOrder"] = value; }
        }
    }
}
