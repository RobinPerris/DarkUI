using System;
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
                item.Icon = Icons.application_16x;
                lstLayers.Items.Add(item);
            }

            // Build dropdown list data
            for (var i = 0; i < 5; i++)
            {
                cmbList.Items.Add(new DarkDropdownItem($"Dropdown item #{i}"));
            }
        }

        #endregion
    }
}
