using DarkUI.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    [ToolboxItem(false)]
    public class DarkDockGroup : Panel
    {
        #region Field Region

        private List<DarkDockContent> _contents;

        private DarkDockTabArea _tabArea;

        #endregion

        #region Property Region

        public DarkDockPanel DockPanel { get; private set; }

        public DarkDockRegion DockRegion { get; private set; }

        public DarkDockArea DockArea { get; private set; }

        public DarkDockContent VisibleContent { get; private set; }

        public int Order { get; set; }

        public int ContentCount { get { return _contents.Count; } }

        #endregion

        #region Constructor Region

        public DarkDockGroup(DarkDockPanel dockPanel, DarkDockRegion dockRegion, int order)
        {
            _contents = new List<DarkDockContent>();

            DockPanel = dockPanel;
            DockRegion = dockRegion;
            DockArea = dockRegion.DockArea;

            Order = order;

            _tabArea = new DarkDockTabArea(DockArea);
        }

        #endregion

        #region Method Region

        public void AddContent(DarkDockContent dockContent)
        {
            dockContent.DockGroup = this;
            dockContent.Dock = DockStyle.Fill;

            _contents.Add(dockContent);
            Controls.Add(dockContent);

            if (VisibleContent == null)
                VisibleContent = dockContent;

            var menuItem = new ToolStripMenuItem(dockContent.DockText);
            menuItem.Tag = dockContent;
            menuItem.Click += TabMenuItem_Select;
            menuItem.Image = dockContent.Icon;
            _tabArea.TabMenu.Items.Add(menuItem);

            UpdateTabArea();
        }

        public void RemoveContent(DarkDockContent dockContent)
        {
            dockContent.DockGroup = null;

            _contents.Remove(dockContent);
            Controls.Remove(dockContent);

            if (VisibleContent == dockContent)
            {
                VisibleContent = null;

                // todo: order?
                foreach (var content in _contents)
                    VisibleContent = content;
            }

            ToolStripMenuItem itemToRemove = null;
            foreach (ToolStripMenuItem item in _tabArea.TabMenu.Items)
            {
                var menuContent = item.Tag as DarkDockContent;
                if (menuContent == null)
                    continue;

                if (menuContent == dockContent)
                    itemToRemove = item;
            }

            if (itemToRemove != null)
            {
                itemToRemove.Click -= TabMenuItem_Select;
                _tabArea.TabMenu.Items.Remove(itemToRemove);
            }

            UpdateTabArea();
        }

        private void UpdateTabArea()
        {
            if (DockArea == DarkDockArea.Document)
                _tabArea.Visible = (_contents.Count > 0);
            else
                _tabArea.Visible = (_contents.Count > 1);

            var size = 0;

            switch (DockArea)
            {
                case DarkDockArea.Document:
                    size = _tabArea.Visible ? Consts.DocumentTabAreaSize : 0;
                    Padding = new Padding(0, size, 0, 0);
                    _tabArea.Area = new Rectangle(Padding.Left, 0, ClientRectangle.Width - Padding.Horizontal, size);
                    break;
                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    size = _tabArea.Visible ? Consts.ToolWindowTabAreaSize : 0;
                    Padding = new Padding(0, 0, 0, size);
                    _tabArea.Area = new Rectangle(Padding.Left, ClientRectangle.Height - size, ClientRectangle.Width - Padding.Horizontal, size);
                    break;
                case DarkDockArea.Bottom:
                    size = _tabArea.Visible ? Consts.ToolWindowTabAreaSize : 0;
                    Padding = new Padding(1, 0, 0, size);
                    _tabArea.Area = new Rectangle(Padding.Left, ClientRectangle.Height - size, ClientRectangle.Width - Padding.Horizontal, size);
                    break;
            }

            BuildTabs();
        }

        private void BuildTabs()
        {
            if (!_tabArea.Visible)
                return;

            SuspendLayout();



            ResumeLayout();

            Invalidate();
        }

        #endregion

        #region Event Handler Region

        private void TabMenuItem_Select(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null)
                return;

            var content = menuItem.Tag as DarkDockContent;
            if (content == null)
                return;

            DockPanel.ActiveContent = content;
        }

        #endregion

        #region Render Region

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            using (var b = new SolidBrush(Colors.GreyBackground))
            {
                g.FillRectangle(b, ClientRectangle);
            }

            if (!_tabArea.Visible)
                return;

            using (var b = new SolidBrush(Colors.MediumBackground))
            {
                g.FillRectangle(b, _tabArea.Area);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Absorb event
        }

        #endregion
    }
}
