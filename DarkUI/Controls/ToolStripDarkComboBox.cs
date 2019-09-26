using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DarkUI.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripDarkComboBox : ToolStripSpringControlHost
    {
        // Call the base constructor passing in a DarkComboBox instance.
        public ToolStripDarkComboBox() : this(new DarkComboBox()) { }
        public ToolStripDarkComboBox(Control c) : base(c) { }

        public DarkComboBox ComboBox
        {
            get { return Control as DarkComboBox; }
        }

        // Declare the SelectedIndexChanged event.
        public event EventHandler SelectedIndexChanged;

        // Expose the DarkComboBox.SelectedIndex as a property.
        public int SelectedIndex
        {
            get { return ComboBox.SelectedIndex; }
            set { ComboBox.SelectedIndex = value; }
        }

        // Subscribe and unsubscribe the control events
        protected override void OnSubscribeControlEvents(Control c)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(c);

            // Cast the control to a DarkComboBox control.
            DarkComboBox combo = (DarkComboBox)c;

            // Add the event.
            combo.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);

            // Cast the control to a DarkComboBox control.
            DarkComboBox combo = (DarkComboBox)c;

            // Remove the event.
            combo.SelectedIndexChanged -= new EventHandler(OnSelectedIndexChanged);
        }

        // Raise the SelectedIndexChanged event.
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
        }
    }
}
