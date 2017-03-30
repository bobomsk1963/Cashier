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
    public partial class FormNomenklaturaInfo : Form
    {
        cNomenklatura Nomenklatura;
        public FormNomenklaturaInfo(cNomenklatura nomenklatura)
        {
            Nomenklatura = nomenklatura;
            InitializeComponent();
        }

        private void FormNomenklaturaInfo_Shown(object sender, EventArgs e)
        {
            label2.Text = Nomenklatura.Name;
            label4.Text = Nomenklatura.ShtrihCode;
            label6.Text = Nomenklatura.Articl;
            int CountPost = (from n in Nomenklatura.ElementPost select n.Count).Sum();
            label9.Text=CountPost.ToString();

            decimal dPost=(from n in Nomenklatura.ElementPost select n.Count*n.Price).Sum();
            label11.Text=dPost.ToString("0.00");

            int CountProd = (from n in Nomenklatura.ElementSale select n.Count*(1 - (2 * n.Sale.TypeSale))).Sum();
            label14.Text = CountProd.ToString();

            decimal  dProd = (from n in Nomenklatura.ElementSale select (n.Count * n.Price * (1 - (n.Discount / 100)) * (1 - (2 * n.Sale.TypeSale)))).Sum();            
            label16.Text = dProd.ToString("0.00");

            label18.Text = (CountPost - CountProd).ToString(); 
            label20.Text = (dPost - dProd).ToString("0.00"); 

        }
    }
}
