using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using WPFclient.Infrastructure.Commands;
using WPFclient.ViewModels.Base;

namespace WPFclient.ViewModels
{
    public class RegisterVM : ViewModel
    {
        private readonly string apiBaseUrl = "http://a22946-8c78.g.d-f.pw/api/account/register";
        #region TextBox
        private string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string username;
        public string UserName
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        #endregion

        #region Command
        public event EventHandler RequestClose;
        public ICommand RegisterCommand { get; }

        private async void RegisterButton(object parameter)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста заполните все поля");
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                var userData = new { Username = login, Password = password };
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(apiBaseUrl, userData);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Регистрация прошла успешно.");
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка регистрации: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        public RegisterVM()
        {
            RegisterCommand = new RelayCommand(RegisterButton, p => true);
        }
    }
}
