using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net;
using VMCHttpLibrary.Model;
using VMCHttpLibrary.Controller;
using System.IO;

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
            log.Info("Starting Server");

            if (!_httpListener.IsListening)
            {
                _httpListener.Start();
                log.Info("Http server started");
            }
        }


        public void Stop()
        {
            log.Info("Stopping Server");

            if (_httpListener.IsListening)
            {
                _httpListener.Stop();
                _httpListener.Close();
            }
        }


        public void Run()
        {
            while (_httpListener.IsListening)
            {
                HttpListenerContext httpContext = _httpListener.GetContext();
                HttpListenerRequest requestContext = httpContext.Request;
                HttpListenerResponse responseContext = httpContext.Response;

                HttpServerRequestModel requestModel = ProcessRequest(requestContext);
                char [] buffer = LoadFile(requestModel);
                SendResponse(buffer, responseContext);
            }
        }


        private HttpServerRequestModel ProcessRequest(HttpListenerRequest request)
        {
            HttpServerRequestModel _request = new HttpServerRequestModel(request);
            return _request;
        }

        private char [] LoadFile(HttpServerRequestModel httpServerRequestModel)
        {
            HttpServerLoadFile file = new HttpServerLoadFile(httpServerRequestModel);
            return file.loadFile();
        }


        private void SendResponse(char [] buffer, HttpListenerResponse responseContext)
        {
            responseContext.ContentLength64 = Convert.ToInt64(Encoding.UTF8.GetByteCount(buffer));
            responseContext.ContentEncoding = Encoding.UTF8;
            responseContext.ContentType = "text/html";
            responseContext.StatusCode = (int) HttpStatusCode.OK;

            StreamWriter writer = new StreamWriter(responseContext.OutputStream);
            writer.Write(buffer);
            writer.Flush();
            writer.Close();
        }

        private void initializeHttpListener(string localhost, string ipAddress)
        {
            try
            {
                log.Info("Initialising Http listener");

                Uri localhostUri = new Uri(localhost);
                Uri ipUri = new Uri(ipAddress);

                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add(localhostUri.AbsoluteUri);
                _httpListener.Prefixes.Add(ipUri.AbsoluteUri);

                log.Info("Http listener Initialised");
            }
            catch (ArgumentNullException ex)
            {
                log.Error(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                log.Error(ex.Message, ex);
            }
            catch (PlatformNotSupportedException ex)
            {
                log.Error(ex.Message, ex);
            }
            catch (ObjectDisposedException ex)
            {
                log.Error(ex.Message, ex);
            }
            catch (HttpListenerException ex)
            {
                log.Error(ex.Message, ex);
            }
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

            if ((port >= 8000) && (port <= 65534))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
                throw new Exception("Port number is out of range. 8000 < port < 64000");
            }

            return isValid;
        }

    }
}
