using DarkUI.Config;
using DarkUI.Docking;
using DarkUI.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Win32
{
    public class DockContentDragFilter : IMessageFilter
    {
        #region Field Region

        private DarkDockPanel _dockPanel;

        private DarkDockContent _dragContent;

        private DarkTranslucentForm _highlightForm;

        private DarkDockGroup _targetGroup;

        private Dictionary<DarkDockGroup, Rectangle> _groupDropAreas = new Dictionary<DarkDockGroup, Rectangle>();

        #endregion

        #region Constructor Region

        public DockContentDragFilter(DarkDockPanel dockPanel)
        {
            _dockPanel = dockPanel;

            _highlightForm = new DarkTranslucentForm(Colors.BlueSelection);
        }

        #endregion

        #region IMessageFilter Region

        public bool PreFilterMessage(ref Message m)
        {
            // Exit out early if we're not dragging any content
            if (_dragContent == null)
                return false;

            // We only care about mouse events
            if (!(m.Msg == (int)WM.MOUSEMOVE ||
                  m.Msg == (int)WM.LBUTTONDOWN || m.Msg == (int)WM.LBUTTONUP || m.Msg == (int)WM.LBUTTONDBLCLK ||
                  m.Msg == (int)WM.RBUTTONDOWN || m.Msg == (int)WM.RBUTTONUP || m.Msg == (int)WM.RBUTTONDBLCLK))
                return false;

            // Move content
            if (m.Msg == (int)WM.MOUSEMOVE)
            {
                HandleDrag();
                return false;
            }

            // Drop content
            if (m.Msg == (int)WM.LBUTTONUP)
            {
                if (_targetGroup != null)
                    _dockPanel.AddContent(_dragContent, _targetGroup);

                StopDrag();
                return false;
            }

            return true;
        }

        #endregion

        #region Method Region

        public void StartDrag(DarkDockContent content)
        {
            _groupDropAreas = new Dictionary<DarkDockGroup, Rectangle>();

            foreach (var region in _dockPanel.Regions)
            {
                foreach (var group in region.Groups)
                {
                    var rect = new Rectangle
                    {
                        X = group.PointToScreen(Point.Empty).X,
                        Y = group.PointToScreen(Point.Empty).Y,
                        Width = group.Width,
                        Height = group.Height
                    };

                    _groupDropAreas.Add(group, rect);
                }
            }

            _dragContent = content;
        }

        private void StopDrag()
        {
            _highlightForm.Hide();
            _dragContent = null;
        }

        private void HandleDrag()
        {
            var location = Cursor.Position;

            _targetGroup = null;

            foreach (var keyValuePair in _groupDropAreas)
            {
                var group = keyValuePair.Key;
                var rect = keyValuePair.Value;

                if (group == _dragContent.DockGroup)
                    continue;

                if (group.DockArea == DarkDockArea.Document)
                    continue;

                if (rect.Contains(location))
                {
                    _targetGroup = group;

                    _highlightForm.Location = new Point(rect.X, rect.Y);
                    _highlightForm.Size = new Size(rect.Width, rect.Height);
                }
            }

            if (_targetGroup == null)
            {
                if (_highlightForm.Visible)
                {
                    _highlightForm.Hide();
                }
            }
            else
            {
                if (!_highlightForm.Visible)
                {
                    _highlightForm.Show();
                    _highlightForm.BringToFront();
                }
            }
        }

        #endregion
    }
}
