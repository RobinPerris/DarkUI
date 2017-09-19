using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public partial class DarkListBox : ListBox
    {
        private static Color _color = Color.FromArgb(255, 60, 63, 65);
        private static Brush _brush = new SolidBrush(Color.FromArgb(255, 60, 63, 65));

        public DarkListBox()
        {
            InitializeComponent();

            this.BackColor = _color;
            this.ForeColor = Color.White;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 18;
        }

        public DarkListBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            this.BackColor = _color;
            this.ForeColor = Color.White;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 18;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            if (index > Items.Count - 1) return;

            var brush = _brush;
            Rectangle bounds = new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2);
            e.DrawBackground();
            e.Graphics.DrawString(Items[index].ToString(), e.Font, Brushes.White, bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }
    }
}
