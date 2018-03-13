using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DarkUI.Docking
{
    public class DockGroupState : IEquatable<DockGroupState>
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

        #region Comparison Region

        public bool Equals(DockGroupState other) =>
            VisibleContent == other.VisibleContent &&
            Order == other.Order &&
            Size == other.Size &&
            Contents.SequenceEqual(other.Contents);
        public static bool operator ==(DockGroupState first, DockGroupState second) => first.Equals(second);
        public static bool operator !=(DockGroupState first, DockGroupState second) => !first.Equals(second);
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as DockGroupState);

        #endregion
    }
}
