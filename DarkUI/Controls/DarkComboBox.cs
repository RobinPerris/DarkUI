using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Icons;
using System.Diagnostics;

namespace DarkUI.Controls
{
    public class DarkComboBox : ComboBox
    {
        #region Static
        public Bitmap DefaultButtonIcon { get { return ScrollIcons.scrollbar_arrow_standard; } }
        #endregion

        #region Fields
        /* Buffers */
        private Bitmap mDrawBuffer;
        private Bitmap mDrawTextBuffer;

        /* Focus */
        private Color mFocusColor;
        private Brush mFocusBrush;

        /* Background/Foreground */
        private Brush mBackBrush;
        private Brush mForeBrush;


        /* Border */
        private Color mBorderColor;
        private ButtonBorderStyle mBorderStyle;

        /* Button */
        private Rectangle mButtonRect;
        private Color mButtonColor;
        private Brush mButtonBrush;
        private Rectangle mButtonIconRect;
        private Bitmap mButtonIcon;

        /* Text */
        private string mText;
        private Rectangle mTextRect;
        private Padding mTextPadding;
        #endregion Fields

        #region Constructor
        public DarkComboBox() : base()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.DrawMode = DrawMode.OwnerDrawVariable;

            this.mDrawBuffer = null;
            this.mDrawTextBuffer = null;

            this.FlatStyle = FlatStyle.Flat;
            this.DropDownStyle = ComboBoxStyle.DropDownList;

            this.BackColor = Color.FromArgb(69, 73, 74);
            this.ForeColor = Color.Gainsboro;

            this.mFocusColor = SystemColors.Highlight;
            this.mFocusBrush = new SolidBrush(this.mFocusColor);

            this.mBorderColor = Colors.LightBorder;
            this.mBorderStyle = ButtonBorderStyle.Solid;

            this.mButtonColor = Colors.DarkBackground;
            this.mButtonIcon = DefaultButtonIcon;
            
            this.mTextPadding = new Padding(2);
        }
        #endregion Constructor

        #region Properties
        [Category("Appearance")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public override Color BackColor
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
            get { return mButtonColor; }
            set
            {
                mButtonColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Bitmap ButtonIcon
        {
            get { return mButtonIcon; }
            set
            {
                mButtonIcon = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return mBorderColor; }
            set
            {
                mBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return mBorderStyle; }
            set
            {
                mBorderStyle = value;
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
            get { return mTextPadding; }
            set
            {
                mTextPadding = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public override string Text
        {
            get { return mText; }
            set
            {
                mText = value;
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
            mForeBrush = new SolidBrush(ForeColor);
            mBackBrush = new SolidBrush(BackColor);
            mButtonBrush = new SolidBrush(mButtonColor);

            mButtonRect = new Rectangle(ClientRectangle.Width - (SystemInformation.VerticalScrollBarWidth + 1), 0, SystemInformation.VerticalScrollBarWidth + 1, ClientRectangle.Height);
            mButtonIconRect = new Rectangle(mButtonRect.Left + (mButtonRect.Width - mButtonIcon.Width) / 2, mButtonRect.Top + (mButtonRect.Height - mButtonIcon.Height) / 2, mButtonIcon.Width, mButtonIcon.Height);

            mTextRect = new Rectangle(1 + mTextPadding.Left, 1 + mTextPadding.Top, ClientRectangle.Width - (2 + mButtonRect.Width + mTextPadding.Horizontal), ClientRectangle.Height - (2 + mTextPadding.Vertical));

            mDrawBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            mDrawBuffer.MakeTransparent(Color.Transparent);

            using (Graphics bufferGraphics = Graphics.FromImage(mDrawBuffer))
            {
                bufferGraphics.Clear(BackColor);
                bufferGraphics.FillRectangle(mButtonBrush, mButtonRect);
                bufferGraphics.DrawImage(mButtonIcon, mButtonIconRect);
                ControlPaint.DrawBorder(bufferGraphics, ClientRectangle, mBorderColor, ButtonBorderStyle.Solid);
            }

            mDrawTextBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            mDrawTextBuffer.MakeTransparent(Color.Transparent);
            using (Graphics graphics = Graphics.FromImage(mDrawTextBuffer))
            {
                graphics.Clear(Color.Transparent);
                graphics.FillRectangle((Focused && DrawFocusRectangle) ? mFocusBrush : mBackBrush, mTextRect);
                graphics.DrawString((SelectedItem != null) ? SelectedItem.ToString() : Text, Font, mForeBrush, mTextRect, StringFormat.GenericDefault);
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
                mBackBrush = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(mBackBrush,e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }

            if (Items.Count > e.Index && e.Index > -1)
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, mForeBrush, e.Bounds, StringFormat.GenericDefault);


                if (DrawDropdownHoverOutline)
                {
                    e.DrawFocusRectangle();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(mDrawBuffer, Point.Empty);
            e.Graphics.DrawImage(mDrawTextBuffer, Point.Empty);
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
