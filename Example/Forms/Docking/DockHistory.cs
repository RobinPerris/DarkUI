using DarkUI;

namespace Example
{
    public partial class DockHistory : DarkToolWindow
    {
        #region Constructor Region

        public DockHistory()
        {
            InitializeComponent();

            // Build dummy list data
            for (var i = 0; i < 100; i++)
            {
                var item = new DarkListItem(string.Format("List item #{0}", i));
                lstHistory.Items.Add(item);
            }
        }

        #endregion
    }
}
