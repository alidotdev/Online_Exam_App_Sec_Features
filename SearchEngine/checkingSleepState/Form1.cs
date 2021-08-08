using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Speech.Recognition.DictationGrammar;

namespace checkingSleepState
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //System.Diagnostics.Process.Start("https://www.google.com/");
            //SystemEvents.PowerModeChanged += OnPowerChange;
        }



        //private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        //{

        //    switch (e.Mode)
        //    {
        //        case PowerModes.Resume:
        //            richTextBox1.Text += "\nawaking";
        //            break;
        //        case PowerModes.Suspend:
        //            richTextBox1.Text += "\nSleeping";
        //            break;
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            //to search on google, type "google" instead of "youtube"
            if (String.IsNullOrEmpty(richTextBox1.Text.Trim()))
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/");
            }
            else
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/search?q=" + richTextBox1.Text);
            }
            richTextBox1.Text= "";
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
