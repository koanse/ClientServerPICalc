using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO;

namespace PiClient
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static void Calc()
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = new TcpClient("127.0.0.1", 13000);
                stream = client.GetStream();
                BinaryReader br = new BinaryReader(stream);
                BinaryWriter bw = new BinaryWriter(stream);
                while (true)
                {
                    int step = br.ReadInt32();
                    int start = br.ReadInt32();
                    int n = br.ReadInt32();
                    double s = 0;
                    for (int i = start; i < n; i += step)
                    {
                        s += Math.Pow(-1, i) / (2 * i + 1);
                    }
                    bw.Write(s);
                }
            }
            catch (ThreadAbortException) { }
            catch
            {
                MessageBox.Show("Ошибка подключения к серверу. Клиент остановлен");
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
