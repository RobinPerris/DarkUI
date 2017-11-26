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
        public static Bitmap DefaultButtonIcon { get { return ScrollIcons.scrollbar_arrow_standard; } }
        #endregion

        #region Fields
        // Visual look
        private static readonly Brush _focusBrush = new SolidBrush(SystemColors.Highlight);
        private Color _borderColor = Colors.LightBorder;
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private Color _buttonColor = Colors.DarkBackground;
        private Bitmap _buttonIcon = DefaultButtonIcon;

        // Text
        private string _text;
        private Padding _textPadding = new Padding(2);
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

            FlatStyle = FlatStyle.Flat;
            DropDownStyle = ComboBoxStyle.DropDownList;

            BackColor = Colors.LightBackground;
            ForeColor = Colors.LightText;
        }
        #endregion Constructor

        #region Properties
        [DefaultValue(DrawMode.OwnerDrawVariable)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        [DefaultValue(FlatStyle.Flat)]
        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        [DefaultValue(ComboBoxStyle.DropDownList)]
        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set { base.DropDownStyle = value; }
        }

        [Category("Appearance")]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [ReadOnly(true)]
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
        [ReadOnly(true)]
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
        [ReadOnly(true)]
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
        [DefaultValue(ButtonBorderStyle.Solid)]
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
        [DefaultValue(false)]
        public bool DrawDropdownHoverOutline { get; set; }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool DrawFocusRectangle { get; set; }

        [Category("Appearance")]
        [ReadOnly(true)]
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
                using (var backBrush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(backBrush, e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }

            if (Items.Count <= e.Index || e.Index <= -1)
                return;

            using (var foreBrush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, foreBrush, e.Bounds, StringFormat.GenericDefault);

            if (DrawDropdownHoverOutline)
            {
                e.DrawFocusRectangle();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle buttonRect = new Rectangle(ClientRectangle.Width - (SystemInformation.VerticalScrollBarWidth + 1), 0, SystemInformation.VerticalScrollBarWidth + 1, ClientRectangle.Height);
            Rectangle buttonIconRect = new Rectangle(buttonRect.Left + (buttonRect.Width - _buttonIcon.Width) / 2, buttonRect.Top + (buttonRect.Height - _buttonIcon.Height) / 2, _buttonIcon.Width, _buttonIcon.Height);
            Rectangle textRect = new Rectangle(1 + _textPadding.Left, 1 + _textPadding.Top, ClientRectangle.Width - (2 + buttonRect.Width + _textPadding.Horizontal), ClientRectangle.Height - (2 + _textPadding.Vertical));

            // Draw background
            using (var buttonBrush = new SolidBrush(_buttonColor))
                e.Graphics.FillRectangle(buttonBrush, buttonRect);
            e.Graphics.DrawImage(_buttonIcon, buttonIconRect);
            ControlPaint.DrawBorder(e.Graphics, buttonRect, _borderColor, ButtonBorderStyle.Solid);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, _borderColor, ButtonBorderStyle.Solid);

            // Draw text
            using (var backBrush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle((Focused && DrawFocusRectangle) ? _focusBrush : backBrush, textRect);
            using (var foreBrush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(SelectedItem?.ToString() ?? Text, Font, foreBrush, textRect, StringFormat.GenericDefault);
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
