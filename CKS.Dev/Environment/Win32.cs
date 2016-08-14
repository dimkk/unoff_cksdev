using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    class Win32
    {
        public const int WM_SIZE = 0x05;
        public const int SIZE_RESTORED = 0;
        public const int WS_EX_CONTROLPARENT = 0x10000;
        public const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SendMessage(IntPtr hwnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetWindowLong(IntPtr hwnd, int index, long value);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hwnd, int index);
    }
}
