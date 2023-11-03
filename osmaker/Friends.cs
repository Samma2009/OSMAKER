using FireSharp.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace osmaker
{
    public partial class Friends : Form
    {
        IFirebaseClient client;
        #region Friend inizialization
        public Friends(IFirebaseClient client)
        {
            InitializeComponent();
            this.client = client;
        }

        #endregion
        #region Friend managment

        private void Friends_Load(object sender, EventArgs e)
        {

        }

        public void ClearFriends()
        {

            listBox1.Items.Clear();

        }
        public void AddFriends(string friend)
        {

            listBox1.Items.Add(friend);

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (client.Get($@"{guna2TextBox1.Text}/Friends").Body != "null")
            {

                var a = JsonConvert.DeserializeObject<List<string>>(client.Get($@"{guna2TextBox1.Text}/Friends").Body);
                a.Add(main_page.username);
                client.Set($@"{guna2TextBox1.Text}/Friends", a);

                var b = JsonConvert.DeserializeObject<List<string>>(client.Get($@"{main_page.username}/Friends").Body);
                b.Add(guna2TextBox1.Text);
                client.Set($@"{main_page.username}/Friends", b);
                listBox1.Items.Add(guna2TextBox1.Text);

            }

        }
        #endregion
        #region Chat opening
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex != -1) 
            {

                Chat chat = new Chat(client,listBox1.SelectedItem.ToString());
                chat.Show();

            }

        }
        #endregion
    }
}
