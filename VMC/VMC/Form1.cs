using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using VMCHttpLibrary;

namespace VMC
{
    public partial class Form1 : Form
    {

        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Form1()
        {
            InitializeComponent();
            log.Info("Initializing");

            HttpServer c = new HttpServer("localhost", 58081);
        }

    }

}
