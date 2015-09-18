using System.ComponentModel;
using System.Windows.Forms;

namespace DarkUI
{
    [ToolboxItem(false)]
    public class DarkDockContent : UserControl
    {
        #region Event Region



        #endregion

        #region Field Region

        private string _dockText;

        #endregion

        #region Property Region

        public string DockText
        {
            get { return _dockText; }
            set
            {
                _dockText = value;
                Invalidate();
                // TODO: raise event for parent tabs
            }
        }

        #endregion
    }
}
