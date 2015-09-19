using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    public class DarkDockSplitter
    {
        #region Field Region

        private Control _control;
        private DarkSplitterType _splitterType;

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
