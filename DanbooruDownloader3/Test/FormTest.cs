using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Entity;
using System.Net;
using System.Collections.Specialized;

namespace DanbooruDownloader3.Test
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = @"http://danbooru.donmai.us/user/authenticate";
            NameValueCollection postData = new NameValueCollection();
            postData.Add("user[name]", "nandaka");
            postData.Add("user[password]", "sakuraba");
            postData.Add("url", @"http://danbooru.donmai.us/user/home");
            postData.Add("commit", "Login");

            WebClient client = new WebClient();
            
            byte[] response = client.UploadValues(url,  postData);

            ASCIIEncoding enc = new ASCIIEncoding();
            string Source = enc.GetString(response);
            textBox1.Text = Source;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox2.Text);
            
        }
    }
}
