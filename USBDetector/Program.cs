using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace USBDetector
{
    class Program
    {
        //DriveInfo[] allDrives = DriveInfo.GetDrives();
        static void Main(string[] args)
        {
            USBdetector();
        }

        public static bool USBdetector()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            SpeechSynthesizer synth = new SpeechSynthesizer();
            bool flag = false;
            Console.WriteLine(allDrives);
            foreach (DriveInfo d in allDrives)
            {

                if (d.DriveType == (DriveType)2)
                {

                    if (d.IsReady == true)
                    {
                    synth.Speak("REmoveable device name " + d.Name + " is found");
                        Console.WriteLine("Drive {0}", d.Name);
                        Console.WriteLine("  Drive type: {0}", d.DriveType);
                        Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                        Console.WriteLine("  File system: {0}", d.DriveFormat);
                        Console.WriteLine(
                            "  Available space to current user:{0, 15} bytes",
                            d.AvailableFreeSpace);

                        Console.WriteLine(
                            "  Total available space:          {0, 15} bytes",
                            d.TotalFreeSpace);

                        Console.WriteLine(
                            "  Total size of drive:            {0, 15} bytes ",
                            d.TotalSize);
                    }
                    flag = true;
                    //return flag;
                }
            }
            if (!flag)
            {
                synth.Speak("No removeable device found");
                return false;
            }
            return true;
        }
    }
}
