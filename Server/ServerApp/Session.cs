using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Shared.Serialization;
using Shared.Extensions;
using Server.Logic;
using System.Linq;

namespace Server
{
    public class Session : IDisposable
    {
        public string Login => _userName;
        public byte[] PubKey => _PubKey;
        public byte[] IV => _IV;
        public Session(TcpClient tcpClient, Server serverObject)
        {
            _client = tcpClient;
            _server = serverObject;
            serverObject.AddConnection(this);
        }
        public void SendResponse(Response response) => SendInternal(response);
        public void SendCommunication(CommunicationResponse response) => SendInternal(response);
        public void SendNotification(Notification notification) => SendInternal(notification);
        private void SendInternal<T>(T data)
        {
            lock (_lock)
            {
                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                _stream?.Write(buffer, 0, buffer.Length);
                _stream?.Flush();
            }
        }
        public void Process()
        {
            try
            {
                _stream = _client.GetStream();

                while (true)
                {
                    try
                    {
                        var data = ReadData();
                        var stringData = Encoding.UTF8.GetString(data);
                        var jsons = stringData.SplitToJsons().ToList();
                        
                        foreach (var message in jsons)
                        {
                            var request = JsonConvert.DeserializeObject<Request>(message, new RequestConverter());
                            Response response = null;
                            if (request is EncryptedDataWrapper encryptedDataWrapper)
                            {
                                var jsonRequest = GlobalData.CryptoData.Decrypt(encryptedDataWrapper.EncryptedData);
                                request = JsonConvert.DeserializeObject<Request>(jsonRequest, new RequestConverter());
                            }
                            switch (request)
                            {
                                case Authentication authRequest:
                                    var authHandler = new AuthenticationHandler();
                                    response = authHandler.Handle(authRequest);
                                    if (response.Code == ResponseCode.Success)
                                    {
                                        _userName = authRequest.Login;
                                        _PubKey = authRequest.PublicKey;
                                        _IV = authRequest.IV;
                                        _server.BroadcastNotificationExcept(new Notification()
                                        {
                                            Type = ExchangeType.Notification,
                                            NotificationType = NotificationCode.Online,
                                            Login = authRequest.Login,
                                            PublicKey=authRequest.PublicKey,
                                            IV=authRequest.IV
                                        }, this);
                                    }
                                    SendResponse(response);
                                    _server.SendAllLiveUsers(this);
                                    break;
                                case Registration regRequest:
                                    var regHandler = new RegistrationHandler();
                                    response = regHandler.Handle(regRequest);
                                    SendResponse(response);
                                    break;
                                case Communication comRequest:
                                    if (comRequest.CommunicationType == CommunicationType.OneToMany)
                                    {
                                        _server.BroadcastMessageExcept(
                                            new CommunicationResponse()
                                            {
                                                Type = ExchangeType.Communication,
                                                CommunicationType = CommunicationType.OneToMany,
                                                Message = comRequest.Message,
                                                UserFrom = Login,
                                                UserTo = comRequest.UserTo
                                            }, this);
                                    }
                                    else
                                    {
                                        response = _server.SendTo(
                                            new CommunicationResponse()
                                            {
                                                Type = ExchangeType.Communication,
                                                CommunicationType = CommunicationType.OneToOne,
                                                Message = comRequest.Message,
                                                UserFrom = Login,
                                                UserTo = comRequest.UserTo
                                            }) ?
                                            new Response() { Code = ResponseCode.Success, Type = ExchangeType.Communication } :
                                            new Response() { Code = ResponseCode.Fail, Type = ExchangeType.Communication };
                                    }
                                    SendResponse(new CommunicationResponse()
                                    {
                                        UserFrom = _userName,
                                        UserTo = comRequest.UserTo,
                                        Code = ResponseCode.Success,
                                        CommunicationType = comRequest.CommunicationType,
                                        Type = ExchangeType.Communication,
                                    });
                                    break;
                                case HandshakeRequest handRequest:
                                    GlobalData.CryptoData.GenerateKey(handRequest.ClientKey,handRequest.iv);
                                    SendResponse(new HandshakeResponse()
                                    {
                                        ServerKey = GlobalData.CryptoData.serverPublicKey,
                                        Type = ExchangeType.Handshake,
                                        Code = ResponseCode.Success
                                    });
                                    break;
                                default:
                                    throw new InvalidOperationException("Received unknown message");
                            }

                        }
                    }
                    catch (Exception exception)
                    {
                        if (_userName != null)
                        {
                            _server.BroadcastNotificationExcept(new Notification()
                            {
                                Type = ExchangeType.Notification,
                                NotificationType = NotificationCode.Offline,
                                Login = _userName
                            }, this);
                            Console.WriteLine("{0}: покинул чат", _userName);
                        }
                        break;
                    }
                }
            }
            finally
            {
                _server.RemoveConnection(this);
            }
        }
        /// <summary>
        /// Reading of input data
        /// </summary>
        /// <returns></returns>
        private byte[] ReadData()
        {
            byte[] data = new byte[1024];
            int offset = 0;
            do
            {
                var readBytes = _stream.Read(data, offset, data.Length);
                offset += readBytes;
            }
            while (_stream.DataAvailable);
            var receivedData = new byte[offset];
            Array.Copy(data, receivedData, offset);
            return receivedData;
        }
        public void Dispose()
        {
            _stream?.Close();
            _client?.Close();
        }
        private NetworkStream _stream;
        private string _userName;
        private byte[] _PubKey;
        private byte[] _IV;
        private TcpClient _client;
        private Server _server;
        private object _lock = new object();
    }
}