using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public enum DarkControlState
    {
        Normal,
        Hover,
        Pressed
    }

    public enum DarkContentAlignment
    {
        Center,
        Left,
        Right
    }

    public class DarkControl : Control
    {
        #region Property Region

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage
        {
            get { return base.BackgroundImage; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
        }

        #endregion
    }
}
