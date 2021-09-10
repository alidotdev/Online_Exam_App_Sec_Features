using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace NetworkConnection
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer synth = new SpeechSynthesizer();
        public Form1()
        {
            InitializeComponent();
        }

        private void checkFunction()
        {
            //NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler((sender, e) => AvailabilityChangedCallback(sender, e));

        }

        private void AvailabilityChangedCallback(object sender, NetworkAvailabilityEventArgs e)
        {
            synth.Speak("I am in function of network checking");
            if (e.IsAvailable)
            {
                //panel1.ForeColor = Color.Green;
                panel2.BackColor = Color.Green;
                synth.Speak("Network is available");
                //Internet Connection is available   
            }
            else
            {
                panel2.BackColor = Color.Red;
                synth.Speak("Network is not available");
            }
        }

        private void button1_Click(object sender1, EventArgs e1)
        {
            //synth.Speak("I am going to check network connection");
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler((sender, e) => AvailabilityChangedCallback(sender, e));
        }
    }
}
