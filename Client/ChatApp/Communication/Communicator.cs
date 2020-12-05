using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

using Shared.Serialization;
using Shared.Extensions;
using System.Windows.Forms;

namespace Client.App.Integration
{
    public class Communicator
    {
        public event Action<ResponseCode> AuthenticationReponseReceived;
        public event Action<ResponseCode> RegistrationReponseReceived;
        public event Action<ResponseCode> CommunicationReponseReceived;
        public event Action<CommunicationResponse> MessageReceived;
        public event Action<Notification> NotificationReceived;
        public void Connect()
        {
            _client = new TcpClient();
            try
            {
                _client.Connect(HOST, PORT);
                _stream = _client.GetStream();

                var thread = new Thread(ProcessData);
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                Environment.Exit(0);
                Dispose();
            }
        }
        public void Authenticate(string login, string password)
        {
            var authRequest = new Authentication()
            {
                Login = login,
                Password = password,
                Type = ExchangeType.Authentication,
                PublicKey=GlobalData.CryptoData.PublicKey,
                IV=GlobalData.CryptoData.IV
            };
            SendWithEncryption(authRequest);
        }
        public void Handshake(HandshakeRequest handshake) => Send(handshake);
        private void Send<T>(T data)
        {
            lock (_lock)
            {
                var obj = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(obj);
                _stream?.Write(buffer, 0, buffer.Length);
            }
        }
        private void SendWithEncryption<T>(T data)
        {
            lock (_lock)
            {
                var encryptedData = GlobalData.CryptoData.Encrypt(JsonConvert.SerializeObject(data));
                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new EncryptedDataWrapper()
                {
                    EncryptedData = encryptedData,
                    Type = ExchangeType.EncryptedData
                }));
                
                _stream?.Write(buffer, 0, buffer.Length);
            }
        }
        public void Registrate(string login, string password)
        {
            var regRequest = new Shared.Serialization.Registration()
            {
                Login = login,
                Password = password,
                Type = ExchangeType.Registration
            };
            SendWithEncryption(regRequest);
        }
        public void SendMessage(string message, string userTo, bool toAll)
        {
            GlobalData.CryptoData.GenerateKey(GlobalData.Users[userTo].pubKey);
            var comRequest = new Communication()
            {
                CommunicationType = toAll ? CommunicationType.OneToMany : CommunicationType.OneToOne,
                Type = ExchangeType.Communication,
                Message = GlobalData.CryptoData.Encrypt(message),
                UserTo = userTo
            };
            Send(comRequest);
        }
        public void Dispose()
        {
            _stream?.Close();
            _stream?.Dispose();
            _client?.Close();
            _client?.Dispose();
        }
        private void ProcessData()
        {
            while (true)
            {
                try
                {
                    var message = Encoding.UTF8.GetString(ReadData());
                    foreach (var json in message.SplitToJsons())
                    {
                        var data = JsonConvert.DeserializeObject<Response>(json, new ResponseConverter());
                        switch (data)
                        {
                            case AuthResponse authResponse:
                                AuthenticationReponseReceived?.Invoke(authResponse.Code);
                                break;
                            case RegResponse regResponse:
                                RegistrationReponseReceived?.Invoke(regResponse.Code);
                                break;
                            case Notification notification:
                                NotificationReceived?.Invoke(notification);
                                break;
                            case CommunicationResponse communication when communication.Message is null:
                                CommunicationReponseReceived?.Invoke(communication.Code);
                                break;
                            case CommunicationResponse communication:
                                MessageReceived?.Invoke(communication);
                                break;
                            case HandshakeResponse handResponse:
                                if (handResponse.Code == ResponseCode.Success)
                                {
                                    GlobalData.CryptoData.GenerateKey(handResponse.ServerKey);
                                }
                                else
                                {
                                    throw new InvalidOperationException("Bad handshake data");
                                }

                                break;
                            default:
                                throw new InvalidOperationException("Received unknown response");
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw;
                }
            }
        }
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
        private object _lock = new object();
        private TcpClient _client;
        private const string HOST = "127.0.0.1";
        private const int PORT = 8888;
        private NetworkStream _stream;
        private byte[] _data = new byte[1024];
    }
}
