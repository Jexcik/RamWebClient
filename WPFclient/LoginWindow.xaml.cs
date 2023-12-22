using System.Windows;
using System.Windows.Controls;
using WPFclient.ViewModels;

namespace WPFclient.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginWindowVM();
        }
    }
}
