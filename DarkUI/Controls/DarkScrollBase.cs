using DarkUI.Config;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public abstract class DarkScrollBase : Control
    {
        #region Event Region

        public event EventHandler ViewportChanged;
        public event EventHandler ContentSizeChanged;

        #endregion

        #region Field Region

        protected readonly DarkScrollBar VScrollBar;
        protected readonly DarkScrollBar HScrollBar;

        private Size _visibleSize;
        private Size _contentSize;

        private Rectangle _viewport;

        private int _maxDragChange;
        private readonly Timer _dragTimer;

        private bool _hideScrollBars = true;

        #endregion

        #region Property Region


        [DefaultValue(0.42f)]
        public float MouseWheelScrollSpeedV { get; set; } = 0.42f;

        [DefaultValue(0.42f)]
        public float MouseWheelScrollSpeedH { get; set; } = 0.42f;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Viewport
        {
            get { return _viewport; }
            private set
            {
                _viewport = value;

                ViewportChanged?.Invoke(this, null);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size ContentSize
        {
            get { return _contentSize; }
            set
            {
                _contentSize = value;
                UpdateScrollBars();

                ContentSizeChanged?.Invoke(this, null);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point OffsetMousePosition { get; private set; }

        [Category("Behavior")]
        [Description("Determines the maximum scroll change when dragging.")]
        [DefaultValue(0)]
        public int MaxDragChange
        {
            get { return _maxDragChange; }
            set
            {
                _maxDragChange = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragging { get; private set; }

        [Category("Behavior")]
        [Description("Determines whether scrollbars will remain visible when disabled.")]
        [DefaultValue(true)]
        public bool HideScrollBars
        {
            get { return _hideScrollBars; }
            set
            {
                _hideScrollBars = value;
                UpdateScrollBars();
            }
        }

        #endregion

        #region Constructor Region

        protected DarkScrollBase()
        {
            SetStyle(ControlStyles.Selectable |
                     ControlStyles.UserMouse, true);

            VScrollBar = new DarkScrollBar { ScrollOrientation = DarkScrollOrientation.Vertical };
            HScrollBar = new DarkScrollBar { ScrollOrientation = DarkScrollOrientation.Horizontal };

            Controls.Add(VScrollBar);
            Controls.Add(HScrollBar);

            VScrollBar.ValueChanged += delegate { UpdateViewport(); };
            HScrollBar.ValueChanged += delegate { UpdateViewport(); };

            VScrollBar.MouseDown += delegate { Select(); };
            HScrollBar.MouseDown += delegate { Select(); };

            _dragTimer = new Timer {Interval = 1};
            _dragTimer.Tick += DragTimer_Tick;
        }

        #endregion

        #region Method Region

        private void UpdateScrollBars()
        {
            if (VScrollBar.Maximum != ContentSize.Height)
                VScrollBar.Maximum = ContentSize.Height;

            if (HScrollBar.Maximum != ContentSize.Width)
                HScrollBar.Maximum = ContentSize.Width;

            const int scrollSize = Consts.ScrollBarSize;

            VScrollBar.Location = new Point(ClientSize.Width - scrollSize, 0);
            VScrollBar.Size = new Size(scrollSize, ClientSize.Height);

            HScrollBar.Location = new Point(0, ClientSize.Height - scrollSize);
            HScrollBar.Size = new Size(ClientSize.Width, scrollSize);

            if (DesignMode)
                return;

            // Do this twice in case changing the visibility of the scrollbars
            // causes the VisibleSize to change in such a way as to require a second scrollbar.
            // Probably a better way to detect that scenario...
            SetVisibleSize();
            SetScrollBarVisibility();
            SetVisibleSize();
            SetScrollBarVisibility();

            if (VScrollBar.Visible)
                HScrollBar.Width -= scrollSize;

            if (HScrollBar.Visible)
                VScrollBar.Height -= scrollSize;

            VScrollBar.ViewSize = _visibleSize.Height;
            HScrollBar.ViewSize = _visibleSize.Width;

            UpdateViewport();
        }

        private void SetScrollBarVisibility()
        {
            VScrollBar.Enabled = _visibleSize.Height < ContentSize.Height;
            HScrollBar.Enabled = _visibleSize.Width < ContentSize.Width;

            if (_hideScrollBars)
            {
                VScrollBar.Visible = VScrollBar.Enabled;
                HScrollBar.Visible = HScrollBar.Enabled;
            }
        }

        private void SetVisibleSize()
        {
            const int scrollSize = Consts.ScrollBarSize;

            _visibleSize = new Size(ClientSize.Width, ClientSize.Height);

            if (VScrollBar.Visible)
                _visibleSize.Width -= scrollSize;

            if (HScrollBar.Visible)
                _visibleSize.Height -= scrollSize;
        }

        private void UpdateViewport()
        {
            var left = 0;
            var top = 0;
            var width = ClientSize.Width;
            var height = ClientSize.Height;

            if (HScrollBar.Visible)
            {
                left = HScrollBar.Value;
                height -= HScrollBar.Height;
            }

            if (VScrollBar.Visible)
            {
                top = VScrollBar.Value;
                width -= VScrollBar.Width;
            }

            Viewport = new Rectangle(left, top, width, height);

            var pos = PointToClient(MousePosition);
            OffsetMousePosition = new Point(pos.X + Viewport.Left, pos.Y + Viewport.Top);

            Invalidate();
        }

        public void ScrollTo(Point point)
        {
            HScrollTo(point.X);
            VScrollTo(point.Y);
        }

        public void VScrollTo(int value)
        {
            if (VScrollBar.Visible)
                VScrollBar.Value = value;
        }

        public void HScrollTo(int value)
        {
            if (HScrollBar.Visible)
                HScrollBar.Value = value;
        }

        protected virtual void StartDrag()
        {
            IsDragging = true;
            _dragTimer.Start();
        }

        protected virtual void StopDrag()
        {
            IsDragging = false;
            _dragTimer.Stop();
        }

        public Point PointToView(Point point)
        {
            return new Point(point.X - Viewport.Left, point.Y - Viewport.Top);
        }

        public Rectangle RectangleToView(Rectangle rect)
        {
            return new Rectangle(new Point(rect.Left - Viewport.Left, rect.Top - Viewport.Top), rect.Size);
        }

        #endregion

        #region Event Handler Region

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            UpdateScrollBars();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateScrollBars();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            OffsetMousePosition = new Point(e.X + Viewport.Left, e.Y + Viewport.Top);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right)
                Select();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // ReSharper disable once ReplaceWithSingleAssignment.False
            var horizontal = false;

            if (HScrollBar.Visible && ModifierKeys == Keys.Control)
                horizontal = true;

            if (HScrollBar.Visible && !VScrollBar.Visible)
                horizontal = true;

            if (!horizontal)
            {
                float speed = MouseWheelScrollSpeedV * e.Delta;
                int speedInt = (int)Math.Min(1073741824, Math.Max(-1073741824, speed));
                if (speedInt == 0)
                    speedInt = speed > 0 ? 1 : -1;
                VScrollBar.ScrollByPhysical(speedInt);
            }
            else
            {
                float speed = MouseWheelScrollSpeedH * e.Delta;
                int speedInt = (int)Math.Min(1073741824, Math.Max(-1073741824, speed));
                if (speedInt == 0)
                    speedInt = speed > 0 ? 1 : -1;
                HScrollBar.ScrollByPhysical(speedInt);
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            // Allows arrow keys to trigger OnKeyPress
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void DragTimer_Tick(object sender, EventArgs e)
        {
            var pos = PointToClient(MousePosition);

            var right = ClientRectangle.Right;
            var bottom = ClientRectangle.Bottom;

            if (VScrollBar.Visible)
                right = VScrollBar.Left;

            if (HScrollBar.Visible)
                bottom = HScrollBar.Top;

            if (VScrollBar.Visible)
            {
                // Scroll up
                if (pos.Y < ClientRectangle.Top)
                {
                    var difference = (pos.Y - ClientRectangle.Top) * -1;

                    if (MaxDragChange > 0 && difference > MaxDragChange)
                        difference = MaxDragChange;

                    VScrollBar.Value = VScrollBar.Value - difference;
                }

                // Scroll down
                if (pos.Y > bottom)
                {
                    var difference = pos.Y - bottom;

                    if (MaxDragChange > 0 && difference > MaxDragChange)
                        difference = MaxDragChange;

                    VScrollBar.Value = VScrollBar.Value + difference;
                }
            }

            if (HScrollBar.Visible)
            {
                // Scroll left
                if (pos.X < ClientRectangle.Left)
                {
                    var difference = (pos.X - ClientRectangle.Left) * -1;

                    if (MaxDragChange > 0 && difference > MaxDragChange)
                        difference = MaxDragChange;

                    HScrollBar.Value = HScrollBar.Value - difference;
                }

                // Scroll right
                if (pos.X > right)
                {
                    var difference = pos.X - right;

                    if (MaxDragChange > 0 && difference > MaxDragChange)
                        difference = MaxDragChange;

                    HScrollBar.Value = HScrollBar.Value + difference;
                }
            }
        }

        #endregion
    }
}
