using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using log4net;

namespace VMCplayer
{
    static class Program
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.Info("*******************************");
            log.Info("*\t" + Application.ProductName + "\t" + Application.ProductVersion + "\t*");
            log.Info("*******************************");
            log.Info("Application entry point triggered");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
