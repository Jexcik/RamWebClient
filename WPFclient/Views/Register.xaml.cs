using System.Windows;
using WPFclient.ViewModels;

namespace WPFclient.Views
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            DataContext = new RegisterVM();
            (DataContext as RegisterVM).RequestClose += (s, e) => Close();
        }
    }
}
