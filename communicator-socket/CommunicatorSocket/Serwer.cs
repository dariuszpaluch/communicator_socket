using System;
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
        private CancellationTokenSource mainWindowTokenSource;
        private CancellationTokenSource loginWindowTokenSource;
        private MainWindow mainWindow;
        private List<User> users;
        public bool work;
        private string loginNick;
        private string password;
        private bool connecting;

        public Serwer()
        {
            this.users = new List<User>();
            this.work = true;
            this.connecting = false;
            this.mainWindowTokenSource = new CancellationTokenSource();
            this.loginWindowTokenSource = new CancellationTokenSource();
        }

        public void showLoginWindow()
        {
            var t = Task.Run(() =>
            {
                this.login = new Login(this);
                Application.Run(this.login);
            });
        }

        public void sendMessage(string message, string nick)
        {
            string data = nick + ';' + message; 
            this.sendData(TYPE_MESSAGE, data);
        }

        private void sendData(int type, string data)
        {
            Console.WriteLine("SEND: " + type + ";" + data + "|");
            byte[] byteData = Encoding.ASCII.GetBytes(type + ";" + data + "|");
            this.socketFd.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), this.socketFd);
        }

        public void loginInUser(string loginNick, string password)
        {
            this.loginNick = loginNick;
            this.password = password;
            string data = loginNick + ';' + password;
            this.sendData(TYPE_LOGIN, data);
        }

        public bool getConnecting()
        {
            return this.connecting;
        }

        private void handleLoginAnswer(string data)
        {
            string[] messageSplit = data.Split(';');
            int status = Int32.Parse(messageSplit[1]);

            if (status == 1)
            {
                this.login.setLoginInStatus(true);
                this.login.setThreadedCloseForm();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var token = this.mainWindowTokenSource.Token;
                this.mainWindow = new MainWindow(this, this.loginNick, this.mainWindowTokenSource);
                var t = Task.Run(() =>
                {
                    Application.Run(this.mainWindow);
                }, token);
                t.Wait();

            }
            else
            {
                if (messageSplit.Length > 2)
                    this.login.setThreadedErrorLabel(messageSplit[2]);
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
            this.mainWindow.setContacts(data);
        }

        private void handleMessage(string data)
        {
            string[] allData = data.Split(';');
            string nick = allData[1];
            string time = allData[2];
            string message = allData[3];

            bool exist = false;
            for (int i = 0; i < this.users.Count; i++)
            {
                if (this.users[i].nick == nick)
                {
                    exist = true;
                    this.users[i].addMessage(time, message);
                    break;
                }
                  
            }

            if (!exist)
            {
                this.users.Add(new User(nick, this.loginNick, this));
                this.users[this.users.Count - 1].addMessage(time, message);
            }

        }

        public void openMessageWindow(string nick)
        {
            bool exist = false;
            for (int i = 0; i < this.users.Count; i++)
            {
                if (this.users[i].nick == nick)
                {
                    exist = true;
                    this.users[i].show();
                    break;
                }

            }

            if (!exist)
            {
                this.users.Add(new User(nick, this.loginNick, this));
            }
        }

        public void removeUser(string nick)
        {
            for (int i = 0; i < this.users.Count; i++)
            {
                if (this.users[i].nick == nick)
                {
                    this.users[i].hide();
                    break;
                }

            }
        }

        private void handleLogout(string data)
        {
            int status = Int32.Parse(data.Split(';')[1]);

            if (status == 1)
            {
               
                this.work = false;
                this.logoutCall();
            }

        }

        public void logoutCall()
        {
            this.sendData(TYPE_LOGOUT, "1");
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
                case TYPE_LOGOUT:
                    Console.WriteLine("LOGOUT");
                    this.handleLogout(data);
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
                this.login.setThreadedErrorLabel("Connected");
                this.connecting = true;

                this.loginInUser(this.loginNick, this.password);
                //this.login = new Login(this);
                //Application.Run(this.login);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                Console.WriteLine("Check \"Server Info\" and try again!");
                this.login.setThreadedErrorLabel("Check \"Server Info\" and try again!");
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

                this.login.setThreadedErrorLabel("Wait! Connecting...");
                Console.WriteLine("Wait! Connecting...");

                /* connect to the server */
                //jeśli połączy się to odpali ConnectCallback
                socketFd.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), socketFd);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
                Console.WriteLine("Check \"Server Info\" and try again!");
                this.login.setThreadedErrorLabel("Check \"Server Info\" and try again!");
            }
        }

        public void connection(string nick, string password, string address, string port)
        {
            this.loginNick = nick;
            this.password = password;
            this.address = address;
            this.port = port;
            Dns.BeginGetHostEntry(this.address, new AsyncCallback(GetHostEntryCallback), null);

        }

        public void closeSerwer()
        {
            if (this.connecting)
            {
                this.socketFd.Shutdown(SocketShutdown.Both);
                this.socketFd.Close();
            }

            this.work = false;
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
