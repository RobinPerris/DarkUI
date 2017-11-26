using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
    public sealed class DarkNumericUpDown : NumericUpDown
    {
        [Category("Data")]
        [Description("Determines increment value used with shift modifier.")]
        public decimal IncrementAlternate { get; set; } = 1.0M;

        [Category("Behavior")]
        [Description("Forces mousewheel to scroll by one increment.")]
        public bool MousewheelSingleIncrement { get; set; } = true;

        private bool _mouseDown = false;
        private Point? _mousePos;

        public DarkNumericUpDown()
        {
            BackColor = Colors.LightBackground;
            ForeColor = Colors.LightText;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.UserPaint, true);
            Controls[0].Paint += SubControlPaint_Paint;
            try
            {
                // Prevent flickering, only if our assembly
                // has reflection permission.
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
                // Don't do anything, we are running in a trusted contex.
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            _mouseDown = true;
            Controls[0].Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _mouseDown = false;
            Controls[0].Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _mousePos = new Point(e.Location.X - (Width - Controls[0].Width), e.Location.Y);
            Controls[0].Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _mousePos = null;
            _mouseDown = false;
            Controls[0].Invalidate();
        }

        private void SubControlPaint_Paint(object sender, PaintEventArgs e)
        {
            var upDownRect = new Rectangle(0, 0, Controls[0].Width, Controls[0].Height);

            // Up arrow
            Bitmap flippedIcon = Icons.NumericUpDownIcons.numericUpDown_arrow;
            flippedIcon.RotateFlip(RotateFlipType.RotateNoneFlipY);
            RenderArrow(flippedIcon, new Rectangle(upDownRect.X, upDownRect.Y, upDownRect.Width, upDownRect.Height / 2), e);

            // Down arrow
            RenderArrow(Icons.NumericUpDownIcons.numericUpDown_arrow,
                new Rectangle(upDownRect.X, upDownRect.Y + upDownRect.Height / 2, upDownRect.Width, upDownRect.Height - (upDownRect.Height / 2)), e);
        }

        private void RenderArrow(Image image, Rectangle area, PaintEventArgs e)
        {
            Color backColor;
            if (!Enabled)
                backColor = Colors.DarkGreySelection;
            else if (!_mousePos.HasValue || !area.Contains(_mousePos.Value))
                backColor = Colors.LightBackground;
            else if (!_mouseDown)
                backColor = Colors.LighterBackground;
            else
                backColor = Colors.LightestBackground;

            using (Brush brush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(brush, area);
            e.Graphics.DrawImage(image, new Point(area.X + (area.Width - image.Width) / 2, area.Y + (area.Height - image.Height) / 2));
            ControlPaint.DrawBorder(e.Graphics, area, Colors.GreySelection, ButtonBorderStyle.Solid);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(100, 100, 100), ButtonBorderStyle.Solid);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (MousewheelSingleIncrement)
            {
                decimal newValue = Value;

                if (e.Delta > 0)
                    newValue += (ModifierKeys == Keys.Shift) ? IncrementAlternate : Increment;
                else
                    newValue -= (ModifierKeys == Keys.Shift) ? IncrementAlternate : Increment;

                Value = Math.Min(Maximum, Math.Max(Minimum, newValue));
            }
            else
                base.OnMouseWheel(e);
        }

        public override void UpButton()
        {
            if (ModifierKeys.HasFlag(Keys.Shift))
                Value = Math.Min(Maximum, Value + IncrementAlternate);
            else
                base.UpButton();
        }

        public override void DownButton()
        {
            if (ModifierKeys.HasFlag(Keys.Shift))
                Value = Math.Max(Minimum, Value - IncrementAlternate);
            else
                base.DownButton();
        }
    }
}
