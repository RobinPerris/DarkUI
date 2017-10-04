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
        private Point _mousePos = new Point();
        private Rectangle _scrollButtons;

        public DarkNumericUpDown()
        {
            BackColor = Colors.LightBackground;
            ForeColor = Colors.LightText;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.UserPaint, true);
            Controls[0].Paint += DarkNumericUpDown_Paint;
            MouseMove += DarkNumericUpDown_MouseMove;
            MouseUp += DarkNumericUpDown_MouseUp;
            MouseDown += DarkNumericUpDown_MouseDown;
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

        private void DarkNumericUpDown_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
        }

        private void DarkNumericUpDown_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void DarkNumericUpDown_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePos = new Point(e.Location.X - (Width - _scrollButtons.Width),e.Location.Y);
        }

        private void DarkNumericUpDown_Paint(object sender, PaintEventArgs e)
        {
            // Up-down background
            var upDownRect = new Rectangle(0, 0, Controls[0].Width, Controls[0].Height);
            e.Graphics.FillRectangle(new SolidBrush(Colors.DarkBackground), upDownRect);

            // Up arrow
            var upIcon = ScrollIcons.scrollbar_arrow_standard;
            var upRect = new Rectangle(Controls[0].Size.Width / 2 - upIcon.Width / 2, Controls[0].Size.Height / 4 - upIcon.Height / 2,upIcon.Width, upIcon.Height);
            upIcon = upRect.Contains(_mousePos) ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard;

            if (_mouseDown && upRect.Contains(_mousePos))
                upIcon = ScrollIcons.scrollbar_arrow_clicked;

            upIcon.RotateFlip(RotateFlipType.RotateNoneFlipY);

            e.Graphics.DrawImage(upIcon,upRect);

            // Down arrow
            var downIcon = ScrollIcons.scrollbar_arrow_standard;
            var downRect = new Rectangle(Controls[0].Size.Width / 2 - upIcon.Width / 2, Controls[0].Size.Height / 2 + Controls[0].Size.Height / 4 - upIcon.Height / 2, upIcon.Width, upIcon.Height);
            downIcon = downRect.Contains(_mousePos) ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard;

            if (_mouseDown && downRect.Contains(_mousePos))
                downIcon = ScrollIcons.scrollbar_arrow_clicked;

            e.Graphics.DrawImage(downIcon, downRect);

            _scrollButtons = upDownRect;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(100,100,100), ButtonBorderStyle.Solid);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if(MousewheelSingleIncrement)
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
