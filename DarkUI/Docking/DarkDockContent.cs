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

    [ToolboxItem(false)]
    public class DarkDockContent : UserControl
    {
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
                _dockText = value;
                Invalidate();
                // TODO: raise event for re-sizing parent tabs
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
            if (DockPanel != null)
                DockPanel.RemoveContent(this);
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
