using System;
using System.Windows.Forms;
using System.IO;
using Shared.Serialization;
using System.Text;

namespace Client.App
{
    public partial class Messenger : Form
    {
        public Messenger()
        {
            InitializeComponent();
            GlobalData.Communicator.NotificationReceived += NotificationReceived;
            GlobalData.Communicator.MessageReceived += MessageReceived;
            GlobalData.Communicator.CommunicationReponseReceived += CommunicationReponseReceived;
        }

        private void CommunicationReponseReceived(ResponseCode code)
        {
            if (code == ResponseCode.Fail)
                MessageBox.Show("Receiver is offline", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MessageReceived(CommunicationResponse communication)
        {
            Invoke(new Action(() =>
            {
                string message = null;
                if (communication.CommunicationType == CommunicationType.OneToOne)
                {
                    message = $"{communication.UserFrom} to {communication.UserTo}: ";
                }
                else message = $"{communication.UserFrom} to ALL: ";
                GlobalData.CryptoData.GenerateKey(GlobalData.Users[communication.UserFrom].pubKey);
                byte[] IV = GlobalData.Users[communication.UserFrom].IV;
                byte[] strmessage=communication.Message;
                string Message = GlobalData.CryptoData.Decrypt(strmessage, IV);
                _messages.AppendText($"{message}{Message}" + Environment.NewLine);
            }));
        }
        private void NotificationReceived(Notification notification)
        {
            Invoke(new Action(() =>
            {
                if (notification.NotificationType == NotificationCode.Online)
                {
                    _users.Items.Add(notification.Login);
                    GlobalData.Users.Add(notification.Login, (notification.PublicKey, notification.IV));
                }
                else
                {
                    _users.Items.Remove(notification.Login);
                    GlobalData.Users.Remove(notification.Login);
                }
            }));
        }

        private void SendMessage(object sender, EventArgs e)
        {
            if (_isToAll.Checked && _users.Items.Count != 0)
            {
                foreach (var item in _users.Items)
                {
                    GlobalData.Communicator.SendMessage(
                    _messageContainer.Text,
                    item.ToString(),
                    true);
                }
                
            }
            else if (_users.SelectedIndex >= 0)
            {
                GlobalData.Communicator.SendMessage(
                    _messageContainer.Text,
                    _users.SelectedItem.ToString(),
                    false);
            }
            else
            {
                MessageBox.Show("Получатель не выбран или список подключенных пользователей пуст", "Отправить", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string message = null;
            if (_isToAll.Checked)
            {
                message = $"{GlobalData.Login} to ALL: ";
            }
            else message = $"{GlobalData.Login} to {_users.SelectedItem}: ";
            _messages.AppendText($"{message}{_messageContainer.Text}" + Environment.NewLine);
        }

        private void CloseApp(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SelectedUserIndex(object sender, EventArgs e)
        {
            _send.Enabled = _users.SelectedIndex >= 0;
        }
    }
}
