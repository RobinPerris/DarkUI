using System.Windows.Forms;

namespace DarkUI
{
    public class DarkDockResizeFilter : IMessageFilter
    {
        #region Field Region

        private DarkDockPanel _dockPanel;

        #endregion

        #region Constructor Region

        public DarkDockResizeFilter(DarkDockPanel dockPanel)
        {
            _dockPanel = dockPanel;
        }

        #endregion

        #region IMessageFilter Region

        public bool PreFilterMessage(ref Message m)
        {
            return false;
        }

        #endregion
    }
}