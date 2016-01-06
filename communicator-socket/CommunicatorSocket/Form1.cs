﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

/*
 * $Id: Form1.cs,v 1.1 2006/10/24 19:32:59 mkalewski Exp $
 */

namespace CommunicatorSocket
{
    public partial class Form1 : Form
    {
        private Form obj;
        delegate void setThreadedTextBoxCallback(String text);
        delegate void setThreadedStatusLabelCallback(String text);
        delegate void setThreadedButtonCallback(bool status);
        public delegate void delPassData(String text);

        public Form1()
        {
            InitializeComponent();
            this.obj = this;
        }

        private void setThreadedTextBox(String text)
        {
            if (this.textBoxDate.InvokeRequired)
            {
                setThreadedTextBoxCallback textBoxCallback = new setThreadedTextBoxCallback(setThreadedTextBox);
                this.obj.Invoke(textBoxCallback, text);
            }
            else
            {
                //text = text.Substring(0, text.Length - 2);
                this.textBoxDate.Text = text;
            }
        }

        public void setThreadedStatusLabel(String text)
        {
            if (this.statusStrip.InvokeRequired)
            {
                setThreadedStatusLabelCallback statusLabelCallback = new setThreadedStatusLabelCallback(setThreadedStatusLabel);
                this.obj.Invoke(statusLabelCallback, text);
            }
            else
            {
                this.toolStripStatusLabel1.Text = text;
            }
        }

        private void setThreadedButton(bool status)
        {
            if (this.buttonGetDate.InvokeRequired) // sprawdzenie czy to z tego wątku czy z innego
            {
                setThreadedButtonCallback buttonCallback = new setThreadedButtonCallback(setThreadedButton);
                this.obj.Invoke(buttonCallback, status);
            }
            else
            {
                this.buttonGetDate.Enabled = status;
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                /* retrieve the SocketStateObject */
                SocketStateObject state = (SocketStateObject)ar.AsyncState;
                Socket socketFd = state.m_SocketFd;

                /* read data */
                int size = socketFd.EndReceive(ar);

                if (size > 0)
                {
                    state.m_StringBuilder.Append(Encoding.ASCII.GetString(state.m_DataBuf, 0, size));

                    /* get the rest of the data */
                    socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    /* all the data has arrived */
                    if (state.m_StringBuilder.Length > 1)
                    {
                        string message = state.m_StringBuilder.ToString();
                        string[] messageSplit = message.Split(';');
                        string status = messageSplit[0];
                        string name = messageSplit[1];
                        string userMessage = messageSplit[2];

                        setThreadedTextBox(message);
                        setThreadedStatusLabel("Done.");
                        setThreadedButton(true);
                        ChatWindow chat = new ChatWindow(name);

                        delPassData del = new delPassData(chat.funData);
                        del("SUPER MAN");
                        //chat.Show();
                        Application.Run(chat);
                        del("DAREK");


                        /* shutdown and close socket */
                        socketFd.Shutdown(SocketShutdown.Both);
                        socketFd.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                setThreadedStatusLabel("Check \"Server Info\" and try again!");
                setThreadedButton(true);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                /* retrieve the socket from the state object */
                Socket socketFd = (Socket)ar.AsyncState;

                /* complete the connection */
                socketFd.EndConnect(ar);

                /* create the SocketStateObject */
                SocketStateObject state = new SocketStateObject();
                state.m_SocketFd = socketFd;

                setThreadedStatusLabel("Wait! Reading...");

                /* begin receiving the data */
                //odczytywanie danych
                socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                setThreadedStatusLabel("Check \"Server Info\" and try again!");
                setThreadedButton(true);
            }
        }

        private void GetHostEntryCallback(IAsyncResult ar)
        {
            try
            {
                IPHostEntry hostEntry = null;
                IPAddress[] addresses = null;
                Socket socketFd = null;
                IPEndPoint endPoint = null;

                /* complete the DNS query */
                hostEntry = Dns.EndGetHostEntry(ar);
                addresses = hostEntry.AddressList;

                /* create a socket */
                socketFd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                /* remote endpoint for the socket */
                endPoint = new IPEndPoint(addresses[0], Int32.Parse(this.textBoxPort.Text.ToString()));

                setThreadedStatusLabel("Wait! Connecting...");

                /* connect to the server */
                //jeśli połączy się to odpali ConnectCallback
                socketFd.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), socketFd);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                setThreadedStatusLabel("Check \"Server Info\" and try again!");
                setThreadedButton(true);
            }
        }

        private void buttonGetDate_Click(object sender, EventArgs e)
        {
            try
            {
                setThreadedButton(false);
                setThreadedTextBox("");
                setThreadedStatusLabel("Wait! DNS query...");

                //jeśli wpisany port i ip
                if (this.textBoxAddr.Text.Length > 0 && this.textBoxPort.Text.Length > 0)
                {
                    /* get DNS host information */
                    Dns.BeginGetHostEntry(this.textBoxAddr.Text.ToString(), new AsyncCallback(GetHostEntryCallback), null);
                }
                else
                {
                    if (this.textBoxAddr.Text.Length <= 0) MessageBox.Show("No server address!");
                    else
                        if (this.textBoxPort.Text.Length <= 0) MessageBox.Show("No server port number!");
                    setThreadedButton(true);
                    setThreadedStatusLabel("Check \"Server Info\" and try again!");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                setThreadedStatusLabel("Check \"Server Info\" and try again!");
                setThreadedButton(true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxAddr_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class SocketStateObject
    {
        public const int BUF_SIZE = 1024;
        public byte[] m_DataBuf = new byte[BUF_SIZE];
        public StringBuilder m_StringBuilder = new StringBuilder();
        public Socket m_SocketFd = null;
    }
}