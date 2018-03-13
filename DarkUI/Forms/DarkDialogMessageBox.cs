using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Icons;

namespace DarkUI.Forms
{
    internal partial class DarkDialogMessageBox : DarkDialog
    {
        private const int MaximumWidth = 700;

        internal string Message { get; set; }

        internal DarkDialogMessageBox()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CalculateSize();
        }

        internal new MessageBoxIcon Icon
        {
            set
            {
                switch (value)
                {
                    case MessageBoxIcon.None:
                        picIcon.Visible = false;
                        lblText.Left = 10;
                        break;
                    case MessageBoxIcon.Question:
                        picIcon.Image = MessageBoxIcons.question;
                        break;
                    case MessageBoxIcon.Information:
                        picIcon.Image = MessageBoxIcons.info;
                        break;
                    case MessageBoxIcon.Warning:
                        picIcon.Image = MessageBoxIcons.warning;
                        break;
                    case MessageBoxIcon.Error:
                        picIcon.Image = MessageBoxIcons.error;
                        break;
                    default:
                        throw new NotImplementedException("MessageBoxIcon " + value + " unavailable.");
                }
            }
        }

        private void CalculateSize()
        {
            var width = 260;
            var height = 124;

            // Reset form back to original size
            Size = new Size(width, height);

            lblText.Text = string.Empty;
            lblText.AutoSize = true;
            lblText.Text = Message;

            // Set the minimum dialog size to whichever is bigger - the original size or the buttons.
            var minWidth = Math.Max(width, TotalButtonSize + 15);

            // Calculate the total size of the message
            var totalWidth = lblText.Right + 25;

            // Make sure we're not making the dialog bigger than the maximum size
            if (totalWidth < MaximumWidth)
            {
                // Width is smaller than the maximum width.
                // This means we can have a single-line message box.
                // Move the label to accomodate this.
                width = totalWidth;
                lblText.Top = picIcon.Top + picIcon.Height / 2 - lblText.Height / 2;
            }
            else
            {
                // Width is larger than the maximum width.
                // Change the label size and wrap it.
                width = MaximumWidth;
                var offsetHeight = Height - picIcon.Height;
                lblText.AutoUpdateHeight = true;
                lblText.Width = width - lblText.Left - 25;
                height = offsetHeight + lblText.Height;
            }

            // Force the width to the minimum width
            if (width < minWidth)
                width = minWidth;

            // Set the new size of the dialog
            Size = new Size(width, height);
        }
    }
}
