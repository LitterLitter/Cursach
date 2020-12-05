using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Shared.Serialization;

namespace Server
{
    public class Server : IDisposable
    {
        public void AddConnection(Session Session)
        {
            _clients.Add(Session);
        }
        public void RemoveConnection(Session session)
        {
            _ = _clients.Remove(session);
            session.Dispose();
        }
        /// <summary>
        /// Listening of input connections
        /// </summary>
        public void Process()
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Any, 8888);
                _tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                    var session = new Session(tcpClient, this);
                    var clientThread = new Thread(session.Process);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Dispose();
            }
        }

        public void SendAllLiveUsers(Session session)
        {
            _clients.Where(client => client != session)
                .ToList()
                .ForEach((client) => session.SendNotification(new Notification()
                {
                    NotificationType = NotificationCode.Online,
                    Type = ExchangeType.Notification,
                    Login = client.Login,
                    PublicKey=client.PubKey,
                    IV=client.IV
                }));
        }
        /// <summary>
        ///translation notifications to connected clients 
        /// </summary>
        public void BroadcastNotificationExcept(Notification notification, Session session)
        {
            _clients.Where(client => client != session)
                .ToList()
                .ForEach((client) => client.SendNotification(notification));
        }
        /// <summary>
        ///translation messages to connected clients 
        /// </summary>
        public void BroadcastMessageExcept(CommunicationResponse message, Session session)
        {
            _clients.Where(client => client != session).ToList().ForEach((client) => client.SendCommunication(message));
        }
        public bool SendTo(CommunicationResponse message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            lock (_lock)
            {
                var client = _clients.FirstOrDefault(client => client.Login.Equals(message.UserTo));
                if (client != null)
                {
                    client.SendCommunication(message);
                }
                return client != null;
            }
        }
        /// <summary>
        /// disconnection all of the clients
        /// </summary>
        public void Dispose()
        {
            _tcpListener?.Stop(); 
        }
        private object _lock = new object();
        private TcpListener _tcpListener; 
        private List<Session> _clients = new List<Session>(); 
    }
}