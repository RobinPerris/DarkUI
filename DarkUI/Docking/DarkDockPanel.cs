using DarkUI.Config;
using DarkUI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    public class DarkDockPanel : UserControl
    {
        #region Event Region

        public event EventHandler<DockContentEventArgs> ActiveContentChanged;
        public event EventHandler<DockContentEventArgs> ContentAdded;
        public event EventHandler<DockContentEventArgs> ContentRemoved;

        #endregion

        #region Field Region

        private List<DarkDockContent> _contents;
        private Dictionary<DarkDockArea, DarkDockRegion> _regions;

        private DarkDockContent _activeContent;
        private bool _switchingContent = false;

        #endregion

        #region Property Region

        public DarkDockContent ActiveContent
        {
            get { return _activeContent; }
            internal set
            {
                // Don't let content visibility changes re-trigger event
                if (_switchingContent)
                    return;

                _switchingContent = true;

                _activeContent = value;

                ActiveGroup = _activeContent.DockGroup;
                ActiveRegion = ActiveGroup.DockRegion;

                foreach (var region in _regions.Values)
                    region.Redraw();

                if (ActiveContentChanged != null)
                    ActiveContentChanged(this, new DockContentEventArgs(_activeContent));

                _switchingContent = false;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockRegion ActiveRegion { get; internal set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockGroup ActiveGroup { get; internal set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMessageFilter MessageFilter { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<DarkDockSplitter> Splitters { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MouseButtons MouseButtonState
        {
            get
            {
                var buttonState = MouseButtons;
                return buttonState;
            }
        }

        #endregion

        #region Constructor Region

        public DarkDockPanel()
        {
            Splitters = new List<DarkDockSplitter>();
            MessageFilter = new DarkDockResizeFilter(this);

            _regions = new Dictionary<DarkDockArea, DarkDockRegion>();
            _contents = new List<DarkDockContent>();

            BackColor = Colors.GreyBackground;

            CreateRegions();
        }

        #endregion

        #region Method Region

        public void AddContent(DarkDockContent dockContent)
        {
            AddContent(dockContent, null);
        }

        public void AddContent(DarkDockContent dockContent, DarkDockGroup dockGroup)
        {
            if (_contents.Contains(dockContent))
                return;

            if (dockGroup != null && dockContent.DockArea != dockGroup.DockArea)
                throw new Exception($"Attempting to add '{dockContent.DockArea}' content to '{dockGroup.DockArea}' group.");

            dockContent.DockPanel = this;
            _contents.Add(dockContent);

            var region = _regions[dockContent.DockArea];
            region.AddContent(dockContent, dockGroup);

            if (ContentAdded != null)
                ContentAdded(this, new DockContentEventArgs(dockContent));
        }

        public void RemoveContent(DarkDockContent dockContent)
        {
            if (!_contents.Contains(dockContent))
                return;

            dockContent.DockPanel = null;
            _contents.Remove(dockContent);

            var region = _regions[dockContent.DockArea];
            region.RemoveContent(dockContent);

            if (ContentRemoved != null)
                ContentRemoved(this, new DockContentEventArgs(dockContent));
        }

        public bool ContainsContent(DarkDockContent dockContent)
        {
            return _contents.Contains(dockContent);
        }

        private void CreateRegions()
        {
            var documentRegion = new DarkDockRegion(this, DarkDockArea.Document);
            _regions.Add(DarkDockArea.Document, documentRegion);

            var leftRegion = new DarkDockRegion(this, DarkDockArea.Left);
            _regions.Add(DarkDockArea.Left, leftRegion);

            var rightRegion = new DarkDockRegion(this, DarkDockArea.Right);
            _regions.Add(DarkDockArea.Right, rightRegion);

            var bottomRegion = new DarkDockRegion(this, DarkDockArea.Bottom);
            _regions.Add(DarkDockArea.Bottom, bottomRegion);

            // Add the regions in this order to force the bottom region to be positioned
            // between the left and right regions properly.
            Controls.Add(documentRegion);
            Controls.Add(bottomRegion);
            Controls.Add(leftRegion);
            Controls.Add(rightRegion);

            // Create tab index for intuitive tabbing order
            documentRegion.TabIndex = 0;
            rightRegion.TabIndex = 1;
            bottomRegion.TabIndex = 2;
            leftRegion.TabIndex = 3;
        }

        #endregion
    }
}
    