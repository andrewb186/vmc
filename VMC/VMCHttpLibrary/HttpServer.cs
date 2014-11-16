using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net;
//using VMCHttpLibrary.Model;
using VMCHttpLibrary.Controller;
using System.IO;
using VMCHttpLibrary.Objects;

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

                HttpServerRequestObject requestModel = ProcessRequest(requestContext);
                HttpServerResponseObject responseObject = LoadFile(requestModel);
                SendResponse(responseObject, responseContext);
            }
        }


        private HttpServerRequestObject ProcessRequest(HttpListenerRequest request)
        {
            HttpServerRequestObject _request = new HttpServerRequestObject(request);
            return _request;
        }

        private HttpServerResponseObject LoadFile(HttpServerRequestObject httpServerRequestModel)
        {
            HttpServerLoadFile file = new HttpServerLoadFile(httpServerRequestModel);
            file.loadFile();
            return file.ResponseObject;
        }


        private void SendResponse(HttpServerResponseObject responseObject, HttpListenerResponse responseContext)
        {
            responseContext.ContentLength64 = responseObject.ContentLength;
            responseContext.ContentEncoding = responseObject.EncodingType;
            responseContext.ContentType = responseObject.ContentType;
            responseContext.StatusCode = (int)responseObject.StatusCode;

            StreamWriter writer = new StreamWriter(responseContext.OutputStream);
            writer.Write(responseObject.Data);
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
