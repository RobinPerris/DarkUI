using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI
{
    public class DarkLabel : Label
    {
        #region Field Region

        private bool _autoUpdateHeight;
        private bool _isGrowing;

        #endregion

        #region Property Region

        [Category("Appearance")]
        [Description("Determines whether the label will automatically update height based on content.")]
        [DefaultValue(false)]
        public bool AutoUpdateHeight
        {
            get { return _autoUpdateHeight; }
            set
            {
                _autoUpdateHeight = value;

                if (_autoUpdateHeight)
                {
                    AutoSize = false;
                    ResizeLabel();
                }
            }
        }

        #endregion

        #region Constructor Region

        public DarkLabel()
        {
            ForeColor = Colors.LightText;
        }

        #endregion

        #region Method Region

        private void ResizeLabel()
        {
            if (!_autoUpdateHeight || _isGrowing)
                return;

            try
            {
                _isGrowing = true;
                var sz = new Size(Width, int.MaxValue);
                sz = TextRenderer.MeasureText(Text, Font, sz, TextFormatFlags.WordBreak);
                Height = sz.Height + Padding.Vertical;
            }
            finally
            {
                _isGrowing = false;
            }
        }

        #endregion

        #region Event Handler Region

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            ResizeLabel();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ResizeLabel();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ResizeLabel();
        }

        #endregion
    }
}
