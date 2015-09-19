using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    public class DarkDockResizeFilter : IMessageFilter
    {
        #region Field Region

        private DarkDockPanel _dockPanel;

        private bool _isDragging;
        private DarkDockSplitter _activeSplitter;

        #endregion

        #region Constructor Region

        public DarkDockResizeFilter(DarkDockPanel dockPanel)
        {
            _dockPanel = dockPanel;
        }

        #endregion

        #region IMessageFilter Region

        public bool PreFilterMessage(ref Message m)
        {
            // We only care about mouse events
            if (!(m.Msg == (int)WM.MOUSEMOVE ||
                  m.Msg == (int)WM.LBUTTONDOWN || m.Msg == (int)WM.LBUTTONUP || m.Msg == (int)WM.LBUTTONDBLCLK ||
                  m.Msg == (int)WM.RBUTTONDOWN || m.Msg == (int)WM.RBUTTONUP || m.Msg == (int)WM.RBUTTONDBLCLK))
                return false;

            // Exit out early if we're simply releasing a drag over the area
            if (m.Msg == (int)WM.LBUTTONUP && !_isDragging)
                return false;

            // Force cursor if already dragging.
            ForceDraggingCursor();

            // Stop all events from going through if we're dragging a splitter.
            if (_isDragging)
                return true;

            // Return out early if we're dragging something that's not a splitter.
            if (m.Msg == (int)WM.MOUSEMOVE && !_isDragging && _dockPanel.MouseButtonState != MouseButtons.None)
                return false;

            // Try and create a control from the message handle.
            var control = Control.FromHandle(m.HWnd);

            // Exit out if we didn't manage to create a control.
            if (control == null)
                return false;

            // Exit out if the control is not the dock panel or a child control.
            if (!(control == _dockPanel || _dockPanel.Contains(control)))
                return false;

            // Update the mouse cursor
            CheckCursor();

            // Start drag.
            if (m.Msg == (int)WM.LBUTTONDOWN)
            {
            }

            // Stop drag.
            if (m.Msg == (int)WM.LBUTTONUP)
            {
            }

            // Stop events passing through if we just started to drag something
            if (_isDragging)
            return true;

            // Stop events passing through if we're hovering over a splitter
            foreach (var splitter in _dockPanel.Splitters)
            {
                if (splitter.Bounds.Contains(_dockPanel.PointToClient(Cursor.Position)))
                    return true;
            }

            return false;
        }

        #endregion

        #region Method Region

        private void ForceDraggingCursor()
        {
            if (_isDragging)
            {
                SetCursor(_activeSplitter.ResizeCursor);
                return;
            }
        }

        private void ResetCursor()
        {
            Cursor.Current = Cursors.Default;
            CheckCursor();
        }

        private void CheckCursor()
        {
            if (_isDragging)
                return;

            foreach (var splitter in _dockPanel.Splitters)
            {
                if (splitter.Bounds.Contains(_dockPanel.PointToClient(Cursor.Position)))
                {
                    SetCursor(splitter.ResizeCursor);
                    return;
                }
            }
        }

        private void SetCursor(Cursor cursor)
        {
            Cursor.Current = cursor;
        }

        #endregion
    }
}