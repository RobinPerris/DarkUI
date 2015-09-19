using DarkUI;

namespace Example
{
    public partial class DockProject : DarkToolWindow
    {
        #region Constructor Region

        public DockProject()
        {
            InitializeComponent();

            // Build dummy nodes
            var childCount = 0;
            for (var i = 0; i < 20; i++)
            {
                var node = new DarkTreeNode(string.Format("Root node #{0}", i));
                node.ExpandedIcon = Icons.folder_open;
                node.Icon = Icons.folder_closed;

                for (var x = 0; x < 10; x++)
                {
                    var childNode = new DarkTreeNode(string.Format("Child node #{0}", childCount));
                    childNode.Icon = Icons.files;
                    childCount++;
                    node.Nodes.Add(childNode);
                }

                treeProject.Nodes.Add(node);
            }
        }

        #endregion
    }
}
