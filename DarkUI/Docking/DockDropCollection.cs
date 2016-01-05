namespace DarkUI.Docking
{
    internal class DockDropCollection
    {
        #region Property Region

        internal DockDropArea DropArea { get; private set; }

        internal DockDropArea InsertBeforeArea { get; private set; }

        internal DockDropArea InsertAfterArea { get; private set; }

        #endregion

        #region Constructor Region

        internal DockDropCollection(DarkDockPanel dockPanel, DarkDockRegion region)
        {
            DropArea = new DockDropArea(region, DockInsertType.None);
            InsertBeforeArea = new DockDropArea(region, DockInsertType.Before);
            InsertAfterArea = new DockDropArea(region, DockInsertType.After);

            BuildAreas();
        }

        internal DockDropCollection(DarkDockPanel dockPanel, DarkDockGroup group)
        {
            DropArea = new DockDropArea(group, DockInsertType.None);
            InsertBeforeArea = new DockDropArea(group, DockInsertType.Before);
            InsertAfterArea = new DockDropArea(group, DockInsertType.After);

            BuildAreas();
        }

        #endregion

        #region Method Region

        private void BuildAreas()
        {
            DropArea.BuildAreas();
            InsertBeforeArea.BuildAreas();
            InsertAfterArea.BuildAreas();
        }

        #endregion
    }
}
