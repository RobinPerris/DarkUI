using DarkUI.Controls;
using DarkUI.Docking;

namespace Example
{
    public partial class DockProperties : DarkToolWindow
    {
        #region Constructor Region

        public DockProperties()
        {
            InitializeComponent();

            // Build dummy dropdown data
            cmbList.Items.Add(new DarkDropdownItem("Item1"));
            cmbList.Items.Add(new DarkDropdownItem("Item2"));
            cmbList.Items.Add(new DarkDropdownItem("Item3"));
            cmbList.Items.Add(new DarkDropdownItem("Item4"));
            cmbList.Items.Add(new DarkDropdownItem("Item5"));
            cmbList.Items.Add(new DarkDropdownItem("Item6"));

            cmbList.SelectedItemChanged += delegate { System.Console.WriteLine($"Item changed to {cmbList.SelectedItem.Text}"); };
        }

        #endregion
    }
}
