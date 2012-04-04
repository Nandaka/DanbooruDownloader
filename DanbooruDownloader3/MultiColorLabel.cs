using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace DanbooruDownloader3
{
    public class TagsColumn : DataGridViewColumn
    {
        public TagsColumn()
            : base(new TagsCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(TagsCell)))
                {
                    throw new InvalidCastException("Must be a TagsCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class TagsCell : DataGridViewTextBoxCell 
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            TextBox ctl = DataGridView.EditingControl as TextBox;
            // Use the default row value when Value property is null.
            if (this.Value!= null)
            {
                var tags = String.Join(" ", ((List<DanbooruDownloader3.Entity.DanbooruTag>)Value).Select(x => x.Name));
                ctl.Text = tags;
            }
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            if (value != null && value.GetType() == typeof(List<DanbooruDownloader3.Entity.DanbooruTag>))
            {
                //Rectangle newRect = new Rectangle(cellBounds.X + 1, cellBounds.Y + 1, cellBounds.Width - 4, cellBounds.Height - 4);
                
                using (Brush backColorBrush = new SolidBrush(cellStyle.BackColor), selectedBrush = new SolidBrush(cellStyle.SelectionBackColor))
                {
                    if (Selected) graphics.FillRectangle(selectedBrush, cellBounds);
                    else graphics.FillRectangle(backColorBrush, cellBounds);

                    // Draw the grid lines (only the right and bottom lines;
                    // DataGridView takes care of the others).
                    using (Pen gridLinePen = new Pen(DataGridView.GridColor))
                    {
                        graphics.DrawLine(gridLinePen, cellBounds.Left,
                            cellBounds.Bottom - 1, cellBounds.Right - 1,
                            cellBounds.Bottom - 1);
                        graphics.DrawLine(gridLinePen, cellBounds.Right - 1,
                            cellBounds.Top, cellBounds.Right - 1,
                            cellBounds.Bottom);
                    }

                    PointF pad = new PointF(cellBounds.X + 1, cellBounds.Y + 1);

                    SizeF strSize = new SizeF();
                    // check the total height
                    var limitX = cellBounds.X + 1 + cellBounds.Width - 4;
                    var limitY = cellBounds.Y + 1 + cellBounds.Height - 4;
                    foreach (var item in (List<DanbooruDownloader3.Entity.DanbooruTag>)value)
                    {
                        var temp = item.Name + " ";
                        strSize = graphics.MeasureString(temp, cellStyle.Font);
                        if (pad.X + strSize.Width > limitX)
                        {
                            pad.X = cellBounds.X + 1;
                            pad.Y += strSize.Height;
                        }
                        if (pad.X + strSize.Width > limitX)
                        {
                            pad.X = cellBounds.X + 1;
                            pad.Y += strSize.Height;
                        }
                        else pad.X += strSize.Width;
                    }
                    pad.X = cellBounds.X + 1;
                    if (pad.Y > limitY) pad.Y = cellBounds.Y + 1;
                    else pad.Y = cellBounds.Y + 1 + ((cellBounds.Y + cellBounds.Height) - (pad.Y + strSize.Height)) / 2;

                    // the actual draw
                    foreach (var item in (List<DanbooruDownloader3.Entity.DanbooruTag>)value)
                    {
                        var temp = item.Name + " ";
                        var brush = Brushes.Black;
                        switch (item.Type)
                        {
                            case Entity.DanbooruTagType.Artist: brush = Brushes.HotPink; break;
                            case Entity.DanbooruTagType.Character: brush = Brushes.Blue; break;
                            case Entity.DanbooruTagType.Circle: brush = Brushes.Purple; break;
                            case Entity.DanbooruTagType.Copyright: brush = Brushes.OrangeRed; break;
                            case Entity.DanbooruTagType.Faults: brush = Brushes.Red; break;
                        }

                        strSize = graphics.MeasureString(temp, cellStyle.Font);
                        // check over X bound
                        if (pad.X + strSize.Width > limitX)
                        {
                            pad.X = cellBounds.X + 1;
                            pad.Y += strSize.Height;
                        }
                        // check over Y bound?
                        if (pad.Y + strSize.Height > limitY) break;
                        graphics.DrawString(temp, cellStyle.Font, brush, pad);
                        if (pad.X + strSize.Width > limitX)
                        {
                            pad.X = cellBounds.X + 1;
                            pad.Y += strSize.Height;
                        }
                        else pad.X += strSize.Width;
                    }
                }
            }
        }
    }

}
