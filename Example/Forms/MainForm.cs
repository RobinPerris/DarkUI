using DarkUI;

namespace Example
{
    public partial class MainForm : DarkForm
    {
        public MainForm()
        {
            InitializeComponent();

            btnDialog.Click += delegate {
                    var msgBox = new DarkMessageBox("This is small",
                        "Dark UI Example", DarkMessageBoxIcon.Information, DarkDialogButton.AbortRetryIgnore);
                    msgBox.ShowDialog();
                };

            btnMessageBox.Click += delegate {
                    var msgBox = new DarkMessageBox("This is a test of the dark message box. It's cool, isn't it? You can have really quite a lot of text in here and the message box will size itself appropriately. I dislike how the default .NET message box handled this, so hopefully this will be a better option for you. :)", 
                        "Dark UI Example", DarkMessageBoxIcon.Information, DarkDialogButton.AbortRetryIgnore);
                    msgBox.MaximumWidth = 350;
                    msgBox.ShowDialog();
                };
        }
    }
}
