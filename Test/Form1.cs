using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Wzd.DirectUI.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void DrawBackGround(Graphics g)
        {
            base.DrawBackGround(g);
            g.DrawImage(new Icon(this.Icon,96,96).ToBitmap(),60,60);
        }
    }
}
