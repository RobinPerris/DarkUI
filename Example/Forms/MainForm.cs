using DarkUI;
using System.Windows.Forms;

namespace Example
{
    public partial class MainForm : DarkForm
    {
        #region Constructor Region

        public MainForm()
        {
            InitializeComponent();

            // Add the control scroll message filter to re-route all mousewheel events
            // to the control the user is currently hovering over with their cursor.
            Application.AddMessageFilter(new DarkControlScrollFilter());

            // Add the dock panel message filter to filter through for dock panel splitter
            // input before letting events pass through to the rest of the application.
            Application.AddMessageFilter(DockPanel.MessageFilter);

            // Hook in all the UI events manually for clarity.
            HookEvents();
        }

        #endregion

        #region Method Region

        private void HookEvents()
        {
            mnuDialog.Click += delegate
            {
                var test = new DialogTest();
                test.ShowDialog();
            };

            mnuAbout.Click += delegate
            {
                var about = new DialogAbout();
                about.ShowDialog();
            };
        }

        #endregion
    }
}
