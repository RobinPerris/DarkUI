using DarkUI;
using System.Windows.Forms;

namespace Example
{
    public partial class DialogAbout : DarkDialog
    {
        #region Constructor Region

        public DialogAbout()
        {
            InitializeComponent();

            lblVersion.Text = string.Format("Version: {0}", Application.ProductVersion.ToString());
            btnOk.Text = "Close";
        }

        #endregion
    }
}
