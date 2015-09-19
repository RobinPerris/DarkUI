using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    [ToolboxItem(false)]
    public class DarkDockRegion : Panel
    {
        #region Field Region

        private List<DarkDockGroup> _groups;

        #endregion

        #region Property Region

        public DarkDockPanel DockPanel { get; private set; }

        public DarkDockArea DockArea { get; private set; }

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

        public void AddContent(DarkDockContent dockContent)
        {
            AddContent(dockContent, null);
        }

        public void AddContent(DarkDockContent dockContent, DarkDockGroup dockGroup)
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

            // Show the region if it was previously hidden
            if (!Visible)
                Visible = true;
        }

        public void RemoveContent(DarkDockContent dockContent)
        {
            dockContent.DockRegion = null;

            var group = dockContent.DockGroup;
            group.RemoveContent(dockContent);

            // If that was the final content in the group then remove the group
            if (group.ContentCount == 0)
                RemoveGroup(group);

            // If we just removed the final group, and this isn't the document region, then hide
            if (_groups.Count == 0 && DockArea != DarkDockArea.Document)
                Visible = false;
        }

        private DarkDockGroup CreateGroup()
        {
            var newGroup = new DarkDockGroup(DockPanel, this);
            _groups.Add(newGroup);
            Controls.Add(newGroup);

            PositionGroups();

            return newGroup;
        }

        private void RemoveGroup(DarkDockGroup group)
        {
            _groups.Remove(group);
            Controls.Remove(group);

            PositionGroups();
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
                _groups[0].Dock = DockStyle.Fill;
            else if (_groups.Count > 1)
                foreach (var group in _groups)
                    group.Dock = dockStyle;
        }

        private void BuildProperties()
        {
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

        #endregion

        #region Paint Region

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
                    g.DrawLine(p, ClientRectangle.Left, 0, ClientRectangle.Left, Height);

                // Right border
                if (DockArea == DarkDockArea.Left)
                    g.DrawLine(p, ClientRectangle.Right - 1, 0, ClientRectangle.Right - 1, Height);
            }
        }

        #endregion
    }
}
