using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicatorSocket
{
    public partial class MainWindow : Form
    {
        delegate void setThreadeSetAllContactsCallback(string contacts);
        Form obj;
        private Serwer serwer;
        private List<string> allContacts;
        private string loginNick;
        private CancellationTokenSource tokenSource;
        public MainWindow(Serwer serwer, string loginNick, CancellationTokenSource tokenSource)
        {
            InitializeComponent();
            this.obj = this;
            this.serwer = serwer;
            this.allContacts = new List<string>();
            this.loginNick = loginNick;
            Console.WriteLine(loginNick);
            this.Text = "Communicator - user " + this.loginNick;
            this.tokenSource = tokenSource;
        }

        public void setContacts(string data)
        {
            if (this.ContactsListBox.InvokeRequired)
            {
                setThreadeSetAllContactsCallback setContactsCallback = new setThreadeSetAllContactsCallback(setContacts);
                this.obj.Invoke(setContactsCallback, data);
            }
            else
            {
                this.ContactsListBox.Items.Clear();
                string[] contacts = data.Split(';');
                for (int i = 1; i < contacts.Length; i++)
                {
                    this.ContactsListBox.Items.Add(contacts[i]);
                    this.allContacts.Add(contacts[i]);
                }
            }
        }

        private void MessageButton_Click(object sender, EventArgs e)
        {
            this.serwer.openMessageWindow(this.ContactsListBox.SelectedItem.ToString());
        }

        private void ContactsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.serwer.openMessageWindow(this.ContactsListBox.SelectedItem.ToString());
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.serwer.logoutCall();
            this.tokenSource.Dispose();
        }

    }
}
