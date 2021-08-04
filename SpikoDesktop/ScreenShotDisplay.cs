using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpikoDesktop
{
    public partial class ScreenShotDisplay : Form
    {
        private static  AdminForm_ST adminForm = null;
        private static int CurrentImageRowIndex = -1;

        private static ScreenShotDisplay _instance = null;

        //public ScreenShotDisplay GetInstance()
        //{
        //    return _instance;
        //}
        public static ScreenShotDisplay GetInstance (Image img, int rowIndex, AdminForm_ST AdminForm)
        {

            if (_instance == null)
            {
                _instance = new ScreenShotDisplay();
                _instance.MaximizedBounds = Screen.FromHandle(_instance.Handle).WorkingArea;

            }
            _instance.pictureBox1.Image = img;
            adminForm = AdminForm;
            CurrentImageRowIndex = rowIndex;
            return _instance;
        }
        private ScreenShotDisplay()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ScreenShotDisplay_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ScreenShotDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right)
            {
                //MessageBox.Show("Right key is pressed\nRow value is : " + CurrentImageRowIndex);
                CurrentImageRowIndex = adminForm.GetNextImageRowIndex(CurrentImageRowIndex);
                //MessageBox.Show(temp+ "");
                pictureBox1.Image = adminForm.GetImge(CurrentImageRowIndex);
            }
            else if  (e.KeyData == Keys.Left)
            {
                CurrentImageRowIndex = adminForm.GetPreviousImageRowIndex(CurrentImageRowIndex);
                pictureBox1.Image = adminForm.GetImge(CurrentImageRowIndex);
            }
        }
    }
}
