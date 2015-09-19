using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    [ToolboxItem(false)]
    public class DarkToolWindow : DarkDockContent
    {
        #region Property Region

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Padding Padding
        {
            get { return base.Padding; }
        }

        #endregion

        #region Constructor Region

        public DarkToolWindow()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            BackColor = Colors.GreyBackground;
            base.Padding = new Padding(0, Consts.ToolWindowHeaderSize, 0, 0);
        }

        #endregion

        #region Paint Region

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            // Fill body
            using (var b = new SolidBrush(Colors.GreyBackground))
            {
                g.FillRectangle(b, ClientRectangle);
            }

            // Draw header
            var bgColor = IsActive ? Colors.BlueBackground : Colors.HeaderBackground;
            var darkColor = IsActive ? Colors.DarkBlueBorder : Colors.DarkBorder;
            var lightColor = IsActive ? Colors.LightBlueBorder : Colors.LightBorder;

            using (var b = new SolidBrush(bgColor))
            {
                var bgRect = new Rectangle(0, 0, ClientRectangle.Width, Consts.ToolWindowHeaderSize);
                g.FillRectangle(b, bgRect);
            }

            using (var p = new Pen(darkColor))
            {
                g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Right, 0);
                g.DrawLine(p, ClientRectangle.Left, Consts.ToolWindowHeaderSize - 1, ClientRectangle.Right, Consts.ToolWindowHeaderSize - 1);
            }

            using (var p = new Pen(lightColor))
            {
                g.DrawLine(p, ClientRectangle.Left, 1, ClientRectangle.Right, 1);
            }

            var xOffset = 2;

            if (Icon != null)
            {
                g.DrawImageUnscaled(Icon, ClientRectangle.Left + 5, ClientRectangle.Top + (Consts.ToolWindowHeaderSize / 2) - (Icon.Height / 2) + 1);
                xOffset = Icon.Width + 8;
            }

            using (var b = new SolidBrush(Colors.LightText))
            {
                var textRect = new Rectangle(xOffset, 0, ClientRectangle.Width - 4 - xOffset, Consts.ToolWindowHeaderSize);

                var format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    FormatFlags = StringFormatFlags.NoWrap,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(DockText, Font, b, textRect, format);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Absorb event
        }

        #endregion
    }
}
