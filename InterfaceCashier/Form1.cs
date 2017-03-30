using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Entity;
namespace InterfaceCashier
{
    public partial class FormMain : Form
    {

        public static CashierContext DBase;

        public FormMain()
        {
            InitializeComponent();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Form F = null;


            switch (int.Parse((string)((Button)sender).Tag))
            {
                case 0:
                    F = new FormCashier();
                    break;
                case 1:
                    F = new FormNomeklatura();
                    break;
                case 2:
                    F = new FormPost();
                    break;
                case 3:
                    F = new FormSale();                    
                    break;
            }

            if (F != null) 
            { 
                F.ShowDialog(this);
                F.Dispose();
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            DBase = new CashierContext();
            DBase.Nomenklatura.Load();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (FormAbout F = new FormAbout())
            {
                F.ShowDialog(this);
            }
        }
    }
}
