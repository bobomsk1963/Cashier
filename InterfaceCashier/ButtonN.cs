using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
namespace InterfaceCashier
{
    class ButtonN : Button  // Отключение возможностит фокуса
    {
        public ButtonN()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }
    }
}
