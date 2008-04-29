using System;
using System.Collections.Generic;
using System.Text;

namespace Wzd.DirectUI.Forms
{
    public class Form:System.Windows.Forms.Form
    {
        public Form()
        {
            base.DoubleBuffered = true;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.UserPaint, true);
        }
    }
}