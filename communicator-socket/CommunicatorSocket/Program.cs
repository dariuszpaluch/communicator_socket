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
            //Serwer serwer = new Serwer("vps.tomys.pl", "1234");
            Serwer serwer = new Serwer("192.168.0.28", "1234");

            serwer.connection();
            while (serwer.work)
            {
            }
        }
    }
}