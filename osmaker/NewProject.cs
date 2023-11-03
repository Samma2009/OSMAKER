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
    public partial class NewProject : Form
    {
        public NewProject()
        {
            InitializeComponent();
            guna2TextBox1.PlaceholderText = "Project name";
            guna2TextBox2.PlaceholderText = "Project directory";

        }

        private void NewProject_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                guna2TextBox2.Text = folderBrowserDialog1.SelectedPath;

            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;

        }
    }
}
