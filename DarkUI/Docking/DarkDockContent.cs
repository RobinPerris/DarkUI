using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    public class DockContentEventArgs : EventArgs
    {
        public DarkDockContent Content { get; private set; }

        public DockContentEventArgs(DarkDockContent content)
        {
            Content = content;
        }
    }

    public class DockContentClosingEventArgs : EventArgs
    {
        public DarkDockContent Content { get; private set; }

        public bool Cancel { get; set; }

        public DockContentClosingEventArgs(DarkDockContent content)
        {
            Content = content;
        }
    }

    public class DockTextChangedEventArgs : EventArgs
    {
        public DarkDockContent Content { get; private set; }

        public string OldText { get; private set; }

        public string NewText { get; private set; }

        public DockTextChangedEventArgs(DarkDockContent content, string oldText, string newText)
        {
            Content = content;
            OldText = oldText;
            NewText = newText;
        }
    }

    [ToolboxItem(false)]
    public class DarkDockContent : UserControl
    {
        #region Event Region

        public event EventHandler<DockContentClosingEventArgs> Closing;
        public event EventHandler<DockContentEventArgs> Closed;
        public event EventHandler<DockTextChangedEventArgs> DockHeaderChanged;

        #endregion

        #region Field Region

        private string _dockText;
        private Image _icon;

        #endregion

        #region Property Region

        [Category("Appearance")]
        [Description("Determines the text that will appear in the content tabs and headers.")]
        public string DockText
        {
            get { return _dockText; }
            set
            {
                var oldText = _dockText;

                _dockText = value;

                if (DockHeaderChanged != null)
                    DockHeaderChanged(this, new DockTextChangedEventArgs(this, oldText, _dockText));

                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Determines the icon that will appear in the content tabs and headers.")]
        public Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        [Category("Layout")]
        [Description("Determines which area of the dock panel this content will dock to.")]
        [DefaultValue(DarkDockArea.Document)]
        public DarkDockArea DockArea { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockPanel DockPanel { get; internal set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockRegion DockRegion { get; internal set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockGroup DockGroup { get; internal set; }

        #endregion

        #region Constructor Region

        public DarkDockContent()
        { }

        #endregion

        #region Method Region

        public virtual void Close()
        {
            var e = new DockContentClosingEventArgs(this);

            if (Closing != null)
                Closing(this, e);

            if (e.Cancel)
                return;

            if (DockPanel != null)
                DockPanel.RemoveContent(this);

            if (Closed != null)
                Closed(this, new DockContentEventArgs(this));
        }

        #endregion

        #region Event Handler Region

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (DockPanel == null)
                return;

            DockPanel.ActiveContent = this;
        }

        #endregion
    }
}
