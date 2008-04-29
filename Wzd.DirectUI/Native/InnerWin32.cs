using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Wzd.DirectUI.Native
{
    public static class InnerWin32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public Point Reserved;
            public Point MaxSize;
            public Point MaxPosition;
            public Point MinTrackSize;
            public Point MaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }


    }
}
