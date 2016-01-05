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
        private DarkDockRegion _targetRegion;
        private DarkDockGroup _targetGroup;
        private DockInsertType _insertType = DockInsertType.None;

        private Dictionary<DarkDockRegion, DockDropCollection> _regionDropAreas = new Dictionary<DarkDockRegion, DockDropCollection>();
        private Dictionary<DarkDockGroup, DockDropCollection> _groupDropAreas = new Dictionary<DarkDockGroup, DockDropCollection>();

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

                    switch (_insertType)
                    {
                        case DockInsertType.None:
                            _dockPanel.AddContent(_dragContent, _targetGroup);
                            break;

                        case DockInsertType.Before:
                        case DockInsertType.After:
                            _dockPanel.InsertContent(_dragContent, _targetGroup, _insertType);
                            break;
                    }                        
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
            _regionDropAreas = new Dictionary<DarkDockRegion, DockDropCollection>();
            _groupDropAreas = new Dictionary<DarkDockGroup, DockDropCollection>();

            // Add all regions and groups to the drop collections
            foreach (var region in _dockPanel.Regions.Values)
            {
                if (region.DockArea == DarkDockArea.Document)
                    continue;

                // If the region is visible then build drop areas for the groups.
                if (region.Visible)
                {
                    foreach (var group in region.Groups)
                    {
                        var collection = new DockDropCollection(_dockPanel, group);
                        _groupDropAreas.Add(group, collection);
                    }
                }
                // If the region is NOT visible then build a drop area for the region itself.
                else
                {
                    /*var rect = new Rectangle();

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

                    _regionDropAreas.Add(region, rect);*/
                }
            }

            _dragContent = content;
            _isDragging = true;
        }

        private void StopDrag()
        {
            Cursor.Current = Cursors.Default;

            _highlightForm.Hide();
            _dragContent = null;
            _isDragging = false;
        }

        private void UpdateHighlightForm(Rectangle rect)
        {
            Cursor.Current = Cursors.SizeAll;

            _highlightForm.SuspendLayout();

            _highlightForm.Size = new Size(rect.Width, rect.Height);
            _highlightForm.Location = new Point(rect.X, rect.Y);

            _highlightForm.ResumeLayout();

            if (!_highlightForm.Visible)
            {
                _highlightForm.Show();
                _highlightForm.BringToFront();
            }
        }

        private void HandleDrag()
        {
            var location = Cursor.Position;

            _insertType = DockInsertType.None;

            _targetRegion = null;
            _targetGroup = null;

            // Check all region drop areas
            foreach (var collection in _regionDropAreas.Values)
            {
                if (collection.InsertBeforeArea.DropArea.Contains(location))
                {
                    _insertType = DockInsertType.Before;
                    _targetRegion = collection.InsertBeforeArea.DockRegion;
                    UpdateHighlightForm(collection.InsertBeforeArea.HighlightArea);
                    return;
                }

                if (collection.InsertAfterArea.DropArea.Contains(location))
                {
                    _insertType = DockInsertType.After;
                    _targetRegion = collection.InsertAfterArea.DockRegion;
                    UpdateHighlightForm(collection.InsertAfterArea.HighlightArea);
                    return;
                }

                if (collection.DropArea.DropArea.Contains(location))
                {
                    _insertType = DockInsertType.None;
                    _targetRegion = collection.DropArea.DockRegion;
                    UpdateHighlightForm(collection.DropArea.HighlightArea);
                    return;
                }
            }

            // Check all group drop areas
            foreach (var collection in _groupDropAreas.Values)
            {
                var sameRegion = false;
                var sameGroup = false;
                var groupHasOtherContent = false;

                if (collection.DropArea.DockGroup == _dragContent.DockGroup)
                {
                    sameGroup = true;

                    if (collection.DropArea.DockGroup.ContentCount > 1)
                        groupHasOtherContent = true;
                }

                if (collection.DropArea.DockGroup.DockRegion == _dragContent.DockRegion)
                {
                    sameRegion = true;
                }

                if (!sameGroup || groupHasOtherContent)
                {
                    var skipBefore = false;
                    var skipAfter = false;

                    if (sameRegion && !groupHasOtherContent)
                    {
                        if (collection.InsertBeforeArea.DockGroup.Order == _dragContent.DockGroup.Order + 1)
                            skipBefore = true;

                        if (collection.InsertAfterArea.DockGroup.Order == _dragContent.DockGroup.Order - 1)
                            skipAfter = true;
                    }

                    if (!skipBefore)
                    {
                        if (collection.InsertBeforeArea.DropArea.Contains(location))
                        {
                            _insertType = DockInsertType.Before;
                            _targetGroup = collection.InsertBeforeArea.DockGroup;
                            UpdateHighlightForm(collection.InsertBeforeArea.HighlightArea);
                            return;
                        }
                    }

                    if (!skipAfter)
                    {
                        if (collection.InsertAfterArea.DropArea.Contains(location))
                        {
                            _insertType = DockInsertType.After;
                            _targetGroup = collection.InsertAfterArea.DockGroup;
                            UpdateHighlightForm(collection.InsertAfterArea.HighlightArea);
                            return;
                        }
                    }
                }

                if (!sameGroup)
                {
                    if (collection.DropArea.DropArea.Contains(location))
                    {
                        _insertType = DockInsertType.None;
                        _targetGroup = collection.DropArea.DockGroup;
                        UpdateHighlightForm(collection.DropArea.HighlightArea);
                        return;
                    }
                }
            }

            // Not hovering over anything - hide the highlight
            if (_highlightForm.Visible)
                _highlightForm.Hide();

            // Show we can't drag here
            Cursor.Current = Cursors.No;
        }

        #endregion
    }
}
