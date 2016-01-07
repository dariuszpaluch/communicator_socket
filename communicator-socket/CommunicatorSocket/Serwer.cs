﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicatorSocket
{
    public struct User
    {
        public string nick;
        public ChatWindow chat;

        public void create(string nick,Serwer serwer)
        {
            this.nick=nick;
            this.chat = new ChatWindow(nick, serwer);
        }
    }

    public class Serwer
    {
        public const int TYPE_LOGIN = 1;
        public const int TYPE_CONTACTS = 2;
        public const int TYPE_MESSAGE = 3;
        public const int TYPE_LOGOUT = 4;

        private string address;
        private string port;
        private Socket socketFd;
        private Login login;
        private MainWindow mainWindow;
        private string userToken;
        private List<User> users;

        public Serwer(string address, string port)
        {
            this.address = address;
            this.port = port;
            this.users = new List<User>();
        }

        public void sendMessage(string message, string nick)
        {
            string data = nick + ';' + "07.01.2016" + ';' + message; 
            this.sendData(TYPE_MESSAGE, data);
        }

        private void sendData(int type, string data)
        {
            Console.WriteLine("SEND: " + type + ";" + data + "|");
            byte[] byteData = Encoding.ASCII.GetBytes(type + ";" + data + "|");
            this.socketFd.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), this.socketFd);
        }

        public void loginInUser(string login, string password)
        {
            string data = login + ';' + password;
            this.sendData(TYPE_LOGIN, data);
        }

        private void handleLoginAnswer(string data)
        {
            string[] messageSplit = data.Split(';');
            string status = messageSplit[1];
            string message = messageSplit[2];

            if (Int32.Parse(status) == 1)
            {
                this.userToken = message;
                this.login.setThreadedCloseForm();
                this.mainWindow = new MainWindow(this);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                mainWindow.addContact("WOLSZTYN1");
                var t = Task.Run(() =>
                {
                    Application.Run(this.mainWindow);
                });
                mainWindow.addContact("WOLSZTYN");

            }
            else
            {
                this.login.setThreadedErrorLabel(message);
            }
        }

        private void parsingReceiveData(string data)
        {
            string[] parsingData = data.Split('|');

            for (int i = 0; i < parsingData.Length; i++)
            {
                if (parsingData[i].Length > 0)
                {
                    this.handleAnswers(parsingData[i]);
                }
            }
        }

        private void handleContacts(string data)
        {
            string[] allContacts = data.Split(';');

            for (int i = 1; i < allContacts.Length; i++)
            {
                this.mainWindow.addContact(allContacts[i]);
            }
        }

        private void handleMessage(string data)
        {
            string[] allData = data.Split(';');
            string nick = allData[1];
            string time = allData[2];
            string message = allData[3];

            ChatWindow chat = new ChatWindow(nick, this);
            chat.addMessage(time, message);
            var t = Task.Run(() =>
            {
            Application.Run(chat);
            });
            
        }

        private void handleAnswers(string data)
        {
            this.readMessages();
            Console.WriteLine("RECEIVE MESSAGE: " + data);
            int status = Int32.Parse(data.Split(';')[0]);

            switch (status)
            {
                case TYPE_LOGIN:
                    Console.WriteLine("LOGIN");
                    this.handleLoginAnswer(data);
                    break;
                case TYPE_CONTACTS:
                    Console.WriteLine("CONTACTS");
                    this.handleContacts(data);
                    break;
                case TYPE_MESSAGE:
                    Console.WriteLine("MESSAGE");
                    this.handleMessage(data);
                    break;

                default:
                    Console.WriteLine("I DON'T KNOW THIS MESSAGE");
                    break;
            }

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                /* retrieve the SocketStateObject */
                SocketStateObject state = (SocketStateObject)ar.AsyncState;
                //Socket socketFd = state.m_SocketFd;

                /* read data */
                int size = this.socketFd.EndReceive(ar);

                if (size > 0)
                {
                    state.m_StringBuilder.Append(Encoding.ASCII.GetString(state.m_DataBuf, 0, size));
                    //Console.WriteLine(state.m_StringBuilder.ToString());
                    //Console.WriteLine("Otrzymałem jakąś wiadomość");
                    string message = state.m_StringBuilder.ToString();
                    //this.receiveMessage(message);     
                    //this.handleAnswers(message);
                    this.parsingReceiveData(message);
                    this.readMessages();
                    /* get the rest of the data */
                    //socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    /* all the data has arrived */
                    if (state.m_StringBuilder.Length > 1)
                    {
                        Console.WriteLine("Otrzymałem jakąś wiadomość");
                        string message = state.m_StringBuilder.ToString();
                        //this.receiveMessage(message);
                        this.parsingReceiveData(message);
                        this.readMessages();
                    }

                    Console.WriteLine("Pusta wiadomość");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                Console.WriteLine("Check \"Server Info\" and try again!");
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                /* retrieve the socket from the state object */
                this.socketFd = (Socket)ar.AsyncState;

                /* complete the connection */
                this.socketFd.EndConnect(ar);
                
                Console.WriteLine("Connected");

                this.login = new Login(this);
                Application.Run(this.login);


            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                Console.WriteLine("Check \"Server Info\" and try again!");
            }
        }

        private void wykonujMnieCoJakisCzas(object sender, EventArgs e)
        {
            this.sendMessage("DAWAJ WIADOMOSCI", "DAREK");
        }

        private void readMessages()
        {
            SocketStateObject state = new SocketStateObject();
            state.m_SocketFd = this.socketFd;
            this.socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                this.readMessages();
                // Signal that all bytes have been sent.
                //sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                endPoint = new IPEndPoint(addresses[0], Int32.Parse(this.port));

                Console.WriteLine("Wait! Connecting...");

                /* connect to the server */
                //jeśli połączy się to odpali ConnectCallback
                socketFd.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), socketFd);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                Console.WriteLine("Check \"Server Info\" and try again!");
            }
        }

        public void connection()
        {
            Dns.BeginGetHostEntry(this.address, new AsyncCallback(GetHostEntryCallback), null);

        }

    }
}
