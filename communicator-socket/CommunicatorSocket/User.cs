using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicatorSocket
{
    public class User
    {
        public string nick;
        public ChatWindow chat;
        private Serwer serwer;
        private string loginNick;
        private String allMessages;
        private bool showChat;
        private CancellationTokenSource tokenSource;
        public User(string nick, string loginNick, Serwer serwer)
        {
            this.nick = nick;
            this.serwer = serwer;
            this.loginNick = loginNick;
            this.tokenSource = new CancellationTokenSource();
            var token = this.tokenSource.Token;

            this.chat = new ChatWindow(this.nick, this.loginNick, this.serwer, this.allMessages, this, tokenSource);
            this.showChat = true;

            var t = Task.Run(() =>
            {
                Application.Run(this.chat);
            }, token);
        }

        public void addMessage(string time, string text)
        {
            this.chat.addMessage(time, text);
        }

        public void hide()
        {
            if (this.showChat)
            {
                this.showChat = false;
                this.chat.Dispose();
            }

        }

        public void show()
        {
            if (!this.showChat)
            {
                var token = tokenSource.Token;
                this.chat = new ChatWindow(this.nick, this.loginNick, this.serwer, this.allMessages, this, tokenSource);
                var t = Task.Run(() =>
                {
                    Application.Run(this.chat);
                }, token);
                t.Wait();
            }
        }

        public void saveMessages(string allMessages)
        {
            this.allMessages = allMessages;
        }
    }
}
