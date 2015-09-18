using System.Windows.Forms;

namespace DarkUI
{
    public class DarkDockPanel : UserControl
    {
        #region Property Region

        public IMessageFilter MessageFilter { get; private set; }

        #endregion

        #region Constructor Region

        public DarkDockPanel()
        {
            MessageFilter = new DarkDockResizeFilter(this);

            BackColor = Colors.GreyBackground;
        }

        #endregion
    }
}
    