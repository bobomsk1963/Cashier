using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel;
//using System.Threading;
using System.Timers;
namespace InterfaceCashier
{
    public partial class FormCashier : Form
    {
        System.Timers.Timer TimerCommand;

        CashierContext DBase = null;

        cSale Sale ;
        List<Element> ListElement;// = new List<Element>();

        BindingSource ElementBindingSource = new BindingSource();

        UserKey PanelUserKey = null;

        public FormCashier()
        {
            DBase = FormMain.DBase;
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        void StatusCassa(bool bind=false)
        {

            SummDocument.Text = (from n in ListElement select n.Summ).Sum().ToString("0.00");            
            labelStatusEdinic.Text = (from n in ListElement select n.Count).Sum().ToString();
            labelStatusPos.Text = ListElement.Count().ToString();

            if (bind)
            {
                

                ElementBindingSource.DataSource = ListElement;
                dataGridView1.DataSource = ElementBindingSource;

                labelStatusNumber.DataBindings.Clear();
                labelStatusNumber.DataBindings.Add("Text", Sale, "Number");

                comboBox1.SelectedIndex = Sale.TypeSale;
            }
            else
            {
                ElementBindingSource.ResetBindings(false);
            } 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Штрих-код
            textBox1.Focus();

            // Ищем штрихкод в номенклатуре
            try
            {
                List<cNomenklatura> ListNomenklatura = (from p in DBase.Nomenklatura where p.ShtrihCode.Equals(textBox1.Text) select p).ToList();

                if (ListNomenklatura.Count > 0)
                {
                    cNomenklatura Nomenklatura = ListNomenklatura[0];

                    // Проверять нехватку товара будем перед расчетом                  

                    //Поиск штрихкода в текущем если есть то приплюсовать 1 к количеству                

                    List<Element> le = (from n in ListElement where n.Nomenklatura.ShtrihCode.Equals(textBox1.Text) select n).ToList();
                    if (le.Count > 0)
                    {
                        Element eee = le[0];
                        eee.Count = eee.Count + 1;

                    }
                    else
                    {
                        Element eee = new Element(ListElement, new cElementSale());
                        eee.Nomenklatura = Nomenklatura;
                        eee.Price = eee.Nomenklatura.Price;
                        eee.Count = 1;
                        ListElement.Add(eee);
                    }

                    StatusCassa();                    
                }
                else
                {
                    MessageBox.Show("Штрих-код " + textBox1.Text + " не найден в номенклатуре.");
                }

                textBox1.Text = "";

            }
            catch 
            {
                MessageBox.Show("Неверно введен штрих-код.");
            }
        }

        void InitSale()
        {
            Sale = new cSale();
            ListElement = new List<Element>();

            Sale.TypeSale = 0;
            Sale.Number = 0;
            try
            {
                // Определение нового номенра документа
                Sale.Number = (from p in DBase.Sale select p.Number).Max();
            }
            catch { }
            Sale.Number = Sale.Number + 1;

            textBox1.Focus();

            // Старт таймера
            labelDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            TimerCommand = new System.Timers.Timer(1000);
            TimerCommand.Elapsed += timer_Elapsed;
            TimerCommand.Start();

            //            ElementBindingSource.DataSource = ListElement;
            //            dataGridView1.DataSource = ElementBindingSource;

            StatusCassa(true);

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int s = 0;
            for (int i = 0; i < 6; i++)
            { s = s + dataGridView1.Columns[i].Width; }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[2].Width = 180;
            dataGridView1.Columns[3].Width = 180;
            dataGridView1.Columns[4].Width = 180;
            dataGridView1.Columns[5].Width = 180;
            dataGridView1.Columns[1].Width = s - ((180 * 4) + 70);


            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;

        }

        private void FormCashier_Shown(object sender, EventArgs e)
        {
            InitSale();
        }

        private void timer_Elapsed(object source, ElapsedEventArgs e)
        {
            if (labelDateTime.InvokeRequired)
            {
                labelDateTime.Invoke
                    (
                    new Action(delegate
                        {
                            labelDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        })
                    );
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Цена
            textBox1.Focus();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Element El = (Element)dataGridView1.Rows[index].DataBoundItem;
                decimal d = 0;
                if (decimal.TryParse(textBox1.Text, out d))
                {
                    El.Price = d;
                    StatusCassa();
                }
                else { MessageBox.Show(this, "Ошибка преобразования строки в число!"); }
            }

            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Количество
            textBox1.Focus();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Element El = (Element)dataGridView1.Rows[index].DataBoundItem;
                int c = 0;
                if (int.TryParse(textBox1.Text, out c))
                {
                    El.Count = c;
                    StatusCassa();
                }
                else { MessageBox.Show(this, "Ошибка преобразования строки в число!"); }
            }

            textBox1.Text = "";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Скидка
            textBox1.Focus();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Element El = (Element)dataGridView1.Rows[index].DataBoundItem;
                int c = 0;
                if (int.TryParse(textBox1.Text, out c))
                {
                    El.Discount = c;
                    StatusCassa();
                }
                else { MessageBox.Show(this, "Ошибка преобразования строки в число!"); }
            }

            textBox1.Text = "";
        }


        private void dataGridView1_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void FormCashier_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimerCommand.Stop();
            TimerCommand.Close();
            TimerCommand.Dispose();
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Номенклатура
            textBox1.Focus();

            FormNomeklatura F = new FormNomeklatura(true);
            if (F.ShowDialog() == DialogResult.OK)
            {
                if ((F != null)&&(F.ReturnNomenklatura!=null))
                {
                    
                    textBox1.Text = F.ReturnNomenklatura.ShtrihCode;

                    button1.PerformClick();

                    ElementBindingSource.ResetBindings(false);

                }
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Сторно
            textBox1.Focus();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                Element El = (Element)dataGridView1.Rows[index].DataBoundItem;

                ListElement.Remove(El);

                StatusCassa();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Очистить документ
            textBox1.Focus();
            if (ListElement.Count > 0)
            {
                ListElement.Clear();

                Sale.TypeSale = 0;
                comboBox1.SelectedIndex = Sale.TypeSale;

                StatusCassa();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Сохранение в базу после расчета
            if (ListElement.Count > 0)
            {
                // Сначала проверм хватает ли товара на складе
                Sale.TypeSale= comboBox1.SelectedIndex;

                bool next = true;
                if (Sale.TypeSale == 0)
                {
                    for (int i = 0; i < ListElement.Count; i++)
                    {
                        int CountOstat = TestOstatkov(ListElement[i].Es.Nomenklatura);
                        if (CountOstat < ListElement[i].Es.Count)
                        {
                            next = false;
                            break;
                        }

                    }
                }

                if (next)
                {
                    //MessageBox.Show(Sale.TypeSale.ToString());

                    using (FormRaschet F = new FormRaschet(Sale, (from n in ListElement select n.Summ).Sum()))
                    {
                        if (F.ShowDialog(this) == DialogResult.OK)
                        {
                            Sale.dateTime = DateTime.Now;

                            DBase.Sale.Add(Sale);

                            for (int i = 0; i < ListElement.Count; i++)
                            {
                                ListElement[i].Es.Id = 0;
                                ListElement[i].Es.Sale = Sale;
                                DBase.ElementSale.Add(ListElement[i].Es);
                            }

                            DBase.SaveChanges();

                            InitSale();
                        }
                        else
                        {
                            button8.PerformClick();
                        }
                    }
                }
                else MessageBox.Show("На складе нехватает товара!");
            }
            else { MessageBox.Show("Не введены товары для проджи."); }
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Focus();

            if (PanelUserKey == null)
            {                
                PanelUserKey = new UserKey(textBox1);
                Point p = textBox1.PointToScreen(new Point(textBox1.Left, textBox1.Top));
                PanelUserKey.Top = p.Y + textBox1.Height;
                PanelUserKey.Left = textBox1.Left;
                PanelUserKey.Parent = this;
                PanelUserKey.BringToFront();
            }
            else { PanelUserKey.Visible = true; }
        }

        private void button6_Leave(object sender, EventArgs e)
        {
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        int TestOstatkov(cNomenklatura Nomenklatura)
        {
            // Определение количества товара на складе
            int ret = 0;
            int CountPost = (from n in Nomenklatura.ElementPost select n.Count).Sum();
            int CountSaleProd = (from n in Nomenklatura.ElementSale where n.Sale.TypeSale == 0 select n.Count).Sum();
            int CountSaleVozv = (from n in Nomenklatura.ElementSale where n.Sale.TypeSale == 1 select n.Count).Sum();
            ret = CountPost + CountSaleVozv - CountSaleProd;
            return ret;
        }

    }

    
    class Element
    {
        // Этот класс обертка для вывода в таблицу продаж
        List<Element> Parent;

        [DisplayName("№")]
        public int Number { get { return Parent.IndexOf(this)+1; } }
        [DisplayName("Наименование")]
        public virtual cNomenklatura Nomenklatura { get { return Es.Nomenklatura; } set { Es.Nomenklatura = value; } }
        [DisplayName("Цена")]
        public decimal Price { get { return Es.Price; } set { Es.Price = value; } }
        [DisplayName("Количество")]
        public int Count { get { return Es.Count; } set { Es.Count = value; } }
        [DisplayName("Скидка")]
        public decimal Discount { get { return Es.Discount; } set { Es.Discount = value; } }
        [DisplayName("Сумма")]
        public decimal Summ { get {return (Price*Count)*(1-(Discount/100)); }  }

        public cElementSale Es;
        
        public Element(List<Element> parent,cElementSale es)
        {
            Parent = parent;
            Es = es;
        }
    }
}
