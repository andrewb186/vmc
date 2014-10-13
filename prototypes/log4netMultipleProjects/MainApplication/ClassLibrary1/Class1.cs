using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Core;

namespace ClassLibrary1
{
    public class Class1
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Class1()
        {
            log.Error("Class 1 - Constructor");
        }

    }
}
