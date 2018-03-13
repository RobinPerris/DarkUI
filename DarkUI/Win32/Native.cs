using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DarkUI.Win32
{
    internal static class Native
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        internal static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);

        internal const int GW_CHILD = 5;

        [DllImport("user32")] [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, int wCmd);

        internal const int SW_HIDE = 0;
    }
}
