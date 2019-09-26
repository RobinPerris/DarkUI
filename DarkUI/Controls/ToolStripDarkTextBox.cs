using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DarkUI.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripDarkTextBox : ToolStripSpringControlHost
    {
        // Call the base constructor passing in a DarkComboBox instance.
        public ToolStripDarkTextBox() : this(new DarkTextBox()) { }
        public ToolStripDarkTextBox(Control c) : base(c) { }

        public DarkTextBox TextBox
        {
            get { return Control as DarkTextBox; }
        }
    }
}
