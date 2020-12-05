using System;
using System.Windows.Forms;
using Validation.Forms;
using Shared.Serialization;
using Client.App.Properties;
using System.Text.RegularExpressions;

namespace Client.App
{
    public partial class Registration : Form
    {
        
        public Registration()
        {
            InitializeComponent();
            _login
                .ValidateControl()
                .IsTrue(TextBox=>!TextBox.Text.Contains(" "),
                "Логин не может содержать пробелов")
                .IsTrue(TextBox=> checkLogin(TextBox.Text),
                "Логин должен состоять от 5 до 24 символов," +
                "\nМожет содержать цифры, латинские буквы,"+
                "\nспециальные символы",
                ValidationType.Required);
            _password
                .ValidateControl()
                .IsTrue(TextBox => !TextBox.Text.Contains(" "),
                "Пароль не может содержать пробелов")
                .IsTrue(TextBox=>checkPassword(TextBox.Text),
                "Пароль должен состоять из 8-24 символов" +
                "\nи содержать в себе латинские буквы," +
                "\nкак минимум, одну цифру.",
                ValidationType.Required);
            _ConfirmPassword
                .ValidateControl()
                .IsTrue(txtBox => txtBox.Text.Equals(_password.Text),
                "Пароли должны совпадать.",
                ValidationType.Required);
            butReg
                .ValidateControl()
                .EnableByValidationResult();
            
            GlobalData.Communicator.RegistrationReponseReceived += RegistrationResponseReceived;
        }

        private void RegistrationResponseReceived(ResponseCode code)
        {
            Invoke(new Action(() =>
            {
                if (code == ResponseCode.Fail)
                {
                    MessageBox.Show(
                        "User with such login has already exists",
                        "Registration error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Registration successfull!",
                        "Registration",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Close();
                }
            }));
        }


        private void RegistrationEvent(object sender, EventArgs e)
        {
            GlobalData.Communicator.Registrate(_login.Text, _ConfirmPassword.Text);
        }

        private void _login_TextChanged(object sender, EventArgs e)
        {
           
        }
        private bool checkLogin(string loginString)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasDownChar = new Regex(@"[a-z]");
            var hasMinimum8Chars = new Regex(@".{5,24}");

            var isValidated = (hasNumber.IsMatch(loginString)
                || (hasUpperChar.IsMatch(loginString) || (hasDownChar.IsMatch(loginString)))
                && hasMinimum8Chars.IsMatch(loginString));
            return isValidated;
        }
        private bool checkPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasDownChar = new Regex(@"[a-z]");
            var hasMinimum8Chars = new Regex(@".{8,24}");

            var isValidated = hasNumber.IsMatch(password) 
                &&(hasUpperChar.IsMatch(password)||(hasDownChar.IsMatch(password)))
                && hasMinimum8Chars.IsMatch(password);
            return isValidated;
        }
    }
}
