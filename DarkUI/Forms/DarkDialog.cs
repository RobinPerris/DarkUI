using DarkUI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DarkUI.Forms
{
    public partial class DarkDialog : DarkForm
    {
        private MessageBoxButtons _dialogButtons = MessageBoxButtons.OK;

        private IEnumerable<DarkButton> Buttons => new[] { btnYes, btnNo, btnOk, btnAbort, btnRetry, btnIgnore, btnCancel };

        [Description("Determines the type of the dialog window.")]
        [DefaultValue(MessageBoxButtons.OK)]
        public MessageBoxButtons DialogButtons
        {
            get { return _dialogButtons; }
            set
            {
                if (_dialogButtons == value)
                    return;

                _dialogButtons = value;
                UpdateButtons();
            }
        }

        [Description("Determines the type of the dialog window.")]
        [DefaultValue(MessageBoxDefaultButton.Button1)]
        public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Button1;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TotalButtonSize { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IButtonControl AcceptButton
        {
            get { return base.AcceptButton; }
            private set { base.AcceptButton = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IButtonControl CancelButton
        {
            get { return base.CancelButton; }
            private set { base.CancelButton = value; }
        }

        public DarkDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateButtons();

            // Determine default button index
            int defaultButtonIndex = 0;
            switch (DefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    defaultButtonIndex = 0;
                    break;
                case MessageBoxDefaultButton.Button2:
                    defaultButtonIndex = 1;
                    break;
                case MessageBoxDefaultButton.Button3:
                    defaultButtonIndex = 2;
                    break;
            }

            // Focus default button
            int i = 0;
            foreach (var button in Buttons)
                if (button.Visible)
                    if (i++ == defaultButtonIndex)
                    {
                        button.Select();
                        break;
                    }
        }

        private void UpdateButtons()
        {
            foreach (var btn in Buttons)
                btn.Visible = false;

            switch (_dialogButtons)
            {
                case MessageBoxButtons.OK:
                    ShowButton(btnOk, true);
                    AcceptButton = btnOk;
                    break;
                case MessageBoxButtons.OKCancel:
                    ShowButton(btnOk);
                    ShowButton(btnCancel, true);
                    AcceptButton = btnOk;
                    CancelButton = btnCancel;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    ShowButton(btnAbort);
                    ShowButton(btnRetry);
                    ShowButton(btnIgnore, true);
                    AcceptButton = btnAbort;
                    CancelButton = btnIgnore;
                    break;
                case MessageBoxButtons.RetryCancel:
                    ShowButton(btnRetry);
                    ShowButton(btnCancel, true);
                    AcceptButton = btnRetry;
                    CancelButton = btnCancel;
                    break;
                case MessageBoxButtons.YesNo:
                    ShowButton(btnYes);
                    ShowButton(btnNo, true);
                    AcceptButton = btnYes;
                    CancelButton = btnNo;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    ShowButton(btnYes);
                    ShowButton(btnNo);
                    ShowButton(btnCancel, true);
                    AcceptButton = btnYes;
                    CancelButton = btnCancel;
                    break;
                default:
                    throw new NotImplementedException("MessageBoxButtons " + _dialogButtons + " unavailable.");
            }

            SetFlowSize();
        }

        private static void ShowButton(Control button, bool isLast = false)
        {
            button.SendToBack();

            if (!isLast)
                button.Margin = new Padding(0, 0, 10, 0);

            button.Visible = true;
        }

        private void SetFlowSize()
        {
            var width = flowInner.Padding.Horizontal;

            foreach (var btn in Buttons)
                if (btn.Visible)
                    width += btn.Width + btn.Margin.Right;

            flowInner.Width = width;
            TotalButtonSize = width;
        }
    }
}
