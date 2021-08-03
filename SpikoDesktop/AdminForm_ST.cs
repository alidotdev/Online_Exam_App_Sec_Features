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
                if (string.IsNullOrEmpty(DATA[i]))
                {
                    table.Rows.Add();
                    continue;
                }


                string[] token = DATA[i].Split(',');

                if (token.Length == 5)
                {
                    table.Rows.Add(token[0], token[1], token[2], token[3], Base64ToImage(token[4]));
                }
                else
                {
                    table.Rows.Add(token);
                }
                //if (true)
                //{

                //}
            }
        }
        public Image Base64ToImage(String base64Text)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Text);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            return Image.FromStream(ms, true);
        }

        private void table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            if (col == 4)
            {
                int row = e.RowIndex;
                
                //MessageBox.Show("" + table[col, row].GetType());
                if (table[col, row].Value != null)
                {
                    MessageBox.Show("Row : " + row + "\nColumn : " + col);
                }

            }
        }
    }
}
