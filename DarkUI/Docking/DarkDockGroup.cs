using DarkUI.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    [ToolboxItem(false)]
    public class DarkDockGroup : Panel
    {
        #region Field Region

        private List<DarkDockContent> _contents = new List<DarkDockContent>();

        private Dictionary<DarkDockContent, DarkDockTab> _tabs = new Dictionary<DarkDockContent, DarkDockTab>();

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
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

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

            _tabs.Add(dockContent, new DarkDockTab(dockContent));

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

            if (_tabs.ContainsKey(dockContent))
                _tabs.Remove(dockContent);

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
                    _tabArea.ClientRectangle = new Rectangle(Padding.Left, 0, ClientRectangle.Width - Padding.Horizontal, size);
                    break;
                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    size = _tabArea.Visible ? Consts.ToolWindowTabAreaSize : 0;
                    Padding = new Padding(0, 0, 0, size);
                    _tabArea.ClientRectangle = new Rectangle(Padding.Left, ClientRectangle.Bottom - size, ClientRectangle.Width - Padding.Horizontal, size);
                    break;
                case DarkDockArea.Bottom:
                    size = _tabArea.Visible ? Consts.ToolWindowTabAreaSize : 0;
                    Padding = new Padding(1, 0, 0, size);
                    _tabArea.ClientRectangle = new Rectangle(Padding.Left, ClientRectangle.Bottom - size, ClientRectangle.Width - Padding.Horizontal, size);
                    break;
            }

            if (DockArea == DarkDockArea.Document)
            {
                var dropdownSize = Consts.DocumentTabAreaSize;
                _tabArea.DropdownRectangle = new Rectangle(_tabArea.ClientRectangle.Right - dropdownSize, 0, dropdownSize, dropdownSize);
            }

            BuildTabs();
        }

        private void BuildTabs()
        {
            if (!_tabArea.Visible)
                return;

            SuspendLayout();

            var closeButtonSize = DockIcons.close.Width;

            // Calculate areas of all tabs
            var totalSize = 0;

            foreach (var tab in _tabs.Values)
            {
                int width;

                using (var g = CreateGraphics())
                {
                    width = tab.CalculateWidth(g, Font);
                }

                // Add area for the close button
                if (DockArea == DarkDockArea.Document)
                {
                    width += closeButtonSize;

                    if (tab.DockContent.Icon != null)
                        width += tab.DockContent.Icon.Width + 5;
                }

                // Show separator on all tabs for now
                tab.ShowSeparator = true;
                width += 1;

                var y = DockArea == DarkDockArea.Document ? 0 : ClientRectangle.Height - Consts.ToolWindowTabAreaSize;
                var height = DockArea == DarkDockArea.Document ? Consts.DocumentTabAreaSize : Consts.ToolWindowTabAreaSize;

                var tabRect = new Rectangle(_tabArea.ClientRectangle.Left + totalSize, y, width, height);
                tab.ClientRectangle = tabRect;

                totalSize += width;
            }

            // Cap the size if too large for the tab area
            if (DockArea != DarkDockArea.Document)
            {
                if (totalSize > _tabArea.ClientRectangle.Width)
                {
                    var difference = totalSize - _tabArea.ClientRectangle.Width;

                    // No matter what, we want to slice off the 1 pixel separator from the final tab.
                    var lastTab = _tabs.Values.Last();
                    var tabRect = lastTab.ClientRectangle;
                    lastTab.ClientRectangle = new Rectangle(tabRect.Left, tabRect.Top, tabRect.Width - 1, tabRect.Height);
                    lastTab.ShowSeparator = false;

                    var differenceMadeUp = 1;

                    // Loop through and progressively resize the larger tabs until the total size fits within the tab area.
                    while (differenceMadeUp < difference)
                    {
                        var largest = _tabs.Values.OrderByDescending(tab => tab.ClientRectangle.Width)
                                                                     .First()
                                                                     .ClientRectangle.Width;

                        foreach (var tab in _tabs.Values)
                        {
                            // Check if previous iteration of loop met the difference
                            if (differenceMadeUp >= difference)
                                continue;

                            if (tab.ClientRectangle.Width >= largest)
                            {
                                var rect = tab.ClientRectangle;
                                tab.ClientRectangle = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height);
                                differenceMadeUp += 1;
                            }
                        }
                    }

                    // After resizing the tabs reposition them accordingly.
                    var xOffset = 0;
                    foreach (var tab in _tabs.Values)
                    {
                        var rect = tab.ClientRectangle;
                        tab.ClientRectangle = new Rectangle(_tabArea.ClientRectangle.Left + xOffset, rect.Top, rect.Width, rect.Height);

                        xOffset += rect.Width;
                    }
                }
            }

            // Build close button rectangles
            if (DockArea == DarkDockArea.Document)
            {
                foreach (var tab in _tabs.Values)
                {
                    var closeRect = new Rectangle(tab.ClientRectangle.Right - 7 - closeButtonSize - 1,
                                                  tab.ClientRectangle.Top + (tab.ClientRectangle.Height / 2) - (closeButtonSize / 2) - 1,
                                                  closeButtonSize, closeButtonSize);
                    tab.CloseButtonRectangle = closeRect;
                }
            }

            ResumeLayout();

            Invalidate();
        }

        private Point PointToTabArea(Point point)
        {
            return new Point(point.X - _tabArea.Offset, point.Y);
        }

        private Rectangle RectangleToTabArea(Rectangle rectangle)
        {
            return new Rectangle(PointToTabArea(rectangle.Location), rectangle.Size);
        }

        #endregion

        #region Event Handler Region

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            UpdateTabArea();
        }

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

        public void Redraw()
        {
            Invalidate();

            foreach (var content in _contents)
                content.Invalidate();
        }

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
                g.FillRectangle(b, _tabArea.ClientRectangle);
            }

            foreach (var tab in _tabs.Values)
            {
                if (DockArea == DarkDockArea.Document)
                    PaintDocumentTab(g, tab);
                else
                    PaintToolWindowTab(g, tab);
            }

            if (DockArea == DarkDockArea.Document)
            {
                // Color divider
                var isActiveGroup = DockPanel.ActiveGroup == this;
                var divColor = isActiveGroup ? Colors.BlueSelection : Colors.GreySelection;
                using (var b = new SolidBrush(divColor))
                {
                    var divRect = new Rectangle(_tabArea.ClientRectangle.Left, _tabArea.ClientRectangle.Bottom - 2, _tabArea.ClientRectangle.Width, 2);
                    g.FillRectangle(b, divRect);
                }

                // Content dropdown list
                var dropdownRect = new Rectangle(_tabArea.DropdownRectangle.Left, _tabArea.DropdownRectangle.Top, _tabArea.DropdownRectangle.Width, _tabArea.DropdownRectangle.Height - 2);

                using (var b = new SolidBrush(Colors.MediumBackground))
                {
                    g.FillRectangle(b, dropdownRect);
                }

                using (var img = DockIcons.arrow)
                {
                    g.DrawImageUnscaled(img, dropdownRect.Left + (dropdownRect.Width / 2) - (img.Width / 2), dropdownRect.Top + (dropdownRect.Height / 2) - (img.Height / 2) + 1);
                }
            }
        }

        private void PaintDocumentTab(Graphics g, DarkDockTab tab)
        {
            var tabRect = RectangleToTabArea(tab.ClientRectangle);

            var isVisibleTab = VisibleContent == tab.DockContent;
            var isActiveGroup = DockPanel.ActiveGroup == this;

            var bgColor = isVisibleTab ? Colors.BlueSelection : Colors.DarkBackground;

            if (!isActiveGroup)
                bgColor = isVisibleTab ? Colors.GreySelection : Colors.DarkBackground;

            if (tab.Hot && !isVisibleTab)
                bgColor = Colors.MediumBackground;

            using (var b = new SolidBrush(bgColor))
            {
                g.FillRectangle(b, tabRect);
            }

            // Draw separators
            if (tab.ShowSeparator)
            {
                using (var p = new Pen(Colors.DarkBorder))
                {
                    g.DrawLine(p, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom);
                }
            }

            var xOffset = 0;

            // Draw icon
            if (tab.DockContent.Icon != null)
            {
                g.DrawImageUnscaled(tab.DockContent.Icon, tabRect.Left + 5, tabRect.Top + 4);
                xOffset += tab.DockContent.Icon.Width + 2;
            }

            var tabTextFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            };

            // Draw text
            var textColor = isVisibleTab ? Colors.LightText : Colors.DisabledText;
            using (var b = new SolidBrush(textColor))
            {
                var textRect = new Rectangle(tabRect.Left + 5 + xOffset, tabRect.Top, tabRect.Width - tab.CloseButtonRectangle.Width - 7 - 5 - xOffset, tabRect.Height);
                g.DrawString(tab.DockContent.DockText, Font, b, textRect, tabTextFormat);
            }

            // Close button
            var img = tab.CloseButtonHot ? DockIcons.inactive_close_selected : DockIcons.inactive_close;

            if (isVisibleTab)
            {
                if (isActiveGroup)
                    img = tab.CloseButtonHot ? DockIcons.close_selected : DockIcons.close;
                else
                    img = tab.CloseButtonHot ? DockIcons.close_selected : DockIcons.active_inactive_close;
            }

            var closeRect = RectangleToTabArea(tab.CloseButtonRectangle);
            g.DrawImageUnscaled(img, closeRect.Left, closeRect.Top);
        }

        private void PaintToolWindowTab(Graphics g, DarkDockTab tab)
        {
            var tabRect = tab.ClientRectangle;

            var isVisibleTab = VisibleContent == tab.DockContent;

            var bgColor = isVisibleTab ? Colors.GreyBackground : Colors.DarkBackground;

            if (tab.Hot && !isVisibleTab)
                bgColor = Colors.MediumBackground;

            using (var b = new SolidBrush(bgColor))
            {
                g.FillRectangle(b, tabRect);
            }

            // Draw separators
            if (tab.ShowSeparator)
            {
                using (var p = new Pen(Colors.DarkBorder))
                {
                    g.DrawLine(p, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom);
                }
            }

            var tabTextFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap,
                Trimming = StringTrimming.EllipsisCharacter
            };

            var textColor = isVisibleTab ? Colors.BlueHighlight : Colors.DisabledText;
            using (var b = new SolidBrush(textColor))
            {
                var textRect = new Rectangle(tabRect.Left + 5, tabRect.Top, tabRect.Width - 5, tabRect.Height);
                g.DrawString(tab.DockContent.DockText, Font, b, textRect, tabTextFormat);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Absorb event
        }

        #endregion
    }
}
