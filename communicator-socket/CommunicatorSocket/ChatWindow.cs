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
    public partial class ChatWindow : Form
    {

        delegate void setThreadedAddMessageCallback(string time, string nick);
        delegate void setThreadedSetMessageCallback(string allMessages);
        private string allMessages;
        private Serwer serwer;
        private string nick;
        private string loginNick;
        private User user;
        private Form obj;
        private CancellationTokenSource tokenSource;
        public ChatWindow(string nick, string loginNick, Serwer serwer, string allMessages, User user, CancellationTokenSource tokenSource)
        {
            InitializeComponent();
            this.obj = this;

            this.Text = "Chat with " + nick;
            this.nick = nick;
            this.allMessages = allMessages;
            this.serwer = serwer;
            this.loginNick = loginNick;
            Console.WriteLine(allMessages);
            this.setMessages(this.allMessages);
            this.user = user;
            this.tokenSource = tokenSource;
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
                this.allMessages += this.nick + " " + time + "\n" + text + " \n\n\n";
                this.MessagesRichTextBox.Text = this.allMessages;
                this.MessagesRichTextBox.SelectionStart = this.MessagesRichTextBox.TextLength;
                this.MessagesRichTextBox.ScrollToCaret();
            }
        }

        public void setMessages(string allMessages)
        {
            if (this.MessagesRichTextBox.InvokeRequired)
            {
                setThreadedSetMessageCallback statusLabelCallback = new setThreadedSetMessageCallback(this.setMessages);
                this.obj.Invoke(statusLabelCallback, allMessages);
            }
            else
            {
                this.MessagesRichTextBox.Text = allMessages;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.sendMessage();
        }

        public void sendMessage()
        {
            string message = this.MessageTextBox.Text;

                if (message.Length > 0) {
                    this.MessageTextBox.Text = "";
                    this.serwer.sendMessage(message, this.nick);
                    this.addMessage(message);
                    this.MessageTextBox.Focus();
                }

        }

        public void funData(String text)
        {
            MessageLabel.Text = text;
        }

        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.user.saveMessages(this.allMessages);
            this.serwer.removeUser(this.nick);
            tokenSource.Dispose();
        }

        private void MessageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                this.sendMessage();

                e.Handled = true;
            }
        }

        private void ChatWindow_Shown(object sender, EventArgs e)
        {
            this.MessageTextBox.Focus();
        }

        private void ChatWindow_Enter(object sender, EventArgs e)
        {
            this.MessageTextBox.Focus();

        }

        private void ChatWindow_Activated(object sender, EventArgs e)
        {
            this.MessageTextBox.Focus();

        }

        private void MessagesRichTextBox_TextChanged(object sender, EventArgs e)
        {
            this.MessageTextBox.Focus();
        }

        private void MessagesRichTextBox_Click(object sender, EventArgs e)
        {
            this.MessageTextBox.Focus();
        }

        private void MessagesRichTextBox_EnabledChanged(object sender, EventArgs e)
        {
            this.MessageTextBox.Focus();

        }
    }
}
