using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InterfaceCashier
{
    public partial class UserKey : UserControl
    {
        TextBox Tb;

        bool IsDragMode = false;
        Point DownPoint;
        bool Vis = true;

        public UserKey(TextBox tb, bool vis=true)
        {
            Vis = vis;
            Tb = tb;
            InitializeComponent();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Tb.Focus();
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tb.Focus();
            Tb.Text = Tb.Text + ((Button)sender).Text;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            Tb.Focus();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DownPoint = e.Location;
                IsDragMode = true;
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragMode)
            {
                Point p = e.Location;
                Point dp = new Point(p.X - DownPoint.X, p.Y - DownPoint.Y);
                this.Location = new Point(this.Location.X + dp.X, this.Location.Y + dp.Y);
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            IsDragMode = false;
        }

        private void UserKey_Load(object sender, EventArgs e)
        {
            button13.Visible = Vis;
        }
    }

}
