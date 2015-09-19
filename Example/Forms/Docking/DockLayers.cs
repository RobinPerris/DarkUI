using DarkUI;

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
                var item = new DarkListItem(string.Format("List item #{0}", i));
                lstLayers.Items.Add(item);
            }
        }

        #endregion
    }
}
