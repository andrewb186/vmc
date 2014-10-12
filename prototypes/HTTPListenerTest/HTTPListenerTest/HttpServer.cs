using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using log4net;
using System.Threading;


namespace HTTPListenerTest
{
    class HttpServer
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HttpListener httpListener;

        private Thread thread;
 
        public HttpServer() :this(@"http://192.168.0.25:8081")
        {
            
        }

        public HttpServer(string prefix)
        {
            try
            {
                prefix = prefix.Trim();

                if ((prefix == null) || prefix.Equals(""))
                {
                    log.Error("Invalid Prefix");
                }
                else
                {
                    log.Info("Valid Prefix");
                    Uri uri = new Uri(prefix);

                    log.Info("Initialising Http Listener");
                    httpListener = new HttpListener();
                    httpListener.Prefixes.Add(uri.AbsoluteUri);
                }
            }
            catch (ArgumentNullException ane)
            {
                log.Error(ane.Message, ane);
            }
            catch (ArgumentException ae)
            {
                log.Error(ae.Message, ae);
            }
            catch(PlatformNotSupportedException pnse)
            {
                log.Error(pnse.Message, pnse);
            }
            catch (ObjectDisposedException ode)
            {
                log.Error(ode.Message, ode);
            }
            catch (HttpListenerException hle)
            {
                log.Error(hle.Message, hle);
            }
        }


        public void start()
        {
            log.Info("Starting Server");

            if (!httpListener.IsListening)
            {
                httpListener.Start();
                thread = new Thread(() => sendResponse("hello"));
                thread.Start();
            }
        }


        public void stop()
        {
            log.Info("Stopping Server");

            if (httpListener.IsListening)
            {
                httpListener.Stop();
                httpListener.Close();                
            }
        }

        public void sendResponse(string text)
        {
            try
            {
                while (httpListener.IsListening)
                {
                    HttpListenerContext context = httpListener.GetContext();
                    
                    context.Response.ContentLength64 = Convert.ToInt64(Encoding.UTF8.GetByteCount(getResponseMessage()));
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.ContentType = "text/plain";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;

                    log.InfoFormat("Sending Response to {0}", context.Request.RemoteEndPoint);
                    log.InfoFormat("Response content type {0}, Status code {1}", context.Response.ContentType, context.Response.StatusCode);
                    log.Info("Creating Stream");

                    Stream stream = context.Response.OutputStream;
                    StreamWriter writer = new StreamWriter(stream);

                    log.Info("Writing to stream");

                    writer.Write(getResponseMessage());
                    writer.Flush();
                    writer.Close();

                    log.Info("Closing Stream");
                }
            }
            catch (ObjectDisposedException ode)
            {
                log.Error(ode.Message, ode);
            }
            catch (ArgumentNullException ane)
            {
                log.Error(ane.Message, ane);
            }
            catch (ArgumentException ae)
            {
                log.Error(ae.Message, ae);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
            

        }


        private string getResponseMessage()
        {
            return "Hello World - Message from Http Listener - " + randomString();
        }


        private string randomString()
        {
            Random r = new Random();
            int i = r.Next(100, 99999999);
            return Convert.ToString(i).PadLeft(8, '0');
        }

    }
}
