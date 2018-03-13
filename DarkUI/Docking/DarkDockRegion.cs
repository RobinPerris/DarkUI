using DarkUI.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    [ToolboxItem(false)]
    public class DarkDockRegion : Panel
    {
        #region Field Region

        private readonly List<DarkDockGroup> _groups;

        private Form _parentForm;
        private DarkDockSplitter _splitter;

        #endregion

        #region Property Region

        public DarkDockPanel DockPanel { get; private set; }

        public DarkDockArea DockArea { get; private set; }

        public DarkDockContent ActiveDocument
        {
            get
            {
                if (DockArea != DarkDockArea.Document || _groups.Count == 0)
                    return null;

                return _groups[0].VisibleContent;
            }
        }

        public List<DarkDockGroup> Groups
        {
            get
            {
                return _groups.ToList();
            }
        }

        #endregion

        #region Constructor Region

        public DarkDockRegion(DarkDockPanel dockPanel, DarkDockArea dockArea)
        {
            _groups = new List<DarkDockGroup>();

            DockPanel = dockPanel;
            DockArea = dockArea;

            BuildProperties();
        }

        #endregion

        #region Method Region

        internal void AddContent(DarkDockContent dockContent)
        {
            AddContent(dockContent, null);
        }

        internal void AddContent(DarkDockContent dockContent, DarkDockGroup dockGroup)
        {
            // If no existing group is specified then create a new one
            if (dockGroup == null)
            {
                // If this is the document region, then default to first group if it exists
                if (DockArea == DarkDockArea.Document && _groups.Count > 0)
                    dockGroup = _groups[0];
                else
                    dockGroup = CreateGroup();
            }

            dockContent.DockRegion = this;
            dockGroup.AddContent(dockContent);

            if (!Visible)
            {
                Visible = true;
                CreateSplitter();
            }

            RebuildGroupSplitters();
            PositionGroups();
        }

        internal void InsertContent(DarkDockContent dockContent, DarkDockGroup dockGroup, DockInsertType insertType)
        {
            var order = dockGroup.Order;

            if (insertType == DockInsertType.After)
                order++;

            var newGroup = InsertGroup(order);

            dockContent.DockRegion = this;
            newGroup.AddContent(dockContent);

            if (!Visible)
            {
                Visible = true;
                CreateSplitter();
            }

            RebuildGroupSplitters();
            PositionGroups();
        }

        internal void RemoveContent(DarkDockContent dockContent)
        {
            dockContent.DockRegion = null;

            var group = dockContent.DockGroup;
            group.RemoveContent(dockContent);

            dockContent.DockArea = DarkDockArea.None;

            // If that was the final content in the group then remove the group
            if (group.ContentCount == 0)
                RemoveGroup(group);

            // If we just removed the final group, and this isn't the document region, then hide
            if (_groups.Count == 0 && DockArea != DarkDockArea.Document)
            {
                Visible = false;
                RemoveSplitter();
            }

            RebuildGroupSplitters();
            PositionGroups();
        }

        public List<DarkDockContent> GetContents()
        {
            var result = new List<DarkDockContent>();

            foreach (var group in _groups)
                result.AddRange(group.GetContents());

            return result;
        }

        private DarkDockGroup CreateGroup()
        {
            var order = 0;

            if (_groups.Count >= 1)
            {
                order = -1;
                foreach (var group in _groups)
                {
                    if (group.Order >= order)
                        order = group.Order + 1;
                }
            }

            var newGroup = new DarkDockGroup(DockPanel, this, order);
            _groups.Add(newGroup);
            Controls.Add(newGroup);

            return newGroup;
        }

        private DarkDockGroup InsertGroup(int order)
        {
            foreach (var group in _groups)
            {
                if (group.Order >= order)
                    group.Order++;
            }

            var newGroup = new DarkDockGroup(DockPanel, this, order);
            _groups.Add(newGroup);
            Controls.Add(newGroup);

            return newGroup;
        }

        private void RemoveGroup(DarkDockGroup group)
        {
            var lastOrder = group.Order;

            _groups.Remove(group);
            Controls.Remove(group);

            group.RemoveSplitter();

            foreach (var otherGroup in _groups)
            {
                if (otherGroup.Order > lastOrder)
                    otherGroup.Order--;
            }
        }

        private void PositionGroups()
        {
            DockStyle dockStyle;

            switch (DockArea)
            {
                default:
                case DarkDockArea.Document:
                    dockStyle = DockStyle.Fill;
                    break;
                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    dockStyle = DockStyle.Top;
                    break;
                case DarkDockArea.Bottom:
                    dockStyle = DockStyle.Left;
                    break;
            }

            if (_groups.Count == 1)
            {
                _groups[0].Dock = DockStyle.Fill;
            }
            else if (_groups.Count > 1)
            {
                var lastGroup = _groups.OrderByDescending(g => g.Order).First();

                foreach (var group in _groups.OrderByDescending(g => g.Order))
                {
                    group.SendToBack();

                    if (group.Order == lastGroup.Order)
                        group.Dock = DockStyle.Fill;
                    else
                        group.Dock = dockStyle;
                }

                SizeGroups();
            }

            UpdateMinimumSize();

            _splitter?.UpdateBounds();
        }

        private void SizeGroups()
        {
            if (_groups.Count <= 1)
                return;

            bool restart;
            var lastGroup = _groups.OrderByDescending(g => g.Order).First();

            switch (DockArea)
            {
                default:
                case DarkDockArea.Document:
                    return;

                // At first we try to resize group according to their native size.
                // If any group is pushed out of screen space by other groups, we try to cut largest group by the minimum size of current one.
                // If all groups already cut by minimum size, we stop trying.

                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    do
                    {
                        restart = false;
                        foreach (var group in _groups)
                        {
                            if (group.Location.Y >= ClientRectangle.Height - group.MinimumSize.Height)
                                restart = CropLargestGroup(@group.MinimumSize.Height > 0 ? group.MinimumSize.Height : Consts.ToolWindowHeaderSize);

                            if (group.Height <= 0)
                                group.Size = new Size(ClientRectangle.Width, @group.MinimumSize.Height > 0 ? @group.MinimumSize.Width : Consts.ToolWindowHeaderSize);
                            else if (@group.Order == lastGroup.Order && @group.Location.Y > ClientRectangle.Height)
                                group.Size = new Size(ClientRectangle.Width, group.Location.Y - ClientRectangle.Height);
                            else
                                group.Size = new Size(ClientRectangle.Width, group.Height);
                        }
                    } while (restart);
                    break;

                case DarkDockArea.Bottom:
                    do
                    {
                        restart = false;
                        foreach (var group in _groups)
                        {
                            if (group.Location.X >= ClientRectangle.Width - group.MinimumSize.Width)
                                restart = CropLargestGroup(@group.MinimumSize.Width > 0 ? group.MinimumSize.Width : Consts.ToolWindowHeaderSize);

                            if (group.Width <= 0)
                                group.Size = new Size(@group.MinimumSize.Width > 0 ? @group.MinimumSize.Width : Consts.ToolWindowHeaderSize, ClientRectangle.Height);
                            else if (@group.Order == lastGroup.Order && @group.Location.X > ClientRectangle.Width)
                                group.Size = new Size(group.Location.X - ClientRectangle.Width, ClientRectangle.Height);
                            else
                                group.Size = new Size(group.Width, ClientRectangle.Height);
                        }
                    } while (restart);
                    break;
            }

        }

        private bool CropLargestGroup(int spaceToCut)
        {
            DarkDockGroup largestGroup = null;

            if (_groups.Count <= 1 || spaceToCut == 0 || DockArea == DarkDockArea.Document)
                return false;

            int maxSize = 0;

            switch (DockArea)
            {
                default:
                    return false;

                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    foreach (var group in _groups)
                        if (group.Height > maxSize && group.Height > group.MinimumSize.Height)
                        {
                            maxSize = group.Height;
                            largestGroup = group;
                        }
                    if(largestGroup != null && largestGroup.Size.Height > spaceToCut && largestGroup.Size.Height > largestGroup.MinimumSize.Height)
                    {
                        largestGroup.Size = new Size(largestGroup.Size.Width, largestGroup.Size.Height - spaceToCut);
                        return true;
                    }
                    break;

                case DarkDockArea.Bottom:
                    foreach (var group in _groups)
                        if (group.Width > maxSize && group.Width > group.MinimumSize.Width)
                        {
                            maxSize = group.Width;
                            largestGroup = group;
                        }
                    if (largestGroup != null && largestGroup.Size.Width > spaceToCut && largestGroup.Size.Width > largestGroup.MinimumSize.Width)
                    {
                        largestGroup.Size = new Size(largestGroup.Size.Width - spaceToCut, largestGroup.Size.Height);
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void UpdateMinimumSize()
        {
            int minRegionSize = Consts.MinimumRegionSize;

            if (_groups.Count < 1)
                return;

            switch (DockArea)
            {
                default:
                    return;

                case DarkDockArea.Left:
                case DarkDockArea.Right:
                    foreach (var group in _groups)
                        if (minRegionSize < group.MinimumSize.Width)
                            minRegionSize = group.MinimumSize.Width + 2;
                    MinimumSize = new Size(minRegionSize, 0);
                    break;

            case DarkDockArea.Bottom:
                    foreach (var group in _groups)
                        if (minRegionSize < group.MinimumSize.Height)
                            minRegionSize = group.MinimumSize.Height + 2;
                    MinimumSize = new Size(0, minRegionSize);
                    break;
            }
        }

        private void UpdateSplitterBounds()
        {
            _splitter?.UpdateBounds();

            if (DockArea != DarkDockArea.Document)
            {
                foreach (var regionGroup in Groups)
                    regionGroup.UpdateSplitterBounds();
            }
        }

        private void BuildProperties()
        {
            MinimumSize = new Size(50, 50);

            switch (DockArea)
            {
                default:
                case DarkDockArea.Document:
                    Dock = DockStyle.Fill;
                    Padding = new Padding(0, 1, 0, 0);
                    break;
                case DarkDockArea.Left:
                    Dock = DockStyle.Left;
                    Padding = new Padding(0, 0, 1, 0);
                    Visible = false;
                    break;
                case DarkDockArea.Right:
                    Dock = DockStyle.Right;
                    Padding = new Padding(1, 0, 0, 0);
                    Visible = false;
                    break;
                case DarkDockArea.Bottom:
                    Dock = DockStyle.Bottom;
                    Padding = new Padding(0, 0, 0, 0);
                    Visible = false;
                    break;
            }
        }

        private void CreateSplitter()
        {
            RemoveSplitter();

            switch (DockArea)
            {
                case DarkDockArea.Left:
                    _splitter = new DarkDockSplitter(DockPanel, this, DarkSplitterType.Right, DarkSplitterMode.Region);
                    break;
                case DarkDockArea.Right:
                    _splitter = new DarkDockSplitter(DockPanel, this, DarkSplitterType.Left, DarkSplitterMode.Region);
                    break;
                case DarkDockArea.Bottom:
                    _splitter = new DarkDockSplitter(DockPanel, this, DarkSplitterType.Top, DarkSplitterMode.Region);
                    break;
                default:
                    return;
            }

            DockPanel.Splitters.Add(_splitter);
        }

        private void RemoveSplitter()
        {
            if (_splitter != null && DockPanel.Splitters.Contains(_splitter))
                DockPanel.Splitters.Remove(_splitter);
        }

        private void RebuildGroupSplitters()
        {
            if (DockArea == DarkDockArea.Document)
                return;
            
            foreach (var regionGroup in Groups)
            {
                if (Groups.Count <= 1 || regionGroup.Order == _groups.OrderByDescending(g => g.Order).First().Order)
                    regionGroup.RemoveSplitter();
                else
                    regionGroup.CreateSplitter();
            }
        }

        #endregion

        #region Event Handler Region

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            _parentForm = FindForm();
            Debug.Assert(_parentForm != null, nameof(_parentForm) + " != null");
            _parentForm.ResizeEnd += ParentForm_ResizeEnd;
        }

        private void ParentForm_ResizeEnd(object sender, EventArgs e)
        {
            UpdateSplitterBounds();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            UpdateSplitterBounds();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            UpdateSplitterBounds();
        }

        #endregion

        #region Paint Region

        public void Redraw()
        {
            Invalidate();

            foreach (var group in _groups)
                group.Redraw();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            if (!Visible)
                return;

            // Fill body
            using (var b = new SolidBrush(Colors.GreyBackground))
            {
                g.FillRectangle(b, ClientRectangle);
            }

            // Draw border
            using (var p = new Pen(Colors.DarkBorder))
            {
                // Top border
                if (DockArea == DarkDockArea.Document)
                    g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Right, 0);

                // Left border
                if (DockArea == DarkDockArea.Right)
                    g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Left, ClientRectangle.Height);

                // Right border
                if (DockArea == DarkDockArea.Left)
                    g.DrawLine(p, ClientRectangle.Right - 1, 0, ClientRectangle.Right - 1, ClientRectangle.Height);
            }
        }

        #endregion
    }
}
