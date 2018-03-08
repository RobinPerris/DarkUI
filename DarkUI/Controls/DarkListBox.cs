using DarkUI.Config;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkListBox : ListBox
    {
        public DarkListBox()
        {
            BackColor = Colors.LightBackground;
            ForeColor = Colors.LightText;
            Padding = new Padding(2, 2, 2, 2);
            BorderStyle = BorderStyle.FixedSingle;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 18;
        }

        public DarkListBox(IContainer container)
        {
            container.Add(this);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            if (index > Items.Count - 1) return;

            Rectangle bounds = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            // Background
            var odd = e.Index % 2 != 0;
            var bgColor = !odd ? Colors.HeaderBackground : Colors.GreyBackground;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                bgColor = Focused ? Colors.BlueSelection : Colors.GreySelection;

            using (var b = new SolidBrush(bgColor))
                e.Graphics.FillRectangle(b, bounds);

            var formatE = new ListControlConvertEventArgs(null, typeof(string), Items[e.Index]);
            OnFormat(formatE);
            string text = formatE.Value?.ToString() ?? Items[e.Index].ToString();
            e.Graphics.DrawString(text, e.Font, Brushes.White, bounds, StringFormat.GenericDefault);
        }
    }
}
