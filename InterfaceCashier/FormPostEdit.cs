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

    public partial class FormPostEdit : Form
    {
        cPost Post;
        public List<cElementPost> ListElementPost = new List<cElementPost>();

        BindingSource ElementBindingSource = new BindingSource();

        bool Edit = true;

        public FormPostEdit(cPost post,bool edit=true)
        {
            Edit = edit;
            Post = post;
            InitializeComponent();
        }

        private void FormPostEdit_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("22222");

            propertyGrid1.SelectedObject = Post;

            ElementBindingSource.DataSource = ListElementPost;
            dataGridView1.DataSource = ElementBindingSource;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].ReadOnly = false;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].ReadOnly = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormNomeklatura F = new FormNomeklatura(true);
            if (F.ShowDialog() == DialogResult.OK)
            {
                if (F != null)
                {
                    cElementPost ep = new cElementPost();
                    ep.Id = ListElementPost.Count+1;
                    ep.Post = Post;
                    ep.Nomenklatura = F.ReturnNomenklatura;

                    ListElementPost.Add(ep);

                    ElementBindingSource.ResetBindings(false);
                    
                }
            }
             
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Удалить товар
            
            if (dataGridView1.SelectedCells.Count>0)
            {
                ListElementPost.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);//dataGridView1.SelectedRows[0].Index);

                ElementBindingSource.ResetBindings(false);
            }

        }
    }
}
