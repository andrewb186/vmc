using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net;

namespace VMCHttpLibrary
{
    public class HttpServer
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpListener _httpListener;

        public HttpServer(string IpAddress, int port)
        {
            string _localhost = @"http://localhost:" + Convert.ToString(port);
            string _ipAddress = IpAddress + ":" + Convert.ToString(port);
        }


        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Run()
        {
        }


        private void ProcessRequest()
        {
        }

        private void ProcessResponse()
        {
        }
    }
}
