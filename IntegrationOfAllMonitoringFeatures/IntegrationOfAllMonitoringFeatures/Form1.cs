using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationOfAllMonitoringFeatures
{
    public partial class Form1 : Form
    {
        Bitmap image = null;
        public Form1()
        {
            InitializeComponent();
        }

        public enum State
        {
            Hiding,
            Filling_With_Zeros
        };

        /*private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            img = (Bitmap)pictureBox1.Image;
            pictureBox2.Image = embedText(str, img);
            MessageBox.Show("Text : " + str + "\n is added to iamge");
            textBox1.Text = string.Empty;
        }*/

        public Bitmap embedText(string text, Bitmap bmp)
        {
            // initially, we'll be hiding characters in the image
            State state = State.Hiding;

            // holds the index of the character that is being hidden
            int charIndex = 0;

            // holds the value of the character converted to integer
            int charValue = 0;

            // holds the index of the color element (R or G or B) that is currently being processed
            long pixelElementIndex = 0;

            // holds the number of trailing zeros that have been added when finishing the process
            int zeros = 0;

            // hold pixel elements
            int R = 0, G = 0, B = 0;

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    // holds the pixel that is currently being processed
                    Color pixel = bmp.GetPixel(j, i);

                    // now, clear the least significant bit (LSB) from each pixel element
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        // check if new 8 bits has been processed
                        if (pixelElementIndex % 8 == 0)
                        {
                            // check if the whole process has finished
                            // we can say that it's finished when 8 zeros are added
                            if (state == State.Filling_With_Zeros && zeros == 8)
                            {
                                // apply the last pixel on the image
                                // even if only a part of its elements have been affected
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }

                                // return the bitmap with the text hidden in
                                return bmp;
                            }

                            // check if all characters has been hidden
                            if (charIndex >= text.Length)
                            {
                                // start adding zeros to mark the end of the text
                                state = State.Filling_With_Zeros;
                            }
                            else
                            {
                                // move to the next character and process again
                                charValue = text[charIndex++];
                            }
                        }

                        // check which pixel element has the turn to hide a bit in its LSB
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (state == State.Hiding)
                                    {
                                        // the rightmost bit in the character will be (charValue % 2)
                                        // to put this value instead of the LSB of the pixel element
                                        // just add it to it
                                        // recall that the LSB of the pixel element had been cleared
                                        // before this operation
                                        R += charValue % 2;

                                        // removes the added rightmost bit of the character
                                        // such that next time we can reach the next one
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (state == State.Hiding)
                                    {
                                        G += charValue % 2;

                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (state == State.Hiding)
                                    {
                                        B += charValue % 2;

                                        charValue /= 2;
                                    }

                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.Filling_With_Zeros)
                        {
                            // increment the value of zeros until it is 8
                            zeros++;
                        }
                    }
                }
            }

            return bmp;
        }

        public string extractText(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            // holds the text that will be extracted from the image
            string extractedText = String.Empty;

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    // get the LSB from the pixel element (will be pixel.R % 2)
                                    // then add one bit to the right of the current character
                                    // this can be done by (charValue = charValue * 2)
                                    // replace the added bit (which value is by default 0) with
                                    // the LSB of the pixel element, simply by addition
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added, then add the current character to the result text
                        if (colorUnitIndex % 8 == 0)
                        {
                            // reverse? of course, since each time the process happens on the right (for simplicity)
                            charValue = reverseBits(charValue);

                            // can only be 0 if it is the stop character (the 8 zeros)
                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            // convert the character value from int to char
                            char c = (char)charValue;

                            // add the current character to the result text
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }
        public int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }


        public string GetHostName ()
        {
            return Dns.GetHostName();
        }

        [Obsolete]
        public string GetIPAddress ()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }

        public  string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }

        public Image CaptureMyScreen()
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
                //screenshot.Save("Screen.jpg");
                return screenshot;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private static string cameraName;
        public string GetCameraName()
        {
            string path = @"..\..\..\Program Files\CameraName.txt";
            try
            {
                if (!File.Exists(path))
                {
                    //MessageBox.Show("file does not exists");
                    throw new Exception();
                }

                string cameraName = File.ReadAllText(path);

                foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
                {
                    //MessageBox.Show( cameraName.Trim() +"\n" + filterInfo.MonikerString );
                    if (string.Equals(cameraName.Trim(), filterInfo.MonikerString))
                    {
                        return cameraName.Trim();
                    }
                }

                throw new Exception();

                //MessageBox.Show(cameraName);
            }
            catch (Exception ex)
            {
                if (File.Exists(path))
                    File.Delete(path);
                return null;
            }
        }

        /*public Image ClickCameraImage ()
        {
            if (String.IsNullOrEmpty(cameraName))
            {
                return null;
            }
            else
            {
                VideoCaptureDevice videoCaptureDevice = new VideoCaptureDevice(cameraName);
                videoCaptureDevice.Start();
                
            }
        }*/

        static Process[] previous = null;
        static string dateFormat = "dddd dd MMMM yyyy hh:mm:ss tt";

        static void Filing()
        {
            bool isOld = false;

            File.AppendAllText("Data.csv", $"{"Date and Time : "},{DateTime.Now.ToString(dateFormat)}\n\n");
            File.AppendAllText("Data.csv", $"{"Applications"},{"RAM Used in MB"},{"Starting Time"}\n");
            Process[] p = Process.GetProcesses();



            if (previous == null)
            {
                foreach (Process s in p)
                {
                    if (s.MainWindowTitle.Length > 0)
                        File.AppendAllText("Data.csv", $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) + "MB"},{s.StartTime.ToString(dateFormat)}\n");
                }
                File.AppendAllText("Data.csv", "\n\n");

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

                            string test = $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) + "MB"},{s.StartTime.ToString(dateFormat)}\n";

                            File.AppendAllText("Data.csv", test);
                        }
                        else
                        {

                            string test = $"{ s.MainWindowTitle },{ String.Format("{0:0.00}", (((double)s.WorkingSet64) / 1024 / 1024)) + "MB"},{s.StartTime.ToString(dateFormat)},{"New"}\n";
                            Console.WriteLine("New : " + s.MainWindowTitle);

                            File.AppendAllText("Data.csv", test);
                            isOld = false;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
                File.AppendAllText("Data.csv", "\n\n");
                previous = p;
            }

        }

    }
}