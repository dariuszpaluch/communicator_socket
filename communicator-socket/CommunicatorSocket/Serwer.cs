using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace CommunicatorSocket
{
    class Serwer
    {
        private string address;
        private string port;

        public Serwer(string address, string port)
        {
            this.address = address;
            this.port = port;
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

                        Console.WriteLine("Done.");
                        ChatWindow chat = new ChatWindow(name);

                        //delPassData del = new delPassData(chat.funData);
                        //chat.Show();
                        Application.Run(chat);
                        //del("DAREK");


                        /* shutdown and close socket */
                        //socketFd.Shutdown(SocketShutdown.Both);
                        //socketFd.Close();
                    }
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
                Socket socketFd = (Socket)ar.AsyncState;

                /* complete the connection */
                socketFd.EndConnect(ar);

                /* create the SocketStateObject */
                SocketStateObject state = new SocketStateObject();
                state.m_SocketFd = socketFd;
                SocketStateObject state1 = new SocketStateObject();
                state1.m_SocketFd = socketFd;
                SocketStateObject state2 = new SocketStateObject();
                state2.m_SocketFd = socketFd;
                SocketStateObject state3 = new SocketStateObject();
                state3.m_SocketFd = socketFd;
                

                Console.WriteLine("Wait! Reading...");

                /* begin receiving the data */
                //odczytywanie danych
                socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state);
                socketFd.BeginReceive(state1.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state1);
                socketFd.BeginReceive(state2.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state2);

            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                Console.WriteLine("Check \"Server Info\" and try again!");
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
