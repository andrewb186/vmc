using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Core;
using ClassLibrary1;
using ClassLibrary2;

namespace MainApplication
{
    public partial class Form1 : Form
    {

        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log.Error("Form Loaded");

            Class1 c = new Class1();
            Class2 c2 = new Class2();
        }
    }
}
