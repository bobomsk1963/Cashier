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
    public partial class FormPost : Form
    {
        CashierContext DBase = null;
        public FormPost()//CashierContext dBase)
        {
            DBase = FormMain.DBase;
            InitializeComponent();
        }

        private void FormPost_Shown(object sender, EventArgs e)
        {
            DBase.Post.Load();

            dataGridView1.DataSource = DBase.Post.Local.ToBindingList();

            dataGridView1.Columns[0].HeaderText = "Код";
            dataGridView1.Columns[1].HeaderText = "Номер накладной";
            dataGridView1.Columns[2].HeaderText = "Дата";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Добавление поступлений
            
            cPost Post = new cPost();

            // Поставить автоматически номер накладной
            Post.Nuber = 0;
            try
            {
                // Определение максимального номера накладной
                //Post.Nuber = DBase.Post.Max(p => p.Nuber);
                Post.Nuber = (from p in DBase.Post select p.Nuber).Max();
                   // Count -- количество
                   // Sum   -- подсчитывает сумму
            }
            catch { }

            Post.Nuber = Post.Nuber + 1;

            //Устанавливаем текущюю дату без времени
            DateTime dt = DateTime.Now;
            Post.dateTime = new DateTime(dt.Year, dt.Month, dt.Day);

            FormPostEdit F = new FormPostEdit(Post);

            if (F.ShowDialog(this) == DialogResult.OK)
            {
                DBase.Post.Add(Post);

                DBase.ElementPost.AddRange(F.ListElementPost);
                DBase.SaveChanges();
            }

            F.Dispose();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Изменения

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                cPost Post = (cPost)dataGridView1.Rows[index].DataBoundItem;

                FormPostEdit F = new FormPostEdit(Post,false);

                List<cElementPost> Lep = Post.ElementPost.ToList();

                for(int i=0;i<Lep.Count;i++)
                {
                    cElementPost ee = new cElementPost();
                    ee.Id=i+1;
                    ee.Post = Lep[i].Post;
                    ee.Nomenklatura = Lep[i].Nomenklatura;
                    ee.Count=Lep[i].Count;
                    ee.Price=Lep[i].Price;
                    F.ListElementPost .Add(ee);
                }

                if (F.ShowDialog(this) == DialogResult.OK)
                {
                    DBase.ElementPost.RemoveRange(Lep);

                    DBase.ElementPost.AddRange(F.ListElementPost);

                    DBase.SaveChanges();
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
                cPost Post = (cPost)dataGridView1.Rows[index].DataBoundItem;

                int count = (from p in Post.ElementPost select p).Count();
                // Проверить есть ли хоть одна подчиненная запись если есть удалять нельзя                
                if (count == 0)
                {
                    DBase.Post.Remove(Post);

                    DBase.SaveChanges();

                    dataGridView1.Refresh();
                }
                else
                { MessageBox.Show("Удаление не состаялось. В этой накладной есть товары."); }

            }
        }
    }
}
