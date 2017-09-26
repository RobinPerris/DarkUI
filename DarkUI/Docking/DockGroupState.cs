using System.Collections.Generic;
using System.Drawing;

namespace DarkUI.Docking
{
    public class DockGroupState
    {
        #region Property Region

        public List<string> Contents { get; set; }

        public string VisibleContent { get; set; }

        public int Order { get; set; }

        public Size Size { get; set; }

        #endregion

        #region Constructor Region

        public DockGroupState()
        {
            Contents = new List<string>();
            Order = 0;
            Size = new Size(100, 100);
        }

        #endregion
    }
}
