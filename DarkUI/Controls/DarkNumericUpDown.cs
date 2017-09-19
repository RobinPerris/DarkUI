using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public partial class DarkNumericUpDown : NumericUpDown
    {
        private static Color _color = Color.FromArgb(255, 60, 63, 65);
        private static Brush _brush = new SolidBrush(Color.FromArgb(255, 60, 63, 65));

        public DarkNumericUpDown()
        {
            InitializeComponent();

            this.BackColor = _color;
            this.ForeColor = Color.White;
        }

        public DarkNumericUpDown(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            this.BackColor = _color;
            this.ForeColor = Color.White;
        }
    }
}
