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
    
    public partial class FormSaleEdit : Form
    {
        cSale Sale;
        BindingSource ElementBindingSource = new BindingSource();

        public FormSaleEdit(cSale sale)
        {
            Sale = sale;
            InitializeComponent();
        }

        private void FormSaleEdit_Shown(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = Sale;
            List<cElementSale> ListElementSale = Sale.ElementSale.ToList();

           ElementBindingSource.DataSource = ListElementSale;
           dataGridView1.DataSource = ElementBindingSource;

           label2.Text = ListElementSale.Count.ToString();
           label3.Text = (from n in ListElementSale select n.Count).Sum().ToString();
           label5.Text = (from n in ListElementSale select n.Count*n.Price*(1-(n.Discount/100))).Sum().ToString("0.00");
        }
    }
}
