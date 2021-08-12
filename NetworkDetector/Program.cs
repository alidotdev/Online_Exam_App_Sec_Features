using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkDetector
{
    class Program
    {
        static System.Speech.Synthesis.SpeechSynthesizer sythesizer = new System.Speech.Synthesis.SpeechSynthesizer();
        static void Main(string[] args)
        {
            Console.WriteLine("Connection : " + NetworkConnectionDetector());
            if (NetworkConnectionDetector())
            {
                System.Diagnostics.Process.Start("ipconfig", "/release"); //For disabling internet
                sythesizer.Speak("Network has been disconnected");
            }
            else
            {
                System.Diagnostics.Process.Start("ipconfig", "/renew"); //For enabling internet
                //Thread.Sleep(5000);
                //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
                if (NetworkInterface.GetIsNetworkAvailable())
                    sythesizer.Speak("Network has been connected");
                else
                {
                    sythesizer.Speak("Network has no internet access");
                }
            }
        }
       
        static public bool NetworkConnectionDetector()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}