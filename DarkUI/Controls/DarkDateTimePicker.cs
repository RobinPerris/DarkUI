using System;
using System.Collections.Generic;
using System.Drawing;

using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
    public class DarkDateTimePicker : DateTimePicker
    {
        private bool mouseDown = false;
        private Point mousePos = new Point();
        private Rectangle scrollButtons;

        public DarkDateTimePicker()
        {
            this.ForeColor = Color.Gainsboro;
            this.BackColor = Color.FromArgb(69, 73, 74);
            this.CalendarForeColor = Color.Gainsboro;
            this.CalendarMonthBackground = Color.FromArgb(69, 73, 74);

            this.MouseMove += DarkDateTimePicker_MouseMove;
            this.MouseUp += DarkDateTimePicker_MouseUp;
            this.MouseDown += DarkDateTimePicker_MouseDown;
        }

        private void DarkDateTimePicker_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }

        private void DarkDateTimePicker_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void DarkDateTimePicker_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Point(e.Location.X - (this.Width - scrollButtons.Width), e.Location.Y);
        }

        private void DarkDateTimePicker_Paint(object sender, PaintEventArgs e)
        {
            if (this.ShowUpDown)
            {
                var rect = new Rectangle(0, 0, this.Width, this.Height);
                e.Graphics.FillRectangle(new SolidBrush(Colors.DarkBackground), new Rectangle(0, 0, rect.Width, rect.Height));

                var upIcon = ScrollIcons.scrollbar_arrow_standard;
                var upRect = new Rectangle(rect.Width / 2 - upIcon.Width / 2, rect.Height / 4 - upIcon.Height / 2, upIcon.Width, upIcon.Height);
                upIcon = upRect.Contains(mousePos) ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard;

                if (mouseDown && upRect.Contains(mousePos))
                    upIcon = ScrollIcons.scrollbar_arrow_clicked;

                upIcon.RotateFlip(RotateFlipType.RotateNoneFlipY);


                e.Graphics.DrawImageUnscaled(upIcon, upRect);

                // Down arrow
                var downIcon = ScrollIcons.scrollbar_arrow_standard;
                var downRect = new Rectangle(rect.Width / 2 - upIcon.Width / 2, rect.Height / 2 + rect.Height / 4 - upIcon.Height / 2, upIcon.Width, upIcon.Height);
                downIcon = downRect.Contains(mousePos) ? ScrollIcons.scrollbar_arrow_hot : ScrollIcons.scrollbar_arrow_standard;

                if (mouseDown && downRect.Contains(mousePos))
                    downIcon = ScrollIcons.scrollbar_arrow_clicked;

                e.Graphics.DrawImageUnscaled(downIcon, downRect);
                scrollButtons = rect;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.FromArgb(100, 100, 100), ButtonBorderStyle.Solid);
        }
    }
}