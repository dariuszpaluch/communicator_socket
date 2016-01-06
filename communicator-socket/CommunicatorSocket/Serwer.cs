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

namespace CommunicatorSocket
{
    public class Serwer
    {
        private string address;
        private string port;
        private Socket socketFd;

        public Serwer(string address, string port)
        {
            this.address = address;
            this.port = port;
        }

        public void wyswietl() {
            Console.WriteLine("DAREK SUPER MAN");
        }


        private void receiveMessage(string message) {

            Console.WriteLine(message);
            string[] messageSplit = message.Split(';');
            string status = messageSplit[0];
            string name = messageSplit[1];
            string time = messageSplit[2];
            string userMessage = messageSplit[3];

            //Console.WriteLine("Done.");
            ChatWindow chat = new ChatWindow(name, this);

            //////delPassData del = new delPassData(chat.funData);
            //////chat.Show();
            ////chat.Show();
            Application.Run(chat);
            //chat.addMessage("SUPER MAN");

            ////del("DAREK");


            ///* shutdown and close socket */
            ////socketFd.Shutdown(SocketShutdown.Both);
            ////socketFd.Close();
        }

        public void sendMessage(string message, string nick)
        {
            //Console.WriteLine("WYSYŁAM WIADOMOSC: " + message);
            byte[] byteData = Encoding.ASCII.GetBytes("WIADOMOSC OD: " + nick + "Tresc:" + message);
            //byte[] byteData = Encoding.ASCII.GetBytes("117225");
            this.socketFd.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), this.socketFd);

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
                    this.receiveMessage(message);         
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
                        this.receiveMessage(message);
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

                /* create the SocketStateObject */
                SocketStateObject state = new SocketStateObject();
                state.m_SocketFd = this.socketFd;
                //SocketStateObject state1 = new SocketStateObject();
                //state1.m_SocketFd = this.socketFd;
                //SocketStateObject state2 = new SocketStateObject();
                //state2.m_SocketFd = this.socketFd;
                //SocketStateObject state3 = new SocketStateObject();
                //state3.m_SocketFd = this.socketFd;
                

                Console.WriteLine("Connected");

                ChatWindow chat = new ChatWindow("Darek", this);

                ////delPassData del = new delPassData(chat.funData);
                ////chat.Show();
                //chat.Show();
                //System.Timers.Timer odmierzacz = new System.Timers.Timer(); //Tworzenie obiektu
                //odmierzacz.Interval = 5000; //Ustawienie przerywania na 1000ms (1s)
                //odmierzacz.Elapsed += new ElapsedEventHandler(this.wykonujMnieCoJakisCzas); //Przypisanie metody
                //odmierzacz.Start(); //Start timera
                this.readMessages();
                Application.Run(chat);

              
                //Console.ReadKey();

                //chat.addMessage("SUPER MAN");

                /* begin receiving the data */
                //odczytywanie danych

                //this.socketFd.BeginReceive(state1.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state1);
                //this.socketFd.BeginReceive(state2.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state2);
                //while (true)
                //{
                //this.socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0, new AsyncCallback(ReceiveCallback), state);

                //}
                //byte[] byteData = Encoding.ASCII.GetBytes("DAREK WYSLANE\0");
                //this.socketFd.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), this.socketFd);

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
