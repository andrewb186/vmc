using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace HTTPListenerTest
{
    public partial class Form1 : Form
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpServer httpServer;

        public Form1()
        {   
            InitializeComponent();
            httpServer = new HttpServer();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            httpServer.start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            httpServer.stop();
        }

        private void btnReply_Click(object sender, EventArgs e)
        {
            //httpServer.sendResponse();
        }

    }
}
