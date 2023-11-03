using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osmaker
{
    public partial class ModernSettings : Form
    {
        public ModernSettings()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            File.Delete(System.Windows.Forms.Application.StartupPath + @"\osmaker.projects");
            Application.Restart();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void ModernSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
