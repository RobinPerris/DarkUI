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
    public enum DarkProgressBarMode
    {
        NoText,
        Percentage,
        XOfN
    }

    public class DarkProgressBar : ProgressBar
    {
        private static readonly StringFormat _textAlignment = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        private DarkProgressBarMode _textMode = DarkProgressBarMode.Percentage;

        [Category("Appearance")]
        [DefaultValue(DarkProgressBarMode.Percentage)]
        public DarkProgressBarMode TextMode
        {
            get { return _textMode; }
            set
            {
                if (value == _textMode)
                    return;
                _textMode = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Browsable(true)]
        public new Font Font { get { return base.Font; } set { base.Font = value; } }

        public DarkProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Prevent progress bar flickering
                return cp;
            }
        }

        // Paint does not work for progressbars for some reason.
        // This is a workaround:
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F)
            {
                float percentage = (float)(Value - Minimum) / (float)(Maximum - Minimum);

                using (Graphics g = CreateGraphics())
                {
                    var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

                    g.Clear(Colors.MediumBackground);

                    using (var b = new SolidBrush(Colors.LighterBackground))
                        g.FillRectangle(b, new Rectangle(rect.Left + 2, rect.Top + 2, (int)((rect.Width - 4) * percentage), rect.Height - 4));

                    using (var p = new Pen(Colors.GreySelection))
                        g.DrawRectangle(p, new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1));

                    if(_textMode != DarkProgressBarMode.NoText)
                    {
                        using (var b = new SolidBrush(Colors.LightText))
                        {
                            switch (_textMode)
                            {
                                case DarkProgressBarMode.Percentage:
                                    g.DrawString(float.IsNaN(percentage) ? "N/A" : Math.Round(percentage * 100) + "%", Font, b, ClientRectangle, _textAlignment);
                                    break;

                                case DarkProgressBarMode.XOfN:
                                    g.DrawString((Minimum == 0 ? Value + 1 : Value) + " / " + (Minimum == 0 ? Maximum + 1 : Maximum), Font, b, ClientRectangle, _textAlignment);
                                    break;

                                default:
                                    throw new NotImplementedException("Text mode: " + _textMode);
                            }
                        }
                    }
                }
            }
        }
    }
}
