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
        private string allMessages;
        private Serwer serwer;
        private string nick;
        public ChatWindow(string nick, Serwer serwer)
        {
            InitializeComponent();
            this.Text = nick;
            this.nick = nick;
            serwer.wyswietl();
            this.allMessages = "";
            this.serwer = serwer;
        }

        public void addMessage(string message)
        {
            allMessages += " " + message + " \n\n";
            this.MessagesRichTextBox.Text = this.allMessages;
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
    }
}
