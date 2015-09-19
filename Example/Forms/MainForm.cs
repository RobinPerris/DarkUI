using DarkUI;
using System;
using System.Windows.Forms;

namespace Example
{
    public partial class MainForm : DarkForm
    {
        #region Field Region

        private DockProject _dockProject;
        private DockProperties _dockProperties;
        private DockConsole _dockConsole;

        #endregion

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

            // Build the tool windows and add them to the dock panel
            _dockProject = new DockProject();
            _dockProperties = new DockProperties();
            _dockConsole = new DockConsole();

            DockPanel.AddContent(_dockProject);
            DockPanel.AddContent(_dockProperties);
            DockPanel.AddContent(_dockConsole);

            // Show the tool windows as visible in the 'Window' menu
            mnuProject.Checked = true;
            mnuProperties.Checked = true;
            mnuConsole.Checked = true;

            // Hook in all the UI events manually for clarity.
            HookEvents();
        }

        #endregion

        #region Method Region

        private void HookEvents()
        {
            mnuNewFile.Click += NewFile_Click;
            mnuClose.Click += Close_Click;

            btnNewFile.Click += NewFile_Click;

            mnuDialog.Click += Dialog_Click;

            mnuProject.Click += Project_Click;
            mnuProperties.Click += Properties_Click;
            mnuConsole.Click += Console_Click;

            mnuAbout.Click += About_Click;
        }

        #endregion

        #region Event Handler Region

        private void NewFile_Click(object sender, EventArgs e)
        {
            var newFile = new DockDocument();
            DockPanel.AddContent(newFile);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Dialog_Click(object sender, EventArgs e)
        {
            var test = new DialogTest();
            test.ShowDialog();
        }

        private void Project_Click(object sender, EventArgs e)
        {
            if (_dockProject.DockPanel == null)
            {
                DockPanel.AddContent(_dockProject);
                mnuProject.Checked = true;
            }
            else
            {
                DockPanel.RemoveContent(_dockProject);
                mnuProject.Checked = false;
            }
        }

        private void Properties_Click(object sender, EventArgs e)
        {
            if (_dockProperties.DockPanel == null)
            {
                DockPanel.AddContent(_dockProperties);
                mnuProperties.Checked = true;
            }
            else
            {
                DockPanel.RemoveContent(_dockProperties);
                mnuProperties.Checked = false;
            }
        }

        private void Console_Click(object sender, EventArgs e)
        {
            if (_dockConsole.DockPanel == null)
            {
                DockPanel.AddContent(_dockConsole);
                mnuConsole.Checked = true;
            }
            else
            {
                DockPanel.RemoveContent(_dockConsole);
                mnuConsole.Checked = false;
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            var about = new DialogAbout();
            about.ShowDialog();
        }

        #endregion
    }
}
