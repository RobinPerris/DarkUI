using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    internal class DarkTranslucentForm : Form
    {
        #region Property Region

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        #endregion

        #region Constructor Region

        public DarkTranslucentForm(Color backColor, double opacity = 0.6)
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            AllowTransparency = true;
            Opacity = opacity;
            BackColor = backColor;
        }

        #endregion
    }
}
