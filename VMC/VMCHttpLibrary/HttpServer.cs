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
            try
            {
                if (isIpAddressValid(IpAddress) && isPortValid(port))
                {
                    string _localhost = @"http://localhost:" + Convert.ToString(port);
                    string _ipAddress = IpAddress + ":" + Convert.ToString(port);

                    initializeHttpListener(_localhost, _ipAddress);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
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

        private void initializeHttpListener(string localhost, string ipAddress)
        {
        }

        private bool isIpAddressValid(string IpAddress)
        {
            bool isValid = true;

            if (IpAddress == null)
            {
                isValid = false;
                throw new Exception("Ip Address is Null ");
            }
            else
            {
                IpAddress = IpAddress.Trim();

                if (IpAddress.Equals(""))
                {
                    isValid = false;
                    throw new Exception("Ip Address is Empty");
                }
            }

            return isValid;
        }


        private bool isPortValid(int port)
        {
            bool isValid = true;

            if ((port >= 10000) && (port <= 65534))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
                throw new Exception("Port number is out of range. 10,000 < port < 64,000");
            }

            return isValid;
        }

    }
}
