using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommunicatorSocket
{
    public partial class Login : Form
    {
        private Form obj;
        private Serwer serwer;
        delegate void setThreadedStatusLabelCallback(String text);
        delegate void setThreadedClose();
        bool loginInStatus;

        public Login(Serwer serwer)
        {
            InitializeComponent();
            this.serwer = serwer;
            this.obj = this;
            this.loginInStatus = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = this.LoginTextBox.Text;
            string password = this.PasswordTextBox.Text;

            this.changeEnabledAllItems(false);
            if (!serwer.getConnecting())
            {
                serwer.connection(login, password, this.AddressTextBox.Text, this.PortTextBox.Text);
            }
            else
            {
                serwer.loginInUser(login, password);
            }
        }

        private void changeEnabledAllItems(bool status)
        {
            this.LoginTextBox.Enabled = status;
            this.PasswordTextBox.Enabled = status;
            this.AddressTextBox.Enabled = status;
            this.PortTextBox.Enabled = status;
            this.LoginInButton.Enabled = status;
        }

        public void setThreadedErrorLabel(String text)
        {
            if (this.ErrorLabel.InvokeRequired)
            {
                setThreadedStatusLabelCallback statusLabelCallback = new setThreadedStatusLabelCallback(setThreadedErrorLabel);
                this.obj.Invoke(statusLabelCallback, text);
            }
            else
            {
                this.ErrorLabel.Text = text;
                this.ErrorLabel.Visible = true;
                this.changeEnabledAllItems(true);
            }
        }

        public void setThreadedCloseForm()
        {
            if (this.InvokeRequired)
            {
                setThreadedClose statusLabelCallback = new setThreadedClose(setThreadedCloseForm);
                this.obj.Invoke(statusLabelCallback);
            }
            else
            {
                this.Close();
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.loginInStatus)
                this.serwer.closeConnection();
        }

        public void setLoginInStatus(bool status)
        {
            this.loginInStatus = status;
        }
    }
}
