using DarkUI.Config;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    [ToolboxItem(false)]
    public class DarkDockGroup : Panel
    {
        #region Field Region

        private List<DarkDockContent> _contents;

        #endregion

        #region Property Region

        public DarkDockPanel DockPanel { get; private set; }

        public DarkDockRegion DockRegion { get; private set; }

        public DarkDockArea DockArea { get; private set; }

        public int ContentCount
        {
            get
            {
                return _contents.Count;
            }
        }

        #endregion

        #region Constructor Region

        public DarkDockGroup(DarkDockPanel dockPanel, DarkDockRegion dockRegion)
        {
            _contents = new List<DarkDockContent>();

            DockPanel = dockPanel;
            DockRegion = dockRegion;
            DockArea = dockRegion.DockArea;
        }

        #endregion

        #region Method Region

        public void AddContent(DarkDockContent dockContent)
        {
            dockContent.DockGroup = this;
            dockContent.Dock = DockStyle.Fill;

            _contents.Add(dockContent);
            Controls.Add(dockContent);
        }

        public void RemoveContent(DarkDockContent dockContent)
        {
            dockContent.DockGroup = null;

            _contents.Remove(dockContent);
            Controls.Remove(dockContent);
        }

        #endregion
    }
}
