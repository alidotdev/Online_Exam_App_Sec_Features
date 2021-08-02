using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using WK.Libraries.SharpClipboardNS;
using WK.Libraries.SharpClipboardNS;

namespace DisablingCommands
{
    public partial class Form1 : Form
    {
        //private Keys[] DisabledKeys;

        public Form1()
        {
            InitializeComponent();
        }

        //private void AddDisableKeys()
        //{
        //    Keys copyCmd = Keys.Control | Keys.C;
        //    Keys cutCmd = Keys.Control | Keys.X;
        //    Keys pasteCmd = Keys.Control | Keys.V;
        //    Keys lStartBtn = Keys.LWin;
        //    Keys rStartBtn = Keys.RWin;
        //    Keys altF4Cmd = Keys.Alt | Keys.F4;
        //    DisabledKeys = new Keys[] { copyCmd, cutCmd, pasteCmd, lStartBtn, rStartBtn, altF4Cmd };
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys[] disabledKeys = new Keys[]
            {
                Keys.Control | Keys.C,
                Keys.Control | Keys.X,
                Keys.Control | Keys.V,
                Keys.Alt | Keys.F4
            };
            if (disabledKeys.Contains(keyData))
            {
                return true;
            }
            return false;
        }
        /**********************************************************************************/

        //helping link: https://github.com/Willy-Kimura/SharpClipboard
        //https://stackoverflow.com/questions/621577/how-do-i-monitor-clipboard-changes-in-c

        private void sharpClipboard1_ClipboardChanged(object sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            // Is the content copied of text type?
            if (e.ContentType == SharpClipboard.ContentTypes.Text)
            {
                // Get the cut/copied text.
               richTextBox1.Text += "--\n--" +sharpClipboard1.ClipboardText;
                
            }

            // Is the content copied of image type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Image)
            {
                // Get the cut/copied image.
                Image img = sharpClipboard1.ClipboardImage;
                pictureBox1.Image = img;
            }

            // Is the content copied of file type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Files)
            {
                // Get the cut/copied file/files.
               richTextBox1.Text += "--\n--" +sharpClipboard1.ClipboardFiles.ToArray();

                // ...or use 'ClipboardFile' to get a single copied file.
               richTextBox1.Text += "--\n--" +sharpClipboard1.ClipboardFile;
            }

            // If the cut/copied content is complex, use 'Other'.
            else if (e.ContentType == SharpClipboard.ContentTypes.Other)
            {
                // Do something with 'sharpClipboard1.ClipboardObject' or 'e.Content' here...
            }
        }

        /**********************************************************************************/
    }
}



