using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace DanbooruDownloader3.CustomControl
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
                // Ensure that the cell used for the template is a TagsCell.
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
            this.DataGridView.SuspendLayout();
            if (value != null && value.GetType() == typeof(List<DanbooruDownloader3.Entity.DanbooruTag>))
            {
                List<DanbooruDownloader3.Entity.DanbooruTag> tags = value as List<DanbooruDownloader3.Entity.DanbooruTag>;

                // sort?
                tags = tags.OrderBy(x =>
                    x.Type == Entity.DanbooruTagType.Copyright ? 1 :
                    x.Type == Entity.DanbooruTagType.Character ? 2 :
                    x.Type == Entity.DanbooruTagType.Artist ? 3 :
                    x.Type == Entity.DanbooruTagType.Circle ? 4 :
                    x.Type == Entity.DanbooruTagType.Faults ? 5 :
                    x.Type == Entity.DanbooruTagType.General ? 6 : 7
                    ).ToList();

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
                    // check the total height for wrapping
                    var limitX = cellBounds.X + 1 + cellBounds.Width - 4;
                    var limitY = cellBounds.Y + 1 + cellBounds.Height - 4;
                    foreach (var item in tags)
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
                    if (this.DataGridView.AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
                    {
                        OwningRow.MinimumHeight = (int)(pad.Y + strSize.Height) - cellBounds.Y;
                    }

                    if (cellStyle.WrapMode == DataGridViewTriState.True)
                    {
                        OwningRow.MinimumHeight = OwningRow.MinimumHeight > pad.Y - cellBounds.Y ? OwningRow.MinimumHeight : (int)(pad.Y + strSize.Height) - cellBounds.Y;
                        pad.Y = cellBounds.Y + 1 + ((cellBounds.Y + cellBounds.Height) - (pad.Y + strSize.Height)) / 2;
                    } 
                    else if (pad.Y > limitY) pad.Y = cellBounds.Y + 1;
                    else pad.Y = cellBounds.Y + 1 + ((cellBounds.Y + cellBounds.Height) - (pad.Y + strSize.Height)) / 2;

                    // the actual draw
                    using (Brush GeneralBrush = new SolidBrush(Helper.ColorGeneral),
                              ArtistBrush = new SolidBrush(Helper.ColorArtist),
                              CharacterBrush = new SolidBrush(Helper.ColorCharacter),
                              CircleBrush = new SolidBrush(Helper.ColorCircle),
                              CopyrightBrush = new SolidBrush(Helper.ColorCopyright),
                              FaultsBrush = new SolidBrush(Helper.ColorFaults), 
                              UnknownBrush = new SolidBrush(Helper.ColorUnknown))
                    {
                        graphics.SetClip(cellBounds, System.Drawing.Drawing2D.CombineMode.Replace);
                        foreach (var item in tags)
                        {
                            var temp = item.Name + " ";
                            var brush = GeneralBrush;
                            switch (item.Type)
                            {
                                case Entity.DanbooruTagType.Artist: brush = ArtistBrush; break;
                                case Entity.DanbooruTagType.Character: brush = CharacterBrush; break;
                                case Entity.DanbooruTagType.Circle: brush = CircleBrush; break;
                                case Entity.DanbooruTagType.Copyright: brush = CopyrightBrush; break;
                                case Entity.DanbooruTagType.Faults: brush = FaultsBrush; break;
                                case Entity.DanbooruTagType.Unknown: brush = UnknownBrush; break;
                            }

                            strSize = graphics.MeasureString(temp, cellStyle.Font);
                            // check over X bound
                            if (pad.X + strSize.Width > limitX)
                            {
                                pad.X = cellBounds.X + 1;
                                pad.Y += strSize.Height;
                            }
                            // check over Y bound?
                            if (pad.Y > limitY) break;
                            graphics.DrawString(temp, cellStyle.Font, brush, pad);
                            if (pad.X + strSize.Width > limitX)
                            {
                                pad.X = cellBounds.X + 1;
                                pad.Y += strSize.Height;
                            }
                            else pad.X += strSize.Width;
                        }
                        graphics.ResetClip();
                        this.DataGridView.ResumeLayout();
                    }
                }
            }
        }

        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter formattedValueTypeConverter, System.ComponentModel.TypeConverter valueTypeConverter)
        {
            if (formattedValue.GetType() == typeof(String))
            {
                DanbooruDownloader3.Entity.DanbooruPost Post = this.OwningRow.DataBoundItem as DanbooruDownloader3.Entity.DanbooruPost;

                if (Post == null || !Post.Provider.HasPrivateTags || DanbooruDownloader3.Properties.Settings.Default.UseGlobalProviderTags)
                {
                    return DanbooruDownloader3.DAO.DanbooruTagsDao.Instance.ParseTagsString(formattedValue as string);
                }
                else
                {
                    return DanbooruDownloader3.DAO.DanbooruTagsDao.Instance.ParseTagsString(formattedValue as string, Post.Provider.ProviderTagCollection);
                }
            }
            else return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
        }

    }

}
