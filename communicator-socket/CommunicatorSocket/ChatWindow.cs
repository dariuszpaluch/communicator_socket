using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommunicatorSocket
{
    public partial class ChatWindow : Form
    {

        delegate void setThreadedAddMessageCallback(string time, string nick);
        private string allMessages;
        private Serwer serwer;
        private string nick;
        private string loginNick;
        private Form obj;
        public ChatWindow(string nick, string loginNick, Serwer serwer)
        {
            InitializeComponent();
            this.obj = this;

            this.Text = "Chat with " + nick;
            this.nick = nick;
            this.allMessages = "";
            this.serwer = serwer;
            this.loginNick = loginNick;
        }

        public void addMessage(string message)
        {
            allMessages += this.loginNick + " " + DateTime.Now.ToString("HH:mm:ss") + "\n" + message + " \n\n\n";
            this.MessagesRichTextBox.Text = this.allMessages;
            this.MessagesRichTextBox.SelectionStart = this.MessagesRichTextBox.TextLength;
            this.MessagesRichTextBox.ScrollToCaret();
        }

        public void addMessage(string time, string text)
        {
            if (this.MessagesRichTextBox.InvokeRequired)
            {
                setThreadedAddMessageCallback statusLabelCallback = new setThreadedAddMessageCallback(this.addMessage);
                this.obj.Invoke(statusLabelCallback, time, text);
            }
            else
            {
                allMessages += this.nick + " " + time + "\n" + text + " \n\n\n";
                this.MessagesRichTextBox.Text = this.allMessages;
                this.MessagesRichTextBox.SelectionStart = this.MessagesRichTextBox.TextLength;
                this.MessagesRichTextBox.ScrollToCaret();
            }
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = this.MessageTextBox.Text;
            this.MessageTextBox.Text = "";
            this.serwer.sendMessage(message, this.nick);
            this.addMessage(message);
        }

        public void funData(String text)
        {
            MessageLabel.Text = text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {

        }

        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.serwer.removeUser(this.nick);
        }

        private void MessageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                string message = this.MessageTextBox.Text;
                this.MessageTextBox.Text = "";
                this.serwer.sendMessage(message, this.nick);
                this.addMessage(message);

                e.Handled = true;
            }
        }
    }
}
