using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Wzd.DirectUI.Native;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Wzd.DirectUI.Utils;
using Wzd.DirectUI.Properties;
using System.ComponentModel;

namespace Wzd.DirectUI.Forms
{
    public class Form : System.Windows.Forms.Form
    {
        private bool bActived;
        private int bWidth, bHeight;

        private Bitmap _backbuffer_bitmap;

        ButtonStatus closeStatus, MinStatus, MaxStatus;

        protected Bitmap BackBufferBitmap
        {
            get
            {
                return this._backbuffer_bitmap;
            }
            set
            {
                if (this.BackBufferBitmap != null)
                {
                    this.BackBufferBitmap.Dispose();
                }
                this._backbuffer_bitmap = value;
            }
        }

        protected Rectangle ContentRectangle
        {
            get { return new Rectangle(0, 0, Width - bWidth * 2, Height - bHeight * 2); }
        }

        protected virtual Rectangle IconRectangle
        {
            get { return new Rectangle(5, 5, 16, 16); }
        }

        protected virtual Rectangle CloseButtonRectangle
        {
            get { return new Rectangle(ContentRectangle.Width - 36, 1, 31, 20); }
        }

        protected virtual Rectangle MinButtonRectangle
        {
            get { return new Rectangle(ContentRectangle.Width - 98, 1, 31, 20); }
        }

        protected virtual Rectangle MaxButtonRectangle
        {
            get { return new Rectangle(ContentRectangle.Width - 67, 1, 31, 20); }
        }

        public Form()
        {
            base.DoubleBuffered = true;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        protected override void OnActivated(EventArgs e)
        {
            this.bActived = true;
            base.Invalidate();
            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            this.bActived = false;
            base.Invalidate();
            base.OnDeactivate(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.TranslateTransform(bWidth, bHeight);
            g.DrawImage(BackBufferBitmap, 0, 0);
            DrawMaxButton(g);
            DrawMinButton(g);
            DrawCloseButton(g);
            g.DrawIcon(new Icon(this.Icon, 16, 16), IconRectangle);
            PaintDeactive(g);
        }

        private void PaintDeactive(Graphics g)
        {
            if (!this.bActived)
            {
                using (Bitmap bitmap = new Bitmap(base.Width, 24))
                {
                    Graphics.FromImage(bitmap).FillRectangle(new SolidBrush(Color.White), 0, 0, bitmap.Width, bitmap.Height);
                    ImageAttributes imageAttr = new ImageAttributes();
                    ColorMatrix newColorMatrix = new ColorMatrix
                    {
                        Matrix00 = 1f,
                        Matrix11 = 1f,
                        Matrix22 = 1f,
                        Matrix33 = 0.3f,
                        Matrix44 = 1f
                    };
                    imageAttr.SetColorMatrix(newColorMatrix);
                    Rectangle destRect = new Rectangle(1, 1, bitmap.Width, bitmap.Height);
                    g.DrawImage(bitmap, destRect, 1, 1, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttr);
                }
            }
        }

        private void DrawCloseButton(Graphics g)
        {
            Image close;
            switch (closeStatus)
            {
                case ButtonStatus.Hover:
                    close = Resources.IMCloseButton_Hover;
                    break;
                case ButtonStatus.Down:
                    close = Resources.IMCloseButton_Down;
                    break;
                default:
                    close = Resources.IMCloseButton_Normal;
                    break;
            }
            g.DrawImage(close, CloseButtonRectangle, new Rectangle(0, 0, 31, 20), GraphicsUnit.Pixel);
        }

        private void DrawMinButton(Graphics g)
        {
            Image min;
            switch (MinStatus)
            {
                case ButtonStatus.Hover:
                    min = Resources.IMMinButton_Hover;
                    break;
                case ButtonStatus.Down:
                    min = Resources.IMMinButton_Down;
                    break;
                default:
                    min = Resources.IMMinButton_Normal;
                    break;
            }

            g.DrawImage(min, MinButtonRectangle, new Rectangle(0, 0, 31, 20), GraphicsUnit.Pixel);
        }

        private void DrawMaxButton(Graphics g)
        {
            Image max;
            switch (MaxStatus)
            {
                case ButtonStatus.Hover:
                    max = WindowState == FormWindowState.Maximized ? Resources.IMRestoreButton_Hover : Resources.IMMaxButton_Hover;
                    break;
                case ButtonStatus.Down:
                    max = WindowState == FormWindowState.Maximized ? Resources.IMRestoreButton_Down : Resources.IMMaxButton_Down;
                    break;
                default:
                    max = WindowState == FormWindowState.Maximized ? Resources.IMRestoreButton_Normal : Resources.IMMaxButton_Normal;
                    break;
            }
            g.DrawImage(max, MaxButtonRectangle, new Rectangle(0, 0, 31, 20), GraphicsUnit.Pixel);
        }

        protected virtual void DrawBackGround(Graphics g)
        {
            g.Clear(Color.FromArgb(170, 217, 251));
            DrawBorder(g);
        }

        protected virtual void DrawBorder(Graphics g)
        {
            TextureBrush brush;
            if (WindowState == FormWindowState.Maximized)
            {
                brush = new TextureBrush(Resources.IMBorderTop2, WrapMode.Tile, new Rectangle(10, 0, 1, 24));
            }
            else
            {
                brush = new TextureBrush(Resources.IMBorderTop, WrapMode.Tile, new Rectangle(10, 0, 1, 24));
            }
            g.FillRectangle(brush, new Rectangle(0, 0, Width, 24));
            brush.Dispose();

            brush = new TextureBrush(Resources.IMBorderLeft);
            g.FillRectangle(brush, new Rectangle(0, 0, 3, Height));
            brush.Dispose();

            brush = new TextureBrush(Resources.IMBorderRight);
            brush.TranslateTransform(base.Width - bWidth * 2, 0);
            g.FillRectangle(brush, new Rectangle(base.Width - bWidth * 2 - 3, 0, 3, Height));
            brush.Dispose();

            brush = new TextureBrush(Resources.IMBorderBottom, new Rectangle(10, 0, 1, 8));
            brush.TranslateTransform(0, base.Height - bHeight * 2);
            g.FillRectangle(brush, new Rectangle(0, base.Height - bHeight * 2 - 3, Width, 3));
            brush.Dispose();

            if (WindowState == FormWindowState.Maximized)
            {
                g.DrawImage(Resources.IMBorderLeftTop2, 0, 0, 3, 24);
                g.DrawImage(Resources.IMBorderRightTop2, base.Width - bWidth * 2 - 3, 0, 3, 24);
                g.DrawImage(Resources.IMBorderLeftBottom2, 0, base.Height - bHeight * 2 - 3, 3, 3);
                g.DrawImage(Resources.IMBorderRightBottom2, base.Width - bWidth * 2 - 3, base.Height - bHeight * 2 - 3, 3, 3);
            }
            else
            {
                g.DrawImage(Resources.IMBorderTop, new Rectangle(0, 0, 25, 24), new Rectangle(0, 0, 25, 24), GraphicsUnit.Pixel);
                g.DrawImage(Resources.IMBorderTop, new Rectangle(base.Width - bWidth * 2 - 25, 0, 25, 24), new Rectangle(25, 0, 25, 24), GraphicsUnit.Pixel);
                g.DrawImage(Resources.IMBorderBottom, new Rectangle(0, base.Height - bHeight * 2 - 8, 10, 8), new Rectangle(0, 0, 10, 8), GraphicsUnit.Pixel);
                g.DrawImage(Resources.IMBorderBottom, new Rectangle(base.Width - bWidth * 2 - 10, base.Height - bHeight * 2 - 8, 10, 8), new Rectangle(40, 0, 10, 8), GraphicsUnit.Pixel);
            }
            int height = g.MeasureString(Text, SystemFonts.CaptionFont).ToSize().Height;
            g.DrawString(Text, SystemFonts.CaptionFont, Brushes.DarkSlateBlue, 22, (24-height)/2);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (WindowState == FormWindowState.Normal)
            {
                bWidth = 0;
                bHeight = 0;
            }
            else
            {
                bWidth = NativeMethods.GetSystemMetrics(32);
                bHeight = NativeMethods.GetSystemMetrics(33);
            }
            this.BackBufferBitmap = new Bitmap(base.Width - bWidth, base.Height - bHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(this.BackBufferBitmap);
            DrawBackGround(g);
            this.WndRegion = BitmapToRegion.Convert(this.BackBufferBitmap, Color.FromArgb(255, 0, 255), TransparencyMode.ColorKeyTransparent, 0, 0);
            base.Invalidate();
        }

        protected Region WndRegion
        {
            get
            {
                return base.Region;
            }
            set
            {
                if (this.WndRegion != null)
                {
                    this.WndRegion.Dispose();
                }
                base.Region = value;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x85:
                case 0x83:
                    return;
                case 0x86:
                    m.Result = (IntPtr)1;
                    return;
                case 0x84:
                    WM_NCHITTEST(ref m);
                    return;
                case 0xa5:
                    Point vPoint = new Point((int)m.LParam);
                    Point pt = PointToClient(vPoint);
                    pt.Offset(-bWidth, -bHeight);
                    Rectangle vRect;
                    if (pt.Y < 24 + bHeight && !new Rectangle(base.Width - bWidth * 2 - 98, 1, 93, 20).Contains(pt))
                        NativeMethods.SendMessage(Handle, InnerWin32.WM_SYSCOMMAND, NativeMethods.TrackPopupMenu(
                            NativeMethods.GetSystemMenu(Handle, false),
                            InnerWin32.TPM_RETURNCMD | InnerWin32.TPM_LEFTBUTTON, vPoint.X, vPoint.Y,
                            0, Handle, out vRect), 0);
                    base.WndProc(ref m);
                    break;
                case 0xa1:
                    WM_NCLBUTTONDOWN(ref m);
                    base.WndProc(ref m);
                    break;
                case 0xa2:
                    WM_NCLBUTTONUP(ref m);
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }


        }

        private void WM_NCLBUTTONDOWN(ref Message m)
        {
            Point vPoint = new Point((int)m.LParam);
            Point pt = PointToClient(vPoint);
            pt.Offset(-bWidth, -bHeight);
            if (CloseButtonRectangle.Contains(pt) && closeStatus != ButtonStatus.Down)
            {
                closeStatus = ButtonStatus.Down;
                base.Invalidate();
            }
            else if (MinButtonRectangle.Contains(pt) && MinStatus != ButtonStatus.Down)
            {
                MinStatus = ButtonStatus.Down;
                base.Invalidate();
            }
            else if (MaxButtonRectangle.Contains(pt) && MaxStatus != ButtonStatus.Down)
            {
                MaxStatus = ButtonStatus.Down;
                base.Invalidate();
            }
        }

        private void WM_NCLBUTTONUP(ref Message m)
        {
            Point vPoint = new Point((int)m.LParam);
            Point pt = PointToClient(vPoint);
            pt.Offset(-bWidth, -bHeight);
            if (CloseButtonRectangle.Contains(pt))
            {
                this.Close();
            }
            else if (MinButtonRectangle.Contains(pt))
            {
                this.WindowState = FormWindowState.Minimized;
                MinStatus = ButtonStatus.Normal;
            }
            else if (MaxButtonRectangle.Contains(pt))
            {
                this.WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
                MaxStatus = ButtonStatus.Normal;
            }
        }

        private void WM_NCHITTEST(ref Message m)
        {
            m.Result = new IntPtr(1);
            Point pt = PointToClient(new Point((int)m.LParam));
            pt.Offset(-bWidth, -bHeight);
            int x = pt.X;
            int y = pt.Y;
            if (IconRectangle.Contains(pt))
            {
                m.Result = new IntPtr(3);
                return;
            }
            if (ContainsClose(ref m, ref pt))
            {
                return;
            }
            if (ContainsMin(ref m, ref pt))
            {
                return;
            }
            if (ContainsMax(ref m, ref pt))
            {
                return;
            }
            if (y < 5 && x < 8 && WindowState == FormWindowState.Normal)
            {
                m.Result = (IntPtr)13;
                return;
            }
            if (y < 5 && x > Width - 8 && WindowState == FormWindowState.Normal)
            {
                m.Result = (IntPtr)14;
                return;
            }
            if (y > Height - 5 && x > Width - 8 && WindowState == FormWindowState.Normal)
            {
                m.Result = (IntPtr)17;
                return;
            }
            if (y > Height - 5 && x < 8 && WindowState == FormWindowState.Normal)
            {
                m.Result = (IntPtr)16;
                return;
            }
            if (y < 5 && WindowState == FormWindowState.Normal)
            {
                m.Result = new IntPtr(12);
                return;
            }
            if (y > (base.Height - 5))
            {
                m.Result = new IntPtr(15);
                return;
            }
            if (x < 5 + bWidth)
            {
                m.Result = new IntPtr(10);
                return;
            }
            if (x > (base.Width - 5))
            {
                m.Result = new IntPtr(11);
                return;
            }
            if (y < 24)
            {
                m.Result = new IntPtr(2);
                return;
            }
        }

        private bool ContainsClose(ref Message m, ref Point pt)
        {
            bool result = false;
            if (CloseButtonRectangle.Contains(pt))
            {
                if (closeStatus == ButtonStatus.Normal)
                {
                    closeStatus = ButtonStatus.Hover;
                    MaxStatus = ButtonStatus.Normal;
                    MinStatus = ButtonStatus.Normal;
                    base.Invalidate();
                }
                m.Result = new IntPtr(19);
                result = true;
            }
            else if (closeStatus != ButtonStatus.Normal)
            {
                closeStatus = ButtonStatus.Normal;
                base.Invalidate();
                result = false;
            }
            return result;
        }

        private bool ContainsMax(ref Message m, ref Point pt)
        {
            bool result = false;
            if (this.MaxButtonRectangle.Contains(pt))
            {
                if (MaxStatus == ButtonStatus.Normal)
                {
                    MaxStatus = ButtonStatus.Hover;
                    MinStatus = ButtonStatus.Normal;
                    closeStatus = ButtonStatus.Normal;
                    base.Invalidate();
                }
                m.Result = new IntPtr(19);
                result = true;
            }
            else if (MaxStatus != ButtonStatus.Normal)
            {
                MaxStatus = ButtonStatus.Normal;
                base.Invalidate();
                result = false;
            }
            return result;
        }

        private bool ContainsMin(ref Message m, ref Point pt)
        {
            bool result = false;
            if (MinButtonRectangle.Contains(pt))
            {
                if (MinStatus == ButtonStatus.Normal)
                {
                    MinStatus = ButtonStatus.Hover;
                    MaxStatus = ButtonStatus.Normal;
                    closeStatus = ButtonStatus.Normal;
                    base.Invalidate();
                }
                m.Result = new IntPtr(19);
                result = true;
            }
            else if (MinStatus != ButtonStatus.Normal)
            {
                MinStatus = ButtonStatus.Normal;
                base.Invalidate();
                result = false;
            }
            return result;
        }

        enum ButtonStatus
        {
            Normal, Hover, Down
        }
    }
}