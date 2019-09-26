using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class ToolStripSpringControlHost : ToolStripControlHost, IToolStripSpring
    {
        public bool Spring { get; set; }

        public ToolStripSpringControlHost(Control c)
            : base(c)
        {
        }

        public ToolStripSpringControlHost(Control c, string name)
            : base(c, name)
        {
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            var basePreferredSize = base.GetPreferredSize(constrainingSize);
            var preferredSize = ToolStripSpring.GetPreferredSize(constrainingSize, this, DefaultSize, basePreferredSize);
            return preferredSize;
        }
    }
}
