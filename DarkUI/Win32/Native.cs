using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DarkUI
{
    public sealed class Native
    {
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
    }
}
