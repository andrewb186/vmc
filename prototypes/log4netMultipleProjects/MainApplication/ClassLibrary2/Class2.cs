using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using log4net;

namespace ClassLibrary2
{
    public class Class2
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Class2()
        {
            log.Error("Class 2");
        }
    }
}
