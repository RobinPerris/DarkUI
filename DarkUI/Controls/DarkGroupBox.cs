using System.ComponentModel;
using System.Drawing;
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
            ForeColor = Colors.LightText;
            BackColor = Colors.GreyBackground;
            ResizeRedraw = true;
            DoubleBuffered = true;
        }

        private void DarkGroupBox_Paint(object sender, PaintEventArgs e)
        {
            if (Parent != null)
                e.Graphics.Clear(Parent.BackColor);
            Size tSize = TextRenderer.MeasureText(Text, Font);
            Rectangle borderRect = ClientRectangle;
            borderRect.Y = borderRect.Y + tSize.Height / 2 + 1;
            borderRect.Height = borderRect.Height - tSize.Height / 2 - 1;
            e.Graphics.FillRectangle(new SolidBrush(BackColor), borderRect);
            ControlPaint.DrawBorder(e.Graphics, borderRect, _borderColor, ButtonBorderStyle.Solid);
            Rectangle textRect = ClientRectangle;
            textRect.X = textRect.X + 6;
            textRect.Y += borderRect.Top;
            textRect.Width = tSize.Width + 2;
            textRect.Height = tSize.Height - borderRect.Top;
            e.Graphics.FillRectangle(new SolidBrush(BackColor), textRect);
            textRect = ClientRectangle;
            textRect.X = textRect.X + 8;
            textRect.Width = tSize.Width + 6;
            textRect.Height = tSize.Height + 1;
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textRect);

        }

        [Category("Appearance")]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate(); // causes control to be redrawn
            }
        }

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }
    }
}
