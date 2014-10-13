using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace WebSocketApplication
{
    class MyWebSocketTesting
    {

        public MyWebSocketTesting()
        {
        }


        public void tcp()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("192.168.0.25"), 8081);

            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:8281.{0}Waiting for a connection..." + Environment.NewLine);

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("A client connected.");

            NetworkStream stream = client.GetStream();

            //enter to an infinite cycle to be able to handle every change in stream
            while (true)
            {
                
                while (!stream.DataAvailable) ;

                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);

                //translate bytes of request to string
                String request = Encoding.UTF8.GetString(bytes);
                Console.WriteLine(request);
                if (new Regex("^GET").IsMatch(request))
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

                    stream.Write(response, 0, response.Length);
                }
                else
                {
                    //some bug in this part
                    Console.WriteLine(request);
                    Byte[] response = Encoding.UTF8.GetBytes("Data Received");
                    response[0] = 0x81; // denotes this is the final message and it is in text
                    response[1] = Convert.ToByte(response.Length-2); // payload size = message - header size
                    Console.WriteLine(request);
                    stream.Write(response, 0, response.Length);

                }
            }
        }


    }
}
