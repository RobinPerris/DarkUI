using DarkUI.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    public class DarkDockSplitter
    {
        #region Field Region

        private readonly Control _parentControl;
        private readonly Control _control;

        private int _minimum;
        private int _maximum;
        private DarkTranslucentForm _overlayForm;

        #endregion

        #region Property Region

        public DarkSplitterMode SplitterMode { get; private set; }
        public DarkSplitterType SplitterType { get; private set; }

        public Rectangle Bounds { get; set; }

        public Cursor ResizeCursor { get; private set; }

        #endregion

        #region Constructor Region

        public DarkDockSplitter(Control parentControl, Control control, DarkSplitterType splitterType, DarkSplitterMode splitterMode)
        {
            _parentControl = parentControl;
            _control = control;
            SplitterType = splitterType;
            SplitterMode = splitterMode;

            switch (SplitterType)
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
            _overlayForm = new DarkTranslucentForm(Color.Black) {Visible = true};

            UpdateOverlay(new Point(0, 0));
        }

        public void HideOverlay()
        {
            _overlayForm.Visible = false;
        }

        public void UpdateOverlay(Point difference)
        {
            var bounds = new Rectangle(Bounds.Location, Bounds.Size);

            switch (SplitterType)
            {
                case DarkSplitterType.Left:
                    var leftX = Math.Max(bounds.Location.X - difference.X, _minimum);

                    if (_maximum != 0 && leftX > _maximum)
                        leftX = _maximum;

                    bounds.Location = new Point(leftX, bounds.Location.Y);
                    break;
                case DarkSplitterType.Right:
                    var rightX = Math.Max(bounds.Location.X - difference.X, _minimum);

                    if (_maximum != 0 && rightX > _maximum)
                        rightX = _maximum;

                    bounds.Location = new Point(rightX, bounds.Location.Y);
                    break;
                case DarkSplitterType.Top:
                    var topY = Math.Max(bounds.Location.Y - difference.Y, _minimum);

                    if (_maximum != 0 && topY > _maximum)
                        topY = _maximum;

                    bounds.Location = new Point(bounds.Location.X, topY);
                    break;
                case DarkSplitterType.Bottom:
                    var bottomY = Math.Max(bounds.Location.Y - difference.Y, _minimum);

                    if (_maximum != 0 && bottomY > _maximum)
                        bottomY = _maximum;

                    bounds.Location = new Point(bounds.Location.X, bottomY);
                    break;
            }

            _overlayForm.Bounds = bounds;
        }

        public void Move(Point difference)
        {
            switch (SplitterType)
            {
                case DarkSplitterType.Left:
                    _control.SetBounds(_control.Bounds.X - difference.X, _control.Bounds.Y, _control.Bounds.Width + difference.X, _control.Bounds.Height);
                    break;
                case DarkSplitterType.Right:
                    _control.SetBounds(_control.Bounds.X, _control.Bounds.Y, _control.Bounds.Width - difference.X, _control.Bounds.Height);
                    break;
                case DarkSplitterType.Top:
                    _control.SetBounds(_control.Bounds.X, _control.Bounds.Y - difference.Y, _control.Bounds.Width, _control.Bounds.Height + difference.Y);
                    break;
                case DarkSplitterType.Bottom:
                    _control.SetBounds(_control.Bounds.X, _control.Bounds.Y, _control.Bounds.Width, _control.Bounds.Height - difference.Y);
                    break;
            }

            UpdateBounds();
        }

        public void UpdateBounds()
        {
            var bounds = _parentControl.RectangleToScreen(_control.Bounds);

            switch (SplitterType)
            {
                case DarkSplitterType.Left:
                    Bounds = new Rectangle(bounds.Left - 2, bounds.Top, 5, bounds.Height);
                    _maximum = bounds.Right - 2 - _control.MinimumSize.Width;
                    break;
                case DarkSplitterType.Right:
                    Bounds = new Rectangle(bounds.Right - 2, bounds.Top, 5, bounds.Height);
                    _minimum = bounds.Left - 2 + _control.MinimumSize.Width;
                    break;
                case DarkSplitterType.Top:
                    Bounds = new Rectangle(bounds.Left, bounds.Top - 2, bounds.Width, 5);
                    _maximum = bounds.Bottom - 2 - _control.MinimumSize.Height;
                    break;
                case DarkSplitterType.Bottom:
                    Bounds = new Rectangle(bounds.Left, bounds.Bottom - 2, bounds.Width, 5);
                    _minimum = bounds.Top - 2 + _control.MinimumSize.Height;
                    break;
            }
        }

        #endregion
    }
}
