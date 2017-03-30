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
    public partial class FormNomeklatura : Form
    {

        CashierContext DBase = null;

        public cNomenklatura ReturnNomenklatura = null;

        bool select=false;  // При истине номенклатура открыта с возможностью выбора


        public FormNomeklatura(bool sel=false)//CashierContext dBase)
        {
            select = sel;
            DBase = FormMain.DBase;
            InitializeComponent();            
        }

        private void FormNomeklatura_Shown(object sender, EventArgs e)
        {
            //DBase.Nomenklatura.Load();

                button5.Visible=select;
                button6.Visible = select;


            dataGridView1.DataSource = DBase.Nomenklatura.Local.ToBindingList();
            dataGridView1.Columns[0].HeaderText = "Код";
            dataGridView1.Columns[1].HeaderText = "Наименование";
            dataGridView1.Columns[2].HeaderText = "Штрих-код";
            dataGridView1.Columns[3].HeaderText = "Артикул";
            dataGridView1.Columns[4].HeaderText = "Цена";
        }

        private void button1_Click(object sender, EventArgs e)
        {                                          
            //Добавление элемента номенклатуры
            cNomenklatura Nomenklatura = new cNomenklatura();

            FormNomenklaturaEdit F = new FormNomenklaturaEdit(Nomenklatura);


            if (F.ShowDialog(this) == DialogResult.OK)
            {
                List<cNomenklatura> ListNomenklatura = (from p in DBase.Nomenklatura where p.ShtrihCode.Equals(Nomenklatura.ShtrihCode) select p).ToList();
                if (ListNomenklatura.Count == 0)
                {
                    try
                    {
                        DBase.Nomenklatura.Add(Nomenklatura);
                        DBase.SaveChanges();
                    }
                    catch { MessageBox.Show("Неверно заполнены поля попробуйте еще раз."); }
                }
                else { MessageBox.Show("Товар с таким штрих-кодом уже есть!"); }
            }

            F.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Редактирование элемента номенклатуры
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;

                cNomenklatura Nomenklatura= (cNomenklatura)dataGridView1.Rows[index].DataBoundItem;

                cNomenklatura Nomenklatura1 =(cNomenklatura)Nomenklatura.Clone();                

                FormNomenklaturaEdit F = new FormNomenklaturaEdit(Nomenklatura1);
                F.Text = "Редактирование товара в номенклатуре";
                if (F.ShowDialog(this) == DialogResult.OK)
                {
                    List<cNomenklatura> ListNomenklatura = (from p in DBase.Nomenklatura where p.ShtrihCode.Equals(Nomenklatura1.ShtrihCode) select p).ToList();

                    if ((ListNomenklatura.Count == 0) ||
                        ((ListNomenklatura.Count > 0) && (ListNomenklatura[0].Id == Nomenklatura.Id)))
                    {

                        Nomenklatura.Name = Nomenklatura1.Name;
                        Nomenklatura.ShtrihCode = Nomenklatura1.ShtrihCode;
                        Nomenklatura.Articl = Nomenklatura1.Articl;
                        Nomenklatura.Price = Nomenklatura1.Price;

                        DBase.Entry(Nomenklatura).State = EntityState.Modified;

                        DBase.SaveChanges();

                        dataGridView1.Refresh();
                    }
                    else { MessageBox.Show("Попытка изменить штрих-код на значение которое уже есть в базе!"); }
                }

                F.Dispose();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Удаление
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                cNomenklatura Nomenklatura = (cNomenklatura)dataGridView1.Rows[index].DataBoundItem;

                int count=(from p in Nomenklatura.ElementPost select p).Count();                
                // Проверить есть ли хоть одна подчиненная запись если есть удалять нельзя                
                if (count == 0)
                {
                    DBase.Nomenklatura.Remove(Nomenklatura);

                    DBase.SaveChanges();
                    dataGridView1.Refresh();
                }
                else 
                { MessageBox.Show("Удаление не состаялось. У этого товара есть поступления."); }
                 
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Выбор товара из номенклатуры по кнопке
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                ReturnNomenklatura = (cNomenklatura)dataGridView1.Rows[index].DataBoundItem;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // Выбор товара из номенклатуры по двойному щелчку
            if (select)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int index = dataGridView1.SelectedRows[0].Index;
                    ReturnNomenklatura = (cNomenklatura)dataGridView1.Rows[index].DataBoundItem;

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Информация о товаре
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
               cNomenklatura Nomenklatura = (cNomenklatura)dataGridView1.Rows[index].DataBoundItem;

                //int c= TestOstatkov(Nomenklatura);
                //MessageBox.Show("Этого товара на складе - " + c.ToString() + " - едениц.");

               using (FormNomenklaturaInfo F = new FormNomenklaturaInfo(Nomenklatura))
               {
                   F.ShowDialog(this);
               }


            }
        }

        int TestOstatkov(cNomenklatura Nomenklatura)
        {
            // Определение количества товара на складе
            int ret = 0;
            int CountPost = (from n in Nomenklatura.ElementPost select n.Count).Sum();
            int CountSaleProd = (from n in Nomenklatura.ElementSale where n.Sale.TypeSale == 0 select n.Count*(1-(2*n.Sale.TypeSale))).Sum();
            //int CountSaleVozv = (from n in Nomenklatura.ElementSale where n.Sale.TypeSale == 1 select n.Count).Sum();
            ret = CountPost + CountSaleProd;
            return ret;
        }
    }
}
