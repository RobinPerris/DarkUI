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

        private bool _isDragging = false;
        private bool _insert = false;
        private DarkDockRegion _targetRegion;
        private DarkDockGroup _targetGroup;

        private Dictionary<DarkDockRegion, Rectangle> _regionDropAreas = new Dictionary<DarkDockRegion, Rectangle>();
        private Dictionary<DarkDockGroup, Rectangle> _groupInsertDropAreas = new Dictionary<DarkDockGroup, Rectangle>();
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
            if (!_isDragging)
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
                if (_targetRegion != null)
                {
                    _dockPanel.RemoveContent(_dragContent);
                    _dragContent.DockArea = _targetRegion.DockArea;
                    _dockPanel.AddContent(_dragContent);
                }
                else if (_targetGroup != null)
                {
                    _dockPanel.RemoveContent(_dragContent);

                    if (_insert)
                        _dockPanel.InsertContent(_dragContent, _targetGroup);
                    else
                        _dockPanel.AddContent(_dragContent, _targetGroup);
                }

                StopDrag();
                return false;
            }

            return true;
        }

        #endregion

        #region Method Region

        public void StartDrag(DarkDockContent content)
        {
            _regionDropAreas = new Dictionary<DarkDockRegion, Rectangle>();
            _groupInsertDropAreas = new Dictionary<DarkDockGroup, Rectangle>();
            _groupDropAreas = new Dictionary<DarkDockGroup, Rectangle>();

            foreach (var region in _dockPanel.Regions.Values)
            {
                if (region.Visible)
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

                        var insertRect = new Rectangle();

                        switch (group.DockArea)
                        {
                            case DarkDockArea.Left:
                            case DarkDockArea.Right:

                                var top = rect.Top;

                                if (group.Order > 0)
                                    top -= 7;

                                insertRect = new Rectangle
                                {
                                    X = rect.Left,
                                    Y = top,
                                    Width = rect.Width,
                                    Height = 15
                                };

                                break;

                            case DarkDockArea.Bottom:

                                var left = rect.Left;

                                if (group.Order > 0)
                                    left -= 7;

                                insertRect = new Rectangle
                                {
                                    X = left,
                                    Y = rect.Top,
                                    Width = 15,
                                    Height = rect.Height
                                };

                                break;
                        }

                        _groupDropAreas.Add(group, rect);
                        _groupInsertDropAreas.Add(group, insertRect);
                    }
                }
                else
                {
                    var rect = new Rectangle();

                    switch (region.DockArea)
                    {
                        case DarkDockArea.Left:

                            rect = new Rectangle
                            {
                                X = _dockPanel.PointToScreen(Point.Empty).X,
                                Y = _dockPanel.PointToScreen(Point.Empty).Y,
                                Width = 15,
                                Height = _dockPanel.Height
                            };

                            break;

                        case DarkDockArea.Right:

                            rect = new Rectangle
                            {
                                X = _dockPanel.PointToScreen(Point.Empty).X + _dockPanel.Width - 15,
                                Y = _dockPanel.PointToScreen(Point.Empty).Y,
                                Width = 15,
                                Height = _dockPanel.Height
                            };

                            break;

                        case DarkDockArea.Bottom:

                            var x = _dockPanel.PointToScreen(Point.Empty).X;
                            var width = _dockPanel.Width;

                            if (_dockPanel.Regions[DarkDockArea.Left].Visible)
                            {
                                x += _dockPanel.Regions[DarkDockArea.Left].Width;
                                width -= _dockPanel.Regions[DarkDockArea.Left].Width;
                            }

                            if (_dockPanel.Regions[DarkDockArea.Right].Visible)
                            {
                                width -= _dockPanel.Regions[DarkDockArea.Right].Width;
                            }

                            rect = new Rectangle
                            {
                                X = x,
                                Y = _dockPanel.PointToScreen(Point.Empty).Y + _dockPanel.Height - 15,
                                Width = width,
                                Height = 15
                            };

                            break;
                    }

                    _regionDropAreas.Add(region, rect);
                }
            }

            _dragContent = content;
            _isDragging = true;
        }

        private void StopDrag()
        {
            _highlightForm.Hide();
            _dragContent = null;
            _isDragging = false;
        }

        private void HandleDrag()
        {
            var location = Cursor.Position;

            _targetRegion = null;
            _targetGroup = null;

            foreach (var keyValuePair in _regionDropAreas)
            {
                var region = keyValuePair.Key;
                var rect = keyValuePair.Value;

                if (rect.Contains(location))
                {
                    _targetRegion = region;

                    _highlightForm.Location = new Point(rect.X, rect.Y);
                    _highlightForm.Size = new Size(rect.Width, rect.Height);
                }
            }

            var inserting = false;

            foreach (var keyValuePair in _groupInsertDropAreas)
            {
                var group = keyValuePair.Key;
                var rect = keyValuePair.Value;

                if (group.DockRegion == _dragContent.DockGroup.DockRegion)
                {
                    if (group == _dragContent.DockGroup)
                        continue;

                    if (_dragContent.DockGroup.Order == group.Order - 1)
                        continue;
                }

                if (rect.Contains(location))
                {
                    inserting = true;

                    _insert = true;
                    _targetGroup = group;

                    _highlightForm.Location = new Point(rect.X, rect.Y);
                    _highlightForm.Size = new Size(rect.Width, rect.Height);
                }
            }

            if (!inserting)
            {
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
                        _insert = false;
                        _targetGroup = group;

                        _highlightForm.Location = new Point(rect.X, rect.Y);
                        _highlightForm.Size = new Size(rect.Width, rect.Height);
                    }
                }
            }

            if (_targetRegion == null && _targetGroup == null)
            {
                if (_highlightForm.Visible)
                    _highlightForm.Hide();
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
