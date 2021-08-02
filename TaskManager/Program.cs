using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager
{
    class Program
    {
        //[DllImport("kernel32.dll")]
        //static extern IntPtr GetConsoleWindow();

        //[DllImport("user32.dll")]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //const int SW_HIDE = 0;
        //const int SW_SHOW = 5;



        /// <summary>
        /// 
        /// </summary>
        /// 
        static Process[] previous = null;
        static string dateFormat = "dddd dd MMMM yyyy hh:mm:ss tt";
        public static void Main(string[] args)
        {
            //var handle = GetConsoleWindow();
            //ShowWindow(handle, SW_HIDE);
            while (true)
            {
                TimeSpan t = new TimeSpan(0, 0, 5);
                Filing();
                Thread.Sleep(t);
            //ShowWindow(handle, SW_SHOW);
                //Thread.Sleep(t);
            }

            // Hide

            // Show
        }
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
                            Console.WriteLine("New : "+ s.MainWindowTitle);

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

