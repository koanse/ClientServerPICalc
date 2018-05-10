using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PiClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "0";
        }
        List<Thread> lThread = new List<Thread>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lThread.Add(new Thread(Program.Calc));
            label1.Text = lThread.Count.ToString();
            lThread.Last().Start();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lThread.Count<1)
                return;
            lThread[0].Abort();
            lThread.RemoveAt(0);
            label1.Text = lThread.Count.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Thread t in lThread)
            {
                t.Abort();
            }
        }
    }
}
