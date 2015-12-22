using System.Collections.Generic;
using System.Drawing;

namespace DarkUI.Docking
{
    public class DockPanelState
    {
        #region Property Region

        public List<string> OpenContent { get; set; }

        public Size LeftRegionSize { get; set; }

        public Size RightRegionSize { get; set; }

        public Size BottomRegionSize { get; set; }

        #endregion

        #region Constructor Region

        public DockPanelState()
        {
            OpenContent = new List<string>();
        }

        public DockPanelState(List<string> openContent, Size leftRegionSize, Size rightRegionSize, Size bottomRegionSize)
            : this()
        {
            OpenContent = openContent;
            LeftRegionSize = leftRegionSize;
            RightRegionSize = rightRegionSize;
            BottomRegionSize = bottomRegionSize;
        }

        #endregion
    }
}
