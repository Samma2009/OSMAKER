using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osmaker
{
    public partial class CustomComboBox : ComboBox
    {
        public CustomComboBox()
        {
            InitializeComponent();
            foreach (var item in Form1.plugins)
            {

                this.Items.Add(item.Key);

            }
        }

    }
}
