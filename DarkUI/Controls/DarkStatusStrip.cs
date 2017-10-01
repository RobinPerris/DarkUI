using DarkUI.Config;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkStatusStrip : StatusStrip
    {
        #region Constructor Region

        public DarkStatusStrip()
        {
            AutoSize = false;
            BackColor = Colors.GreyBackground;
            ForeColor = Colors.LightText;
            Padding = new Padding(0, 5, 0, 3);
            Size = new Size(Size.Width, 24);
            SizingGrip = false;
        }

        #endregion

        #region Paint Region

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;

            using (var b = new SolidBrush(Colors.GreyBackground))
            {
                g.FillRectangle(b, ClientRectangle);
            }

            using (var p = new Pen(Colors.DarkBorder))
            {
                g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Right, 0);
            }

            using (var p = new Pen(Colors.LightBorder))
            {
                g.DrawLine(p, ClientRectangle.Left, 1, ClientRectangle.Right, 1);
            }
        }

        #endregion

        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [DefaultValue(false)]
        [IODescription("StatusStripSizingGripDescr")]
        public new bool SizingGrip
        {
            get { return base.SizingGrip; }
            set { base.SizingGrip = value; }
        }
    }
}
