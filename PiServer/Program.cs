using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace PiServer
{
    static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static List<TcpClient> lClient = new List<TcpClient>();
        public static TcpListener server = null;
        public static void Listen()
        {
            try
            {
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 13000);
                server.Start();
                while (true)
                {
                    while(!server.Pending())
                        Thread.Sleep(100);
                    TcpClient client = server.AcceptTcpClient();
                    lock (lClient)
                    {
                        lClient.Add(client);
                    }
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
