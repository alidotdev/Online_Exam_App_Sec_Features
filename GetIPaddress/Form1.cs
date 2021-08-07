using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetIPaddress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            textBox2.Text =hostName;
            // Get the IP  
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            textBox1.Text = myIP;
            //Console.ReadKey();
        }
    }
}
