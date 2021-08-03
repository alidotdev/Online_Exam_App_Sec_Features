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

namespace SpikoDesktop
{
    public partial class AdminForm_ST : Form
    {
        public AdminForm_ST()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string data =File.ReadAllText("Data.csv");
            string[] DATA = data.Split('\n');
            for (int i = 0; i < DATA.Length; i++)
            {
                string[] token;
                if (string.IsNullOrEmpty(DATA[i]))
                {
                table.Rows.Add();
                    continue;
                }
                token = DATA[i].Split(',');
                table.Rows.Add(token);
                //if (true)
                //{

                //}
            }

        }
    }
}
