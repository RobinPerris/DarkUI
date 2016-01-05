using System.Drawing;

namespace DarkUI.Docking
{
    internal class DockDropArea
    {
        #region Property Region

        internal DarkDockPanel DockPanel { get; private set; }

        internal Rectangle DropArea { get; private set; }

        internal Rectangle HighlightArea { get; private set; }

        internal DarkDockRegion DockRegion { get; private set; }

        internal DarkDockGroup DockGroup { get; private set; }

        internal DockInsertType InsertType { get; private set; }

        #endregion

        #region Constructor Region

        internal DockDropArea(DarkDockRegion region, DockInsertType insertType)
        {
            DockRegion = region;
            InsertType = insertType;
        }

        internal DockDropArea(DarkDockGroup group, DockInsertType insertType)
        {
            DockGroup = group;
            InsertType = insertType;
        }

        #endregion

        #region Method Region

        internal void BuildAreas()
        {
            if (DockRegion != null)
                BuildRegionAreas();
            else if (DockGroup != null)
                BuildGroupAreas();
        }

        private void BuildRegionAreas()
        {

        }

        private void BuildGroupAreas()
        {
            switch (InsertType)
            {
                case DockInsertType.None:
                    var dropRect = new Rectangle
                    {
                        X = DockGroup.PointToScreen(Point.Empty).X,
                        Y = DockGroup.PointToScreen(Point.Empty).Y,
                        Width = DockGroup.Width,
                        Height = DockGroup.Height
                    };

                    DropArea = dropRect;
                    HighlightArea = dropRect;

                    break;

                case DockInsertType.Before:
                    var beforeDropWidth = DockGroup.Width;
                    var beforeDropHeight = DockGroup.Height;

                    switch (DockGroup.DockArea)
                    {
                        case DarkDockArea.Left:
                        case DarkDockArea.Right:
                            beforeDropHeight = DockGroup.Height / 4;
                            break;

                        case DarkDockArea.Bottom:
                            beforeDropWidth = DockGroup.Width / 4;
                            break;
                    }

                    var beforeDropRect = new Rectangle
                    {
                        X = DockGroup.PointToScreen(Point.Empty).X,
                        Y = DockGroup.PointToScreen(Point.Empty).Y,
                        Width = beforeDropWidth,
                        Height = beforeDropHeight
                    };

                    DropArea = beforeDropRect;
                    HighlightArea = beforeDropRect;

                    break;

                case DockInsertType.After:
                    var afterDropX = DockGroup.PointToScreen(Point.Empty).X;
                    var afterDropY = DockGroup.PointToScreen(Point.Empty).Y;
                    var afterDropWidth = DockGroup.Width;
                    var afterDropHeight = DockGroup.Height;

                    switch (DockGroup.DockArea)
                    {
                        case DarkDockArea.Left:
                        case DarkDockArea.Right:
                            afterDropHeight = DockGroup.Height / 4;
                            afterDropY = DockGroup.PointToScreen(Point.Empty).Y + DockGroup.Height - afterDropHeight;
                            break;

                        case DarkDockArea.Bottom:
                            afterDropWidth = DockGroup.Width / 4;
                            afterDropX = DockGroup.PointToScreen(Point.Empty).X + DockGroup.Width - afterDropWidth;
                            break;
                    }

                    var afterDropRect = new Rectangle
                    {
                        X = afterDropX,
                        Y = afterDropY,
                        Width = afterDropWidth,
                        Height = afterDropHeight
                    };

                    DropArea = afterDropRect;
                    HighlightArea = afterDropRect;

                    break;
            }
        }

        #endregion
    }
}
