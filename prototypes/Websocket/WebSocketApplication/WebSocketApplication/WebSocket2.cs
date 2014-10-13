using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WebSocketApplication
{
    class WebSocket2
    {
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private Thread threadAcceptTcpClient;

        public WebSocket2()
        {
            tcpListener = new TcpListener(IPAddress.Parse("192.168.0.25"), 8081);
        }

        public void start()
        {
            this.tcpListener.Start();
            threadAcceptTcpClient = new Thread(o =>
            {
                tcpClient = this.tcpListener.AcceptTcpClient();
                Console.WriteLine("Client Connected");
                Console.WriteLine("Perform Handshake");
                
                NetworkStream stream = tcpClient.GetStream();
                byte [] buffer = new byte[tcpClient.Available];
                stream.Read(buffer, 0, buffer.Length);

                string request = Encoding.UTF8.GetString(buffer);

                byte[] response = getHeader(request);

                stream.Write(response, 0, response.Length);

                Console.WriteLine("Handshake Accepted");
            });
            threadAcceptTcpClient.Start();
        }

             
        public void stop()
        {
            this.tcpClient.Close();
            this.tcpClient = null;

            this.tcpListener.Stop();            
        }


        public string read()
        {
            return "";
        }

       
        public void write(string msg)
        {
        }


        public void write(byte[] msg)
        {
        }


        private byte[] getHeader(string request)
        {
            Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                        + "Connection: Upgrade" + Environment.NewLine
                        + "Upgrade: websocket" + Environment.NewLine
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                            SHA1.Create().ComputeHash(
                                Encoding.UTF8.GetBytes(
                                    new Regex("Sec-WebSocket-Key: (.*)").Match(request).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + Environment.NewLine
                        + Environment.NewLine);
            return response;
        }

    }
}
