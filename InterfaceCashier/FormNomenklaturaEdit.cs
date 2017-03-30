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
    public partial class FormNomenklaturaEdit : Form
    {
        cNomenklatura Nomenklatura;

        public FormNomenklaturaEdit(cNomenklatura nomenklatura)
        {
            Nomenklatura = nomenklatura;
            InitializeComponent();

        }

        private void FormNomenklaturaEdit_Shown(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = Nomenklatura;
        }
    }
}
