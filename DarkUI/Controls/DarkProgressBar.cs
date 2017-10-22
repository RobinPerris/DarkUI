using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        // Paint does not work for progressbars for some reason.
        // This is a workaround:
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F)
            {
                switch (_textMode)
                {
                    case DarkProgressBarMode.NoText:
                        break;

                    case DarkProgressBarMode.Percentage:
                        double percentage = ((Value - Minimum) * 100.0) / (Maximum - Minimum);
                        using (Graphics g = CreateGraphics())
                            g.DrawString(Math.Round(percentage) + "%", Font, Brushes.Black, ClientRectangle, _textAlignment);
                        break;

                    case DarkProgressBarMode.XOfN:
                        using (Graphics g = CreateGraphics())
                            g.DrawString(Value + "/" + (Maximum + 1), Font, Brushes.Black, ClientRectangle, _textAlignment);
                        break;

                    default:
                        throw new NotImplementedException("Text mode: " + _textMode);
                }
            }
        }
    }
}
