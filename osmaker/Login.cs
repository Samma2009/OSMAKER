using FireSharp;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace osmaker
{
    public partial class Login : Form
    {
        IFirebaseClient Fclient;
        public Login(IFirebaseClient Fclient)
        {
            InitializeComponent();
            this.Fclient = Fclient;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Logging in";
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
