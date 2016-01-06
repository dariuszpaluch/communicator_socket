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
        public ChatWindow(string text)
        {
            InitializeComponent();
            this.Text = text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

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
