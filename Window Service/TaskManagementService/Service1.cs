using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace TaskManagementService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new         System.Timers.Timer();
        static Process[] previous = null;
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in             milisecinds  
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            String file = "E:/Data.csv";
            File.AppendAllText("Record.csv",$"{Message}\n");
            bool isOld = false;
            File.AppendAllText(file, $"{"Date and Time : "},{DateTime.Now},,{"Screen short"},{CaptureMyScreen()}\n\n");
            File.AppendAllText(file, $"{"Applications"},{"RAM Used in MB"},{"Starting Time"}\n");
            Process[] p = Process.GetProcesses();
            if (previous == null)
            {
                foreach (Process s in p)
                {
                    if (s.MainWindowTitle.Length > 0)
                        File.AppendAllText(file, $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) + "MB"},{s.StartTime.ToString("dddd dd MMMM yyyy hh:mm:ss tt")}\n");
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
                            string test = $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) + "MB"},{s.StartTime.ToString("dddd dd MMMM yyyy hh:mm:ss tt")}\n";
                            File.AppendAllText(file, test);
                        }
                        else
                        {
                            string test = $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) + "MB"},{s.StartTime.ToString("dddd dd MMMM yyyy hh:mm:ss tt")},{"New"}\n";

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
        private static Image CaptureMyScreen()
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
                screenshot.Save("Screen.jpg");
                if (File.Exists("Screen.jpg"))
                {
                    //pictureBox1.Image = null;
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete("Screen.jpg");
                    screenshot.Save("Screen.jpg", ImageFormat.Jpeg);
                    //pictureBox1.Image = Image.FromFile("Screen.jpg");
                    return screenshot;
                }
                else
                {
                    //pictureBox1.Image = Image.FromFile("Screen.jpg");
                    //screenshot.Save("Screen.jpg", ImageFormat.Jpeg);
                    return screenshot;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}

