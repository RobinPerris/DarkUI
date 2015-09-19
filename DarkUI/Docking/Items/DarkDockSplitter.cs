using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    public class DarkDockSplitter
    {
        #region Field Region

        private Control _control;
        private DarkSplitterType _splitterType;
        private DarkTranslucentForm _overlayForm;

        #endregion

        #region Property Region

        public Rectangle Bounds { get; set; }

        public Cursor ResizeCursor { get; private set; }

        #endregion

        #region Constructor Region

        public DarkDockSplitter(Control control, DarkSplitterType splitterType)
        {
            _control = control;
            _splitterType = splitterType;
            _overlayForm = new DarkTranslucentForm(Color.Black);

            switch (_splitterType)
            {
                case DarkSplitterType.Left:
                case DarkSplitterType.Right:
                    ResizeCursor = Cursors.SizeWE;
                    break;
                case DarkSplitterType.Top:
                case DarkSplitterType.Bottom:
                    ResizeCursor = Cursors.SizeNS;
                    break;
            }
        }

        #endregion

        #region Method Region

        public void ShowOverlay()
        {
            _overlayForm.Show();
            UpdateOverlay(new Point(0, 0));
        }

        public void HideOverlay()
        {
            _overlayForm.Hide();
        }

        public void UpdateOverlay(Point difference)
        {
            var bounds = _control.RectangleToScreen(Bounds);

            switch (_splitterType)
            {
                case DarkSplitterType.Left:
                    bounds.Location = new Point(bounds.Location.X - difference.X, bounds.Location.Y);
                    break;
                case DarkSplitterType.Right:
                    bounds.Location = new Point(bounds.Location.X - difference.X, bounds.Location.Y);
                    break;
                case DarkSplitterType.Top:
                    bounds.Location = new Point(bounds.Location.X, bounds.Location.Y - difference.Y);
                    break;
                case DarkSplitterType.Bottom:
                    bounds.Location = new Point(bounds.Location.X, bounds.Location.Y - difference.Y);
                    break;
            }

            _overlayForm.Location = bounds.Location;
            _overlayForm.Size = bounds.Size;
        }

        public void Move(Point difference)
        {
            switch (_splitterType)
            {
                case DarkSplitterType.Left:
                    _control.Width += difference.X;
                    break;
                case DarkSplitterType.Right:
                    _control.Width -= difference.X;
                    break;
                case DarkSplitterType.Top:
                    _control.Height += difference.Y;
                    break;
                case DarkSplitterType.Bottom:
                    _control.Height -= difference.Y;
                    break;
            }
        }

        public void UpdateBounds()
        {
            switch (_splitterType)
            {
                case DarkSplitterType.Left:
                    Bounds = new Rectangle(_control.Left - 2, _control.Top, 5, _control.Height);
                    break;
                case DarkSplitterType.Right:
                    Bounds = new Rectangle(_control.Right - 3, _control.Top, 5, _control.Height);
                    break;
                case DarkSplitterType.Top:
                    Bounds = new Rectangle(_control.Left, _control.Top - 2, _control.Width, 5);
                    break;
                case DarkSplitterType.Bottom:
                    Bounds = new Rectangle(_control.Left, _control.Bottom - 5, _control.Width, 5);
                    break;
            }
        }

        #endregion
    }
}
