using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PiServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label3.Text = "Сервер остановлен";
        }
        Thread t = new Thread(Program.Listen);
        private void btnStart_Click(object sender, EventArgs e)
        {
            t = new Thread(Program.Listen);
            t.Start();
            label3.Text = "Сервер запущен";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            t.Abort();
            foreach (TcpClient c in Program.lClient)
            {
                c.Close();
            }
            Program.lClient.Clear();
            label3.Text = "Сервер остановлен";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                int n;
                if (!int.TryParse(textBox1.Text, out n))
                {
                    MessageBox.Show("Ошибка ввода");
                    return;
                }
                lock (Program.lClient)
                {
                    List<int> lRemove = new List<int>();
                    for (int i = 0; i < Program.lClient.Count; i++)
                    {
                        if (!Program.lClient[i].Connected)
                            lRemove.Insert(0,i);
                    }
                    foreach (int i in lRemove)
                    {
                        Program.lClient.RemoveAt(i);
                    }
                    for (int i = 0; i < Program.lClient.Count; i++)
                    {
                        NetworkStream ns = Program.lClient[i].GetStream();
                        BinaryWriter bw = new BinaryWriter(ns);
                        bw.Write(Program.lClient.Count);
                        bw.Write(i);
                        bw.Write(n);
                    }
                    double s = 0;
                    for (int i = 0; i < Program.lClient.Count; i++)
                    {
                        NetworkStream ns = Program.lClient[i].GetStream();
                        BinaryReader br = new BinaryReader(ns);
                        double x = (double)br.ReadDouble();
                        s += x;
                    }
                    textBox2.Text = string.Format("{0} (число клиентов: {1})", s * 4,Program.lClient.Count);
                }
            }
            catch
            {
                MessageBox.Show("Один из клиентов недоступен. Попробуйте еще раз");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
            foreach (TcpClient c in Program.lClient)
            {
                c.Close();
            }
        }
    }
}
