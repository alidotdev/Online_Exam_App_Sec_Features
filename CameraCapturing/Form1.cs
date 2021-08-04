using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        static Process[] previous = null;
        static string file = "Data.csv";
        static string screen = "Screen.png";
        //Image img = null;

        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("i AM in FORM 1");
        }
         void Filing()
        {
            bool isOld = false;
            Image i = GetUserImage();
            pictureBox1.Image = i;
            var image = CaptureMyScreen();
            ImageConverter converter = new ImageConverter();
            byte[] FILE = (byte[])converter.ConvertTo(image, typeof(byte[]));
            // byte[] FILE = image.;
            string base64Text = Convert.ToBase64String(FILE);
            //Console.WriteLine(base64Text);
            File.AppendAllText(file, $"{"Date and Time : "},{DateTime.Now},{"Screen short     "},{base64Text}\n");
            File.AppendAllText(file, $"{"Applications"},{"RAM Used in MB"},{"Starting Time"}\n");
            Process[] p = Process.GetProcesses();
            if (previous == null)
            {
                foreach (Process s in p)
                {
                    if (s.MainWindowTitle.Length > 0)
                        File.AppendAllText(file, $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) },{s.StartTime.ToString("dddd dd MMMM yyyy hh:mm:ss tt")}\n\n");
                }
                File.AppendAllText(file, "\n\n");
                previous = p;
            }
            else
            {
                foreach (Process s in p)
                {
                    isOld = true;
                    if (s.MainWindowTitle.Length > 0)
                    {
                        foreach (Process m in previous)
                        {
                            if (s.Id == m.Id)
                            {
                                isOld = true;
                                break;
                            }
                            else
                            {
                                isOld = false;
                            }
                        }
                        if (isOld)
                        {
                            string test = $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) },{s.StartTime.ToString("dddd dd MMMM yyyy hh:mm:ss tt")}\n";
                            File.AppendAllText(file, test);
                        }
                        else
                        {
                            string test = $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) },{s.StartTime.ToString("dddd dd MMMM yyyy hh:mm:ss tt")},{"New"}\n";
                            File.AppendAllText(file, test);
                            isOld = false;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
                File.AppendAllText(file, "\n\n");
                previous = p;
            }
        }

        private static Bitmap CaptureMyScreen()
        {
            try
            {
                Bitmap screenshot = new Bitmap(1920, 1080);
                //MessageBox.Show("Screen Width : " + Screen.PrimaryScreen.Bounds.Width);
                //MessageBox.Show("Screen Height : " + Screen.PrimaryScreen.Bounds.Height);
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
                //MessageBox.Show("Bounds Size : " + SystemInformation.VirtualScreen.Size);
                screenshot.Save(screen);
                if (File.Exists(screen))
                {
                    //pictureBox1.Image = null;
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(screen);
                    screenshot.Save(screen);
                    //pictureBox1.Image = Image.FromFile("Screen.jpg");
                    return screenshot;
                }
                else
                {
                    //pictureBox1.Image = Image.FromFile("Screen.jpg");
                    //screenshot.Save("Screen.jpg", ImageFormat.Jpeg);
                    return (Bitmap)Image.FromFile(screen);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Console.WriteLine(ex);
                return null;
            }
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
            MessageBox.Show("I ma in btn_Click");
            //cboCamera.SelectedIndex ;
            timer.Enabled = !(timer.Enabled);
            timer1.Enabled = !(timer1.Enabled);
            
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += videoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
           
        }




        //NewFrameEventArgs eventArgs1 = null;
        static Bitmap img = null;
        static Bitmap img1 = null;
        private void videoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //MessageBox.Show("I ma in btn_Click event");
            img = (Bitmap)eventArgs.Frame.Clone();
            img1 = img;
            //pic.Image.Save("Screen.png");
            //img = (Bitmap)pic.Image;
            //img = pic.Image;
            //img = (Bitmap)eventArgs.Frame.Clone();
            //eventArgs1 = eventArgs;

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
            //pic.Image.Save("sample.jpg");
            //pictureBox1.Image = (Image)img1;
            //pictureBox1.Image = img;
        }

        static private Bitmap GetUserImage()
        {
            //img = pic.Image;
            return img1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            Filing();
            //pictureBox1.Image = img;
        }
    }
}
