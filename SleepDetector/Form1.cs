using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SleepDetector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SystemEvents.PowerModeChanged += OnPowerChange;

        }
        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            System.Speech.Synthesis.SpeechSynthesizer sythesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            switch (e.Mode)
            {
                case PowerModes.StatusChange:
                    sythesizer.Speak("System charging status is changing");

                    break;
                case PowerModes.Suspend:
                    sythesizer.Speak("System is going to sleep");
                    break;
                case PowerModes.Resume:
                    //MessageBox.Show("I am awaiking the system");
                    MessageBox.Show(sythesizer.ToString());
                    if (sythesizer == null)
                    {
                        //MessageBox.Show("In condition");
                    sythesizer = new System.Speech.Synthesis.SpeechSynthesizer();
                    }
                    //Thread.Sleep(1000);
                    sythesizer.Speak("System is awaking");

                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("ms-settings:clipboard");
        }
    }
}
