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
    public partial class FormSale : Form
    {
        CashierContext DBase = null;
        public FormSale()
        {
            DBase = FormMain.DBase;
            InitializeComponent();
        }

        private void FormSale_Shown(object sender, EventArgs e)
        {
            DBase.Sale.Load();
            dataGridView1.DataSource = DBase.Sale.Local.ToBindingList();

            dataGridView1.Columns[0].HeaderText = "Код";
            dataGridView1.Columns[1].HeaderText = "Номер продажи";
            dataGridView1.Columns[2].HeaderText = "Дата и время продажи";
            dataGridView1.Columns[3].HeaderText = "Тип продажи";
            dataGridView1.Columns[4].HeaderText = "Деньги клиента";
            dataGridView1.Columns[5].HeaderText = "Сдача";
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                cSale Sale = (cSale)dataGridView1.Rows[index].DataBoundItem;

                using (FormSaleEdit F = new FormSaleEdit(Sale))
                {
                    F.ShowDialog();
                    F.Dispose();
                }


            }
        }
    }
}
