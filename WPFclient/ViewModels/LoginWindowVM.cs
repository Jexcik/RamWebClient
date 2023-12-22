using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows;
using WPFclient.ViewModels.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Input;
using WPFclient.Infrastructure.Commands;
using WPFclient.Views;

namespace WPFclient.ViewModels
{
    public class LoginWindowVM : ViewModel
    {
        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string password;
        public string Password
        {
            get => password;
            private set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        #region Command

        public ICommand RegisterCommand { get; }
        private void Register(object parameter)
        {
            var registerWindow = new Register();
            registerWindow.ShowDialog();
        }
        public ICommand LoginCommand { get; }
        private void EnterLogin(object parameter)
        {

        }
        #endregion
        public LoginWindowVM()
        {
            LoginCommand = new RelayCommand(EnterLogin, p => true);
            RegisterCommand = new RelayCommand(Register, p => true);
        }

    }
}
