using DarkUI;

namespace Example
{
    public partial class MainForm : DarkForm
    {
        public MainForm()
        {
            InitializeComponent();

            for (var i = 0; i < 100; i++)
            {
                var item = new DarkListItem(string.Format("List item {0}", i));
                darkListView1.Items.Add(item);
            }

            btnDialog.Click += delegate
            {
                DarkMessageBox.ShowError("This is an error", "Dark UI - Example");
            };

            btnMessageBox.Click += delegate
            {
                DarkMessageBox.ShowInformation("This is some information, except it is much bigger, so there we go. I wonder how this is going to go. I hope it resizes properly. It probably will.", "Dark UI - Example");
            };
        }
    }
}
