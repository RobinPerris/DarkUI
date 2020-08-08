using DarkUI.Config;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkNumericUpDown : NumericUpDown
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor { get; set; }

        private bool _mouseDown;

        public DarkNumericUpDown()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.UserPaint, true);

            base.ForeColor = Color.Gainsboro;
            base.BackColor = ThemeProvider.Theme.Colors.LightBackground;
            
            Controls[0].Paint += DarkNumericUpDown_Paint;

            try
            {
                // Prevent flickering, only if our assembly has reflection permission
                Type type = Controls[0].GetType();
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo method = type.GetMethod("SetStyle", flags);

                if (method != null)
                {
                    object[] param = { ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true };
                    method.Invoke(Controls[0], param);
                }
            }
            catch (SecurityException)
            {
                // Don't do anything, we are running in a trusted contex
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _mouseDown = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _mouseDown = false;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnTextBoxLostFocus(object source, EventArgs e)
        {
            base.OnTextBoxLostFocus(source, e);
            Invalidate();
        }

        private void DarkNumericUpDown_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = e.ClipRectangle;

            var fillColor = ThemeProvider.Theme.Colors.HeaderBackground;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            var mousePos = Controls[0].PointToClient(Cursor.Position);

            var upArea = new Rectangle(0, 0, rect.Width, rect.Height / 2);
            var upHot = upArea.Contains(mousePos);

            var upIcon = upHot ? ScrollIcons.scrollbar_arrow_small_hot : ScrollIcons.scrollbar_arrow_small_standard;
            if (upHot && _mouseDown)
                upIcon = ScrollIcons.scrollbar_arrow_small_clicked;

            upIcon.RotateFlip(RotateFlipType.RotateNoneFlipY);
            g.DrawImageUnscaled(upIcon, (upArea.Width / 2) - (upIcon.Width / 2), (upArea.Height / 2) - (upIcon.Height / 2));

            var downArea = new Rectangle(0, rect.Height / 2, rect.Width, rect.Height / 2);
            var downHot = downArea.Contains(mousePos);

            var downIcon = downHot ? ScrollIcons.scrollbar_arrow_small_hot : ScrollIcons.scrollbar_arrow_small_standard;
            if (downHot && _mouseDown)
                downIcon = ScrollIcons.scrollbar_arrow_small_clicked;

            g.DrawImageUnscaled(downIcon, (downArea.Width / 2) - (downIcon.Width / 2), downArea.Top + (downArea.Height / 2) - (downIcon.Height / 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            var borderColor = ThemeProvider.Theme.Colors.GreySelection;

            if (Focused && TabStop)
                borderColor = ThemeProvider.Theme.Colors.BlueHighlight;

            using (var p = new Pen(borderColor, 1))
            {
                var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                g.DrawRectangle(p, modRect);
            }
        }
    }
}
