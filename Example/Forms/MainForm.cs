using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Win32;
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
        private DockLayers _dockLayers;
        private DockHistory _dockHistory;

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
            _dockLayers = new DockLayers();
            _dockHistory = new DockHistory();

            DockPanel.AddContent(_dockProject);
            DockPanel.AddContent(_dockProperties);
            DockPanel.AddContent(_dockConsole);
            DockPanel.AddContent(_dockLayers);
            DockPanel.AddContent(_dockHistory, _dockLayers.DockGroup);

            // Add dummy documents to the main document area of the dock panel
            DockPanel.AddContent(new DockDocument("Document 1"));
            DockPanel.AddContent(new DockDocument("Document 2"));
            DockPanel.AddContent(new DockDocument("Document 3"));

            // Show the tool windows as visible in the 'Window' menu
            mnuProject.Checked = true;
            mnuProperties.Checked = true;
            mnuConsole.Checked = true;
            mnuLayers.Checked = true;
            mnuHistory.Checked = true;

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
            mnuLayers.Click += Layers_Click;
            mnuHistory.Click += History_Click;

            mnuAbout.Click += About_Click;
        }

        private void ToggleToolWindow(DarkToolWindow toolWindow, ToolStripMenuItem menuItem)
        {
            if (toolWindow.DockPanel == null)
            {
                DockPanel.AddContent(toolWindow);
                menuItem.Checked = true;
            }
            else
            {
                DockPanel.RemoveContent(toolWindow);
                menuItem.Checked = false;
            }
        }

        #endregion

        #region Event Handler Region

        private void NewFile_Click(object sender, EventArgs e)
        {
            var newFile = new DockDocument("New document");
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
            ToggleToolWindow(_dockProject, mnuProject);
        }

        private void Properties_Click(object sender, EventArgs e)
        {
            ToggleToolWindow(_dockProperties, mnuProperties);
        }

        private void Console_Click(object sender, EventArgs e)
        {
            ToggleToolWindow(_dockConsole, mnuConsole);
        }

        private void Layers_Click(object sender, EventArgs e)
        {
            ToggleToolWindow(_dockLayers, mnuLayers);
        }

        private void History_Click(object sender, EventArgs e)
        {
            ToggleToolWindow(_dockHistory, mnuHistory);
        }

        private void About_Click(object sender, EventArgs e)
        {
            var about = new DialogAbout();
            about.ShowDialog();
        }

        #endregion
    }
}
