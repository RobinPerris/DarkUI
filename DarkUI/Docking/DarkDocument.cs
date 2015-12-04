using DarkUI.Config;
using System.ComponentModel;

namespace DarkUI.Docking
{
    [ToolboxItem(false)]
    public class DarkDocument : DarkDockContent
    {
        #region Property Region

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DarkDockArea DockArea
        {
            get { return base.DockArea; }
        }

        #endregion

        #region Constructor Region

        public DarkDocument()
        {
            BackColor = Colors.GreyBackground;
            base.DockArea = DarkDockArea.Document;
        }

        #endregion
    }
}
