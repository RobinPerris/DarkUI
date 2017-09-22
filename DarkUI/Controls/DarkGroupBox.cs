using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
    public class DarkGroupBox : GroupBox
    {
        private Color _borderColor = Colors.LightBorder;

        public DarkGroupBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.UserPaint, true);
            Paint += DarkGroupBox_Paint;
            this.ForeColor = Color.Gainsboro;
            this.BackColor = Colors.GreyBackground;
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
        }

        private void DarkGroupBox_Paint(object sender, PaintEventArgs e)
        {
            if (this.Parent != null)
                e.Graphics.Clear(this.Parent.BackColor);
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);
            Rectangle borderRect = this.ClientRectangle;
            borderRect.Y = (borderRect.Y + (tSize.Height / 2));
            borderRect.Height = (borderRect.Height - (tSize.Height / 2));
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), borderRect);
            ControlPaint.DrawBorder(e.Graphics, borderRect, this._borderColor, ButtonBorderStyle.Solid);
            Rectangle textRect = this.ClientRectangle;
            textRect.X = (textRect.X + 6);
            textRect.Y += borderRect.Top;
            textRect.Width = tSize.Width + 6;
            textRect.Height = tSize.Height - borderRect.Top;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            textRect = this.ClientRectangle;
            textRect.X = (textRect.X + 6);
            textRect.Width = tSize.Width + 6;
            textRect.Height = tSize.Height;
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);

        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate(); // causes control to be redrawn
            }
        }
    }
}
