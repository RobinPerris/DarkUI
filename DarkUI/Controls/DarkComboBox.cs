using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;

namespace DarkUI.Controls
{
    public class DarkComboBox : ComboBox
    {
        #region Static
        public Bitmap DefaultButtonIcon { get { return ScrollIcons.scrollbar_arrow_standard; } }
        #endregion

        #region Fields
        /* Buffers */
        private Bitmap _drawBuffer;
        private Bitmap _drawTextBuffer;

        /* Focus */
        private readonly Color _focusColor;
        private readonly Brush _focusBrush;

        /* Background/Foreground */
        private Brush _backBrush;
        private Brush _foreBrush;


        /* Border */
        private Color _borderColor;
        private ButtonBorderStyle _borderStyle;

        /* Button */
        private Rectangle _buttonRect;
        private Color _buttonColor;
        private Brush _buttonBrush;
        private Rectangle _buttonIconRect;
        private Bitmap _buttonIcon;

        /* Text */
        private string _text;
        private Rectangle _textRect;
        private Padding _textPadding;
        #endregion Fields

        #region Constructor
        public DarkComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            DrawMode = DrawMode.OwnerDrawVariable;

            _drawBuffer = null;
            _drawTextBuffer = null;

            FlatStyle = FlatStyle.Flat;
            DropDownStyle = ComboBoxStyle.DropDownList;

            BackColor = Color.FromArgb(69, 73, 74);
            ForeColor = Color.Gainsboro;

            _focusColor = SystemColors.Highlight;
            _focusBrush = new SolidBrush(_focusColor);

            _borderColor = Colors.LightBorder;
            _borderStyle = ButtonBorderStyle.Solid;

            _buttonColor = Colors.DarkBackground;
            _buttonIcon = DefaultButtonIcon;
            
            _textPadding = new Padding(2);
        }
        #endregion Constructor

        #region Properties
        [Category("Appearance")]
        public sealed override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Bitmap ButtonIcon
        {
            get { return _buttonIcon; }
            set
            {
                _buttonIcon = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool DrawDropdownHoverOutline { get; set; }

        [Category("Appearance")]
        public bool DrawFocusRectangle { get; set; }

        [Category("Appearance")]
        public Padding TextPadding
        {
            get { return _textPadding; }
            set
            {
                _textPadding = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public override string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Invalidate();
            }
        }
        #endregion Properties

        #region Methods
        public new void Invalidate()
        {
            base.Invalidate();
        }
        #endregion Methods

        #region On Events

        #region Lifecycle Events
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            _foreBrush = new SolidBrush(ForeColor);
            _backBrush = new SolidBrush(BackColor);
            _buttonBrush = new SolidBrush(_buttonColor);

            _buttonRect = new Rectangle(ClientRectangle.Width - (SystemInformation.VerticalScrollBarWidth + 1), 0, SystemInformation.VerticalScrollBarWidth + 1, ClientRectangle.Height);
            _buttonIconRect = new Rectangle(_buttonRect.Left + (_buttonRect.Width - _buttonIcon.Width) / 2, _buttonRect.Top + (_buttonRect.Height - _buttonIcon.Height) / 2, _buttonIcon.Width, _buttonIcon.Height);

            _textRect = new Rectangle(1 + _textPadding.Left, 1 + _textPadding.Top, ClientRectangle.Width - (2 + _buttonRect.Width + _textPadding.Horizontal), ClientRectangle.Height - (2 + _textPadding.Vertical));

            _drawBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            _drawBuffer.MakeTransparent(Color.Transparent);

            using (var bufferGraphics = Graphics.FromImage(_drawBuffer))
            {
                bufferGraphics.Clear(BackColor);
                bufferGraphics.FillRectangle(_buttonBrush, _buttonRect);
                bufferGraphics.DrawImage(_buttonIcon, _buttonIconRect);
                ControlPaint.DrawBorder(bufferGraphics, ClientRectangle, _borderColor, ButtonBorderStyle.Solid);
            }

            _drawTextBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            _drawTextBuffer.MakeTransparent(Color.Transparent);
            using (var graphics = Graphics.FromImage(_drawTextBuffer))
            {
                graphics.Clear(Color.Transparent);
                graphics.FillRectangle((Focused && DrawFocusRectangle) ? _focusBrush : _backBrush, _textRect);
                graphics.DrawString(SelectedItem?.ToString() ?? Text, Font, _foreBrush, _textRect, StringFormat.GenericDefault);
            }

            base.OnInvalidated(e);
        }
        #endregion

        #region Data Events
        protected override void OnTextChanged(EventArgs e)
        {
            Invalidate();

            base.OnTextChanged(e);
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            Invalidate();

            base.OnTextUpdate(e);
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            Invalidate();

            base.OnSelectedValueChanged(e);
        }
        #endregion

        #region Drawing Events
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!DroppedDown && !DrawDropdownHoverOutline)
            {
                _backBrush = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(_backBrush,e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }

            if (Items.Count <= e.Index || e.Index <= -1)
                return;
            
            e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, _foreBrush, e.Bounds, StringFormat.GenericDefault);

            if (DrawDropdownHoverOutline)
            {
                e.DrawFocusRectangle();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(_drawBuffer, Point.Empty);
            e.Graphics.DrawImage(_drawTextBuffer, Point.Empty);
            if (DesignMode && Items.Count > 0 && Text != Items[0].ToString()) Text = Items[0].ToString();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            ButtonIcon = ScrollIcons.scrollbar_arrow_clicked;
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            ButtonIcon = ScrollIcons.scrollbar_arrow_standard;
        }
        #endregion Drawing

        #endregion On Events
    }
}
