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
        delegate void setThreadedAddContactCallback(string nick);
        Form obj;
        private Serwer serwer;
        public MainWindow(Serwer serwer)
        {
            InitializeComponent();
            this.obj = this;
            this.serwer = serwer;
        }
        public void addContact(string text)
        {
            if (this.ContactsListBox.InvokeRequired)
            {
                setThreadedAddContactCallback statusLabelCallback = new setThreadedAddContactCallback(addContact);
                this.obj.Invoke(statusLabelCallback, text);
            }
            else
            {
                this.ContactsListBox.Items.Add(text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
