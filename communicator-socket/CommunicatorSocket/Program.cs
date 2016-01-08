using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace CommunicatorSocket
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //vps.tomys.pl:1234
            //192.168.0.28:1234
            Serwer serwer = new Serwer();

            serwer.showLoginWindow();
            while (serwer.work)
            {
            }
        }
    }
}