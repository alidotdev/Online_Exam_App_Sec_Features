using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ScreenCapture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !(timer1.Enabled);
            timer1.Interval = 10000;
            Image i = CaptureMyScreen();
             pictureBox1.Image =i;
        }
        private Image CaptureMyScreen()
        {
            try
            {
                Bitmap screenshot = new Bitmap(1920, 1080);
                
                using (Graphics screenGraph = Graphics.FromImage(screenshot))
                {
                    screenGraph.CopyFromScreen(
                        SystemInformation.VirtualScreen.X,
                        SystemInformation.VirtualScreen.Y,
                        0,
                        0,
                        screenshot.Size,
                        CopyPixelOperation.SourceCopy);
                }
                screenshot.Save("Screen.jpg");
                return screenshot;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Image i = CaptureMyScreen();
            pictureBox1.Image = i;
        }
    }
}

