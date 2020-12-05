using System;
using System.Windows.Forms;

using Client.App.Integration;
using Shared.Serialization;

namespace Client.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _password.UseSystemPasswordChar = true;

            GlobalData.Communicator.Connect();
            GlobalData.Communicator.Handshake(new HandshakeRequest()
            {
                Type = ExchangeType.Handshake,
                iv = GlobalData.CryptoData.IV,
                ClientKey = GlobalData.CryptoData.PublicKey
            });
            GlobalData.Communicator.AuthenticationReponseReceived += AuthResponse;
        }

        private void AuthResponse(Shared.Serialization.ResponseCode code)
        {
            if (code == Shared.Serialization.ResponseCode.Success)
            {
                Invoke(new Action(() =>
                {
                    Hide();
                    var messanger = new Messenger();
                    messanger.Show();
                    messanger.Focus();
                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    GlobalData.Login = string.Empty;
                    MessageBox.Show("Пользователь с данным паролем или логином не существует");
                }));
            }
        }
        private void Authenticate(object sender, EventArgs e)
        {
            GlobalData.Communicator.Authenticate(_login.Text, _password.Text);
            GlobalData.Login = _login.Text;
        }
        private void Registrate(object sender, EventArgs e)
        {
            new Registration().ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
