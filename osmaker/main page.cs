using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using DynamicData.Kernel;
using Application = System.Windows.Forms.Application;


namespace osmaker
{

    public partial class main_page : Form
    {
        
        public static bool loggedin = false;
        public static string username = "", password = "";
        Dictionary<string, string> Projects;
        #region Firebase configs + private keys
        IFirebaseConfig Fconfig = new FirebaseConfig
        {

            AuthSecret = "",
            BasePath = "",

        };
        IFirebaseClient Fclient;
        #endregion
        #region Page inizialization

        public main_page()
        {
            InitializeComponent();
        }

        private void main_page_Load(object sender, EventArgs e)
        {

            if (File.Exists(System.Windows.Forms.Application.StartupPath + @"\osmaker.projects"))
            {

                Projects = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(System.Windows.Forms.Application.StartupPath + @"\osmaker.projects"));

                listBox1.Items.Clear();

                try
                {

                    if (Projects != null)
                    {
                        foreach (var item in Projects)
                        {

                            listBox1.Items.Add(item.Key);

                        }
                    }

                }
                catch
                {
                }

            }
            else
            {

                File.WriteAllText(System.Windows.Forms.Application.StartupPath + @"\osmaker.projects", "");

                Projects = new Dictionary<string, string>();

                listBox1.Items.Clear();


            }

            try
            {

                Fclient = new FirebaseClient(Fconfig);
                if (Fclient != null)
                {

                    if (File.Exists($@"{Application.StartupPath}\OSM.login"))
                    {

                        var a = File.ReadLines($@"{Application.StartupPath}\OSM.login");
                        var b = a.AsArray();
                        username = b[0];
                        password = b[1];
                        var response = Fclient.Get(username);
                        if (response.Body == "null")
                        { }
                        else
                        {
                            var c = Fclient.Get($@"{username}/Password");
                            if (password == c.Body.Replace(@"""", ""))
                            {

                                loggedin = true;
                                guna2Button5.Text = username;

                            }
                            else
                            {

                            }
                        }

                    }

                }

            }
            catch
            {
            }

        }
        #endregion
        #region Project creation and inporting
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            var a = new NewProject();
            if (a.ShowDialog() == DialogResult.OK)
            {

                //notifies and creates project
                notifyIcon1.BalloonTipText = "Creating project, OS maker will freeze for a moment";
                notifyIcon1.ShowBalloonTip(100);
                Directory.CreateDirectory($@"{a.guna2TextBox2.Text}\{a.guna2TextBox1.Text}");
                File.WriteAllText($@"{a.guna2TextBox2.Text}\{a.guna2TextBox1.Text}\{a.guna2TextBox1.Text}.OSM","");

                if (Projects == null)
                {
                    Projects = new Dictionary<string, string>();
                }
                Projects.Add(a.guna2TextBox1.Text, $@"{a.guna2TextBox2.Text}\{a.guna2TextBox1.Text}\{a.guna2TextBox1.Text}.OSM");
                

                var c = JsonConvert.SerializeObject(Projects);
                File.WriteAllText(System.Windows.Forms.Application.StartupPath + @"\osmaker.projects",c);

                ZipFile.ExtractToDirectory(@"BaseCode.zip", $@"{a.guna2TextBox2.Text}\{a.guna2TextBox1.Text}");

                var b = new Form1($@"{a.guna2TextBox2.Text}\{a.guna2TextBox1.Text}\{a.guna2TextBox1.Text}.OSM");

                b.Show();
                notifyIcon1.BalloonTipText = "Project created successfully";
                notifyIcon1.ShowBalloonTip(100);


            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Projects.Add(Path.GetFileNameWithoutExtension(openFileDialog1.FileName), $@"{openFileDialog1.FileName}");

                var c = JsonConvert.SerializeObject(Projects);
                File.WriteAllText(System.Windows.Forms.Application.StartupPath + @"\osmaker.projects", c);

                var b = new Form1($@"{openFileDialog1.FileName}");

                b.Show();

            }

        }
        #endregion
        #region Project opening
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {

            try
            {
                if (listBox1.SelectedItem != null)
                {

                    var b = new Form1(Projects[listBox1.SelectedItem.ToString()]);
                    b.Show();

                }

            }
            catch 
            {

            }

        }
        #endregion
        #region Settings, login and chat 
        private void guna2Button3_Click(object sender, EventArgs e)
        {

            ModernSettings settings = new ModernSettings();
            settings.Show();

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            Friends friends = new Friends(Fclient);
            friends.ClearFriends();
            List<string> a = JsonConvert.DeserializeObject<List<string>>(Fclient.Get($@"{username}/Friends").Body);
            foreach (var item in a)
            {

                if (item != "ME")
                {

                    friends.AddFriends(item);

                }

            }

            friends.Show();

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

            Login login = new Login(Fclient);
            login.Show();

        }
        #endregion

    }

}
class User
{

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

}
