using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace WebSocketApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private WebSocket2 web2 = new WebSocket2();

        private void btnStart_Click(object sender, EventArgs e)
        {
            
           // web2.start();

            // MyWebSocketTesting socket = new MyWebSocketTesting();

            //Thread t = new Thread(socket.tcp);
            //t.Start();

            WebSocket web = new WebSocket(IPAddress.Parse("192.168.0.25"), 8081);
            web.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
           // web2.stop();
        }


    }
}
