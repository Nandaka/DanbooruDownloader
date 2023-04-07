using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DanbooruDownloader3.CustomControl;
using System.Net;
using System.Collections;
using System.Reflection;

namespace DanbooruDownloader3
{
    public partial class FormCookie : Form
    {
        public FormCookie()
        {
            InitializeComponent();
        }

        private void FormCookie_Load(object sender, EventArgs e)
        {
            var cj = ExtendedWebClient.CookieJar;
            if (cj != null)
            {
                dgvCookie.DataSource = GetAllCookies(cj);
                dgvCookie.Refresh();
            }
        }

        /// <summary>
        /// Taken from http://stackoverflow.com/a/15991071
        /// </summary>
        /// <param name="cookieJar"></param>
        /// <returns></returns>
        public List<Cookie> GetAllCookies(CookieContainer cookieJar)
        {
            List<Cookie> cookieCollection = new List<Cookie>();

            Hashtable table = (Hashtable)cookieJar.GetType().InvokeMember("m_domainTable",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            cookieJar,
                                                                            new object[] { });

            foreach (var tableKey in table.Keys)
            {
                String str_tableKey = (string)tableKey;

                if (str_tableKey[0] == '.')
                {
                    str_tableKey = str_tableKey.Substring(1);
                }

                SortedList list = (SortedList)table[tableKey].GetType().InvokeMember("m_list",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            table[tableKey],
                                                                            new object[] { });

                foreach (var listKey in list.Keys)
                {
                    String url = "https://" + str_tableKey + (string)listKey;
                    var cookies = cookieJar.GetCookies(new Uri(url));
                    foreach (Cookie c in cookies)
                    {
                        cookieCollection.Add(c);
                    }
                }
            }

            return cookieCollection;
        }

        private void btnClearCookie_Click(object sender, EventArgs e)
        {
            var cj = ExtendedWebClient.CookieJar;
            if (cj != null)
            {
                ExtendedWebClient.ClearCookies();
                dgvCookie.DataSource = GetAllCookies(ExtendedWebClient.CookieJar);
                dgvCookie.Refresh(); 
            }
        }
    }
}
