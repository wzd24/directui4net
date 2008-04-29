using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Wzd.DirectUI.Native
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr nNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    }
}
