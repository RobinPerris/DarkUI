using DarkUI.Config;
using DarkUI.Icons;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Extensions;

namespace DarkUI.Controls
{
    public class DarkComboBox : ComboBox
    {
        public DarkComboBox() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            DrawMode = DrawMode.OwnerDrawVariable;

            base.FlatStyle = FlatStyle.Flat;
            base.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBoxStyle DropDownStyle { get; set; }

        public new void Invalidate()
        {
            base.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Invalidate();

            base.OnTextChanged(e);
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            Invalidate();

            base.OnTextUpdate(e);
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            Invalidate();

            base.OnSelectedValueChanged(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var g = e.Graphics;
            var rect = e.Bounds;

            var textColor = Colors.LightText;
            var fillColor = Colors.LightBackground;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected || 
                (e.State & DrawItemState.Focus) == DrawItemState.Focus || 
                (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
                fillColor = Colors.BlueSelection;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                var text = Items[e.Index].ToString();
                    
                using (var b = new SolidBrush(textColor))
                {
                    var padding = 2;

                    var modRect = new Rectangle(rect.Left + padding,
                        rect.Top + padding,
                        rect.Width - (padding * 2),
                        rect.Height - (padding * 2));

                    var stringFormat = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    g.DrawString(text, Font, b, modRect, stringFormat);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            var textColor = Colors.LightText;
            var borderColor = Colors.GreySelection;
            var fillColor = Colors.LightBackground;

            if (Focused && TabStop)
                borderColor = Colors.BlueHighlight;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            using (var p = new Pen(borderColor, 1))
            {
                var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                g.DrawRectangle(p, modRect);
            }

            var icon = ScrollIcons.scrollbar_arrow_hot;
            g.DrawImageUnscaled(icon,
                                rect.Right - icon.Width - (Consts.Padding / 2),
                                (rect.Height / 2) - (icon.Height / 2));

            var text = SelectedItem != null ? SelectedItem.ToString() : Text;

            using (var b = new SolidBrush(textColor))
            {
                var padding = 2;

                var modRect = new Rectangle(rect.Left + padding,
                                            rect.Top + padding, 
                                            rect.Width - icon.Width - (Consts.Padding / 2) - (padding * 2),
                                            rect.Height - (padding * 2));

                var stringFormat = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    FormatFlags = StringFormatFlags.NoWrap,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(text, Font, b, modRect, stringFormat);
            }
        }
    }
}
