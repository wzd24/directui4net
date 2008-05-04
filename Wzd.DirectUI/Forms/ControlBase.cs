using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Wzd.DirectUI.Forms
{
    public class ControlBase
    {
        protected virtual void WndProc(ref Message m)
        {

        }

        protected virtual void OnClick(EventArgs e)
        {

        }

        protected virtual void OnDblClick(EventArgs e)
        {

        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {

        }


        protected virtual void OnMouseDown(MouseEventArgs e)
        {

        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
        }

        protected virtual void OnMouseHover(EventArgs e)
        {
        }

        protected virtual void OnMouseEnter(EventArgs e)
        {
        }

        protected virtual void OnMouseLeave(EventArgs e)
        {
        }

        protected virtual void OnMouseWhell(MouseEventArgs e)
        {

        }
    }
}
