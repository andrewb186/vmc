using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;


namespace WebSocketApplication2
{
    class MySocket
    {

        private WebSocketServer webSocketServer;


        public MySocket()
        {
            webSocketServer = new WebSocketServer();
            webSocketServer.Setup("192.168.0.25", 8081, null, null, null, null, null);
            webSocketServer.NewDataReceived += new SessionHandler<WebSocketSession, byte[]>(webSocketServer_NewDataReceived);
            webSocketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(webSocketServer_NewMessageReceived);
            webSocketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(webSocketServer_NewSessionConnected);
            webSocketServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(webSocketServer_SessionClosed);
        }

        void webSocketServer_SessionClosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("====== {0} ======", value);
        }

        void webSocketServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine(" =====  New Session =====");
        }

        void webSocketServer_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine(" ====== {0} =======", value);

            foreach (WebSocketSession _session in webSocketServer.GetAllSessions())
            {
                _session.Send(value);
            }
        }

        void webSocketServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            throw new NotImplementedException();
        }


        public bool start()
        {
            return webSocketServer.Start();
        }

        
        
    }
}
