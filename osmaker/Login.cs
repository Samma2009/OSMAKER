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

        public static string Encrypt(string value)
        {

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

        }
        public static string Decrypt(string value)
        {

            return Encoding.UTF8.GetString(Convert.FromBase64String(value));

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Logging in";
            try
            {
                var response = Fclient.Get(guna2TextBox1.Text);
                if (response.Body == "null")
                {
                    
                    Fclient.Set($@"{guna2TextBox1.Text}/Password", Encrypt(guna2TextBox2.Text));
                    Fclient.Set($@"{guna2TextBox1.Text}/Friends", new List<string>() {"ME"});
                    Fclient.Set($@"{guna2TextBox1.Text}/Chats", new Dictionary<string, string>{{ "ME:0", "CHAT" }});
                    label1.Text = "no accounts found, creating one";
                    main_page.loggedin = true;
                    main_page.username = guna2TextBox1.Text;
                    main_page.password = guna2TextBox2.Text;
                    File.WriteAllText($@"{Application.StartupPath}\OSM.login", $"{guna2TextBox1.Text}\n{guna2TextBox2.Text}");
                    this.Close();
                    Application.Restart();

                }
                else
                {
                    var a = Fclient.Get($@"{guna2TextBox1.Text}/Password");
                    if (guna2TextBox2.Text == Decrypt(a.Body.Replace(@"""","")))
                    {

                        label1.Text = "Logged in";
                        main_page.loggedin = true;
                        File.WriteAllText($@"{Application.StartupPath}\OSM.login", $"{guna2TextBox1.Text}\n{Encrypt(guna2TextBox2.Text)}");
                        Application.Restart();
                        this.Close();

                    }
                    else 
                    {

                        label1.Text = "incorrect password";

                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid Token");

            }


            
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
