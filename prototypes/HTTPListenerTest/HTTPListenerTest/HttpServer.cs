using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HTTPListenerTest
{
    class HttpServer
    {

        private HttpListener httpListener;

        public HttpServer() :this(@"http://192.168.0.25:8081")
        {
            
        }

        public HttpServer(string prefix)
        {
            Uri uri = new Uri(prefix);
            
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(uri.AbsoluteUri);
        }


        public void start()
        {
            httpListener.Start();
        }


        public void stop()
        {
            httpListener.Stop();
        }


        public void sendResponse()
        {
            try
            {
                HttpListenerContext context = httpListener.GetContext();
                context.Response.ContentLength64 = Convert.ToInt64(Encoding.UTF8.GetByteCount(getResponseMessage()));
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                Stream stream = context.Response.OutputStream;
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(getResponseMessage());
                writer.Flush();
                writer.Close();

            }
            catch (ObjectDisposedException ode)
            {
                //Log Exception
            }
            catch (ArgumentNullException ane)
            {
                //log
            }
            catch (ArgumentException ae)
            {
                //log
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
