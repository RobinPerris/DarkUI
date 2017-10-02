using System;
using System.Windows.Forms;

namespace DarkUI.Forms
{
    public static class DarkMessageBox
    {
        [Obsolete("It is recommended to specify the \"owner\" of the message box. This way the message box can be opened on top of the currently active window.")]
        public static DialogResult Show(string message)
        {
            using (var form = new DarkDialogMessageBox() { Message = message })
                return form.ShowDialog();
        }
        public static DialogResult Show(IWin32Window owner, string message)
        {
            using (var form = new DarkDialogMessageBox() { Message = message })
                return form.ShowDialog(owner);
        }

        [Obsolete("It is recommended to specify the \"owner\" of the message box. This way the message box can be opened on top of the currently active window.")]
        public static DialogResult Show(string message, string caption)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption })
                return form.ShowDialog();
        }
        public static DialogResult Show(IWin32Window owner, string message, string caption)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption })
                return form.ShowDialog(owner);
        }

        [Obsolete("It is recommended to specify the \"owner\" of the message box. This way the message box can be opened on top of the currently active window.")]
        public static DialogResult Show(string message, string caption, MessageBoxIcon icon)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, Icon = icon })
                return form.ShowDialog();
        }
        public static DialogResult Show(IWin32Window owner, string message, string caption, MessageBoxIcon icon)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, Icon = icon })
                return form.ShowDialog(owner);
        }


        [Obsolete("It is recommended to specify the \"owner\" of the message box. This way the message box can be opened on top of the currently active window.")]
        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, DialogButtons = buttons })
                return form.ShowDialog();
        }
        public static DialogResult Show(IWin32Window owner, string message, string caption, MessageBoxButtons buttons)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, DialogButtons = buttons })
                return form.ShowDialog(owner);
        }

        [Obsolete("It is recommended to specify the \"owner\" of the message box. This way the message box can be opened on top of the currently active window.")]
        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, DialogButtons = buttons, Icon = icon })
                return form.ShowDialog();
        }
        public static DialogResult Show(IWin32Window owner, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, DialogButtons = buttons, Icon = icon })
                return form.ShowDialog(owner);
        }


        [Obsolete("It is recommended to specify the \"owner\" of the message box. This way the message box can be opened on top of the currently active window.")]
        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, DialogButtons = buttons, Icon = icon, DefaultButton = defaultButton })
                return form.ShowDialog();
        }
        public static DialogResult Show(IWin32Window owner, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            using (var form = new DarkDialogMessageBox() { Message = message, Text = caption, DialogButtons = buttons, Icon = icon, DefaultButton = defaultButton })
                return form.ShowDialog(owner);
        }
    }
}
