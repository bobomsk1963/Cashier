using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InterfaceCashier
{
    public partial class FormRaschet : Form
    {
        cSale Sale;

        decimal Summa;

        UserKey PanelUserKey = null;

        public FormRaschet(cSale sale,decimal summa)
        {
            Sale = sale;
            Summa = summa;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormRaschet_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show(Sale.TypeSale.ToString());
            if (Sale.TypeSale == 1)
            {
                textBox1.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label6.Text = "Сумма возврата";

            }
            else 
            {
                PanelUserKey = new UserKey(textBox1,false);
                
                //Point p = textBox1.PointToScreen(new Point(textBox1.Left, textBox1.Top));
                PanelUserKey.Top = textBox1.Top + textBox1.Height;
                PanelUserKey.Left = textBox1.Left;
                PanelUserKey.Parent = this;
                PanelUserKey.BringToFront();

            }
           SummDocument.Text = Summa.ToString("0.00");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                decimal d=0;
                if (decimal.TryParse(textBox1.Text, out d))
                {
                    Sale.ManyClient = d;
                    Sale.ManyChange = Sale.ManyClient - Summa;
                    label1.Text = Sale.ManyChange.ToString("0.00");
                }
        }

    }
}
