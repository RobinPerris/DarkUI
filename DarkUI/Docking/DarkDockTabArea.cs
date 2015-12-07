using DarkUI.Config;
using DarkUI.Controls;
using System.Collections.Generic;
using System.Drawing;

namespace DarkUI.Docking
{
    internal class DarkDockTabArea
    {
        #region Field Region

        private Dictionary<DarkDockContent, DarkDockTab> _tabs = new Dictionary<DarkDockContent, DarkDockTab>();

        private DarkContextMenu _tabMenu = new DarkContextMenu();

        #endregion

        #region Property Region

        public DarkDockArea DockArea { get; private set; }

        public Rectangle ClientRectangle { get; set; }

        public Rectangle DropdownRectangle { get; set; }

        public bool DropdownHot { get; set; }

        public int Offset { get; set; }

        public int TotalTabSize { get; set; }

        public bool Visible { get; set; }

        public DarkContextMenu TabMenu { get { return _tabMenu; } }

        public DarkDockTab ClickedCloseButton { get; set; }

        #endregion
        
        #region Constructor Region

        public DarkDockTabArea(DarkDockArea dockArea)
        {
            DockArea = dockArea;
        }

        #endregion
    }
}
