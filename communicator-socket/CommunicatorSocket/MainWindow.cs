using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommunicatorSocket
{
    public partial class MainWindow : Form
    {
        delegate void setThreadedAddContactCallback(String nick);
        Form obj;
        private Serwer serwer;
        public MainWindow(Serwer serwer)
        {
            InitializeComponent();
            this.obj = this;
            this.serwer = serwer;
        }
        public void addContact(String text)
        {
            if (this.UsersRichTextBox.InvokeRequired)
            {
                setThreadedAddContactCallback statusLabelCallback = new setThreadedAddContactCallback(addContact);
                this.obj.Invoke(statusLabelCallback, text);
            }
            else
            {
                this.UsersRichTextBox.Text += text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
