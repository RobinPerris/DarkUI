using DarkUI.Controls;
using DarkUI.Docking;

namespace Example
{
    public partial class DockLayers : DarkToolWindow
    {
        #region Constructor Region

        public DockLayers()
        {
            InitializeComponent();

            // Build dummy list data
            for (var i = 0; i < 100; i++)
            {
                var item = new DarkListItem($"List item #{i}");
                lstLayers.Items.Add(item);
            }
        }

        #endregion
    }
}
