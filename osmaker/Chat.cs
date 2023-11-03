using FireSharp.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace osmaker
{
    public partial class Chat : Form
    {
        IFirebaseClient client; string user;
        Dictionary<int, string> chatdata = new Dictionary<int, string>();
        Dictionary<string, string> basedict = new Dictionary<string, string>();
        #region Inizializing chat
        public Chat(IFirebaseClient client, string user)
        {
            InitializeComponent();
            this.client = client;
            this.user = user;
        }
        #endregion
        #region Getting chat data
        async void GetChat()
        {

            var cget1 = await client.GetAsync($@"{main_page.username}/Chats");
            basedict = JsonConvert.DeserializeObject<Dictionary<string, string>>(cget1.Body);
            foreach (var item in basedict)
            {

                try
                {

                    var b = item.Key.Split(':');
                    if (b[0] == user)
                    {

                        chatdata.Add(int.Parse(b[1]), user + ": " + item.Value);

                    }

                }
                catch
                {

                }

            }

            var cget2 = await client.GetAsync($@"{user}/Chats");
            var c = JsonConvert.DeserializeObject<Dictionary<string, string>>(client.Get($@"{user}/Chats").Body);
            foreach (var item in c)
            {

                var d = item.Key.Split(':');
                if (d[0] == main_page.username)
                {

                    try
                    {
                        chatdata.Add(int.Parse(d[1]), main_page.username + ": " + item.Value);
                    }
                    catch
                    {
                    }

                }

            }

            for (var i = 0; i < chatdata.Count; i++)
            {

                if (!listBox1.Items.Contains(chatdata[i]))
                {
                    listBox1.Items.Add(chatdata[i]);
                }

            }

        }

        private void Chat_Load(object sender, EventArgs e)
        {

            GetChat();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            var a = JsonConvert.DeserializeObject<Dictionary<string, string>>(client.Get($@"{user}/Chats").Body);
            a.Add($@"{main_page.username}:{chatdata.Count}",guna2TextBox1.Text);
            client.Set($@"{user}/Chats",a);
            GetChat();
            guna2TextBox1.Clear();

        }

        private async void timer1_Tick(object sender, EventArgs e)
        {

            var get = await client.GetAsync($@"{main_page.username}/Chats");


            var a = JsonConvert.DeserializeObject<Dictionary<string, string>>(get.Body);
            if (basedict.Count != a.Count)
            {

                GetChat();

            }

        }
        #endregion
    }
}
