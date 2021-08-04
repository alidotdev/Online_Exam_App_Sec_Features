using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace CameraCapturing
{
    public partial class Form1 : Form
    {
        Image img = null;
        public Form1()
        {
            InitializeComponent();

        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        //VideoCaptureDevice videoCapture;

        private void Form1_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cboCamera.Items.Add(filterInfo.Name);
                cboCamera.SelectedIndex = 0;
                videoCaptureDevice = new VideoCaptureDevice();
            }


        }

        private void btn_Click(object sender, EventArgs e)
        {
            //cboCamera.SelectedIndex ;
            timer1.Enabled = !(timer1.Enabled);
            
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += videoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
            //while (true)
            //{
            //    TimeSpan s = new TimeSpan(0, 0, 10);
            //    Thread.Sleep(s);
            //    pictureBox1.Image = pic.Image;

            //}
        }




        NewFrameEventArgs eventArgs1 = null;
        private void videoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            eventArgs1 = eventArgs;

            //pic.Image = (Bitmap)eventArgs.Frame.Clone();
            img = (Bitmap)eventArgs.Frame.Clone();
            pic.Image = img;
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs a)
        {
            pic.Image.Save("sample.jpg");
            pictureBox1.Image = pic.Image;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //pictureBox1.Image = pic.Image;
            pictureBox1.Image = img;
        }
    }
}
