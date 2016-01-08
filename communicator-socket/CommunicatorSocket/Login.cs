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

        public Login(Serwer serwer)
        {
            InitializeComponent();
            this.serwer = serwer;
            this.obj = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = this.LoginTextBox.Text;
            string password = this.PasswordTextBox.Text;

            this.LoginTextBox.Text = "";
            this.PasswordTextBox.Text = "";

            serwer.connection(login, password, this.AddressTextBox.Text, this.PortTextBox.Text);
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
            //this.serwer.logoutCall();
        }
    }
}
