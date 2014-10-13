using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WebSocketApplication
{
    class WebSocket
    {
        
        private TcpListener tcpListener;
        private List<Client> clients;
 
        /// <summary>
        /// Constructor for a new server using an IPAddress and Port
        /// </summary>
        /// <param name="localaddr">The Local IP Address for the server.</param>
        /// <param name="port">The port for the server.</param>
        public WebSocket(IPAddress localaddr, int port)
            : this()
        {
            tcpListener = new TcpListener(localaddr, port);
        }
 
        /// <summary>
        /// Constructor for a new server using an end point
        /// </summary>
        /// <param name="localEP">The local end point for the server.</param>
        public WebSocket(IPEndPoint localEP)
            : this()
        {
            tcpListener = new TcpListener(localEP);
        }
 
        /// <summary>
        /// Private constructor for the common constructor operations.
        /// </summary>
        private WebSocket()
        {
            this.Encoding = Encoding.Default;
            this.clients = new List<Client>();
        }
 
        /// <summary>
        /// The encoding to use when sending / receiving strings.
        /// </summary>
        public Encoding Encoding { get; set; }
 
        /// <summary>
        /// An enumerable collection of all the currently connected tcp clients
        /// </summary>
        public IEnumerable<TcpClient> TcpClients
        {
            get
            {
                foreach (Client client in this.clients)
                {
                    yield return client.TcpClient;
                }
            }
        }
 
        /// <summary>
        /// Starts the TCP Server listening for new clients.
        /// </summary>
        public void Start()
        {
            this.tcpListener.Start();
            this.tcpListener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
        }
 
        /// <summary>
        /// Stops the TCP Server listening for new clients and disconnects
        /// any currently connected clients.
        /// </summary>
        public void Stop()
        {
            this.tcpListener.Stop();
            lock (this.clients)
            {
                foreach (Client client in this.clients)
                {
                    client.TcpClient.Client.Disconnect(false);
                }
                this.clients.Clear();
            }
        }
 
        /// <summary>
        /// Writes a string to a given TCP Client
        /// </summary>
        /// <param name="tcpClient">The client to write to</param>
        /// <param name="data">The string to send.</param>
        public void Write(TcpClient tcpClient, string data)
        {
            byte[] bytes = this.Encoding.GetBytes(data);
            Write(tcpClient, bytes);
        }
 
        /// <summary>
        /// Writes a string to all clients connected.
        /// </summary>
        /// <param name="data">The string to send.</param>
        public void Write(string data)
        {
            foreach (Client client in this.clients)
            {
                Write(client.TcpClient, data);
            }
        }
 
        /// <summary>
        /// Writes a byte array to all clients connected.
        /// </summary>
        /// <param name="bytes">The bytes to send.</param>
        public void Write(byte[] bytes)
        {
            foreach (Client client in this.clients)
            {
                Write(client.TcpClient, bytes);
            }
        }
 
        /// <summary>
        /// Writes a byte array to a given TCP Client
        /// </summary>
        /// <param name="tcpClient">The client to write to</param>
        /// <param name="bytes">The bytes to send</param>
        public void Write(TcpClient tcpClient, byte[] bytes)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, tcpClient);
        }
 
        /// <summary>
        /// Callback for the write opertaion.
        /// </summary>
        /// <param name="result">The async result object</param>
        private void WriteCallback(IAsyncResult result)
        {
            TcpClient tcpClient = result.AsyncState as TcpClient;
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.EndWrite(result);
        }
 
        /// <summary>
        /// Callback for the accept tcp client opertaion.
        /// </summary>
        /// <param name="result">The async result object</param>
        private void AcceptTcpClientCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
            Client client = new Client(tcpClient, buffer);
            lock (this.clients)
            {
                this.clients.Add(client);
            }
            NetworkStream networkStream = client.NetworkStream;
            networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
            tcpListener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
        }
 
        /// <summary>
        /// Callback for the read opertaion.
        /// </summary>
        /// <param name="result">The async result object</param>
        private void ReadCallback(IAsyncResult result)
        {
            Client client = result.AsyncState as Client;
            if (client == null) return;
            NetworkStream networkStream = client.NetworkStream;
            int read = networkStream.EndRead(result);
            if (read == 0)
            {
                lock (this.clients)
                {
                    this.clients.Remove(client);
                    return;
                }
            }
            string data = this.Encoding.GetString(client.Buffer, 0, read);
            Console.WriteLine(data);
            Write(getHeader(data));
            //Do something with the data object here.
            networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
        }

        private byte[] getHeader(string request)
        {
            Byte[] response = null;

            if (new Regex("^GET").IsMatch(request))
            {
                response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
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
            }
            else
            {
                //some bug in this part
                Console.WriteLine(request);
                response = Encoding.UTF8.GetBytes("Data Received");
                response[0] = 0x81; // denotes this is the final message and it is in text
                response[1] = Convert.ToByte(response.Length - 2); // payload size = message - header size
                Console.WriteLine(request);
            }
            return response;
        }

    }


    internal class Client
    {
        /// <summary>
        /// Constructor for a new Client
        /// </summary>
        /// <param name="tcpClient">The TCP client</param>
        /// <param name="buffer">The byte array buffer</param>
        public Client(TcpClient tcpClient, byte[] buffer)
        {
            if (tcpClient == null) throw new ArgumentNullException("tcpClient");
            if (buffer == null) throw new ArgumentNullException("buffer");
            this.TcpClient = tcpClient;
            this.Buffer = buffer;
        }
 
        /// <summary>
        /// Gets the TCP Client
        /// </summary>
        public TcpClient TcpClient { get; private set; }
 
        /// <summary>
        /// Gets the Buffer.
        /// </summary>
        public byte[] Buffer { get; private set; }
 
        /// <summary>
        /// Gets the network stream
        /// </summary>
        public NetworkStream NetworkStream { get { return TcpClient.GetStream(); } }
    }
      


    }

