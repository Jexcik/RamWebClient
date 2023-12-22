using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WPFclient.ViewModels;

namespace WPFclient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();
        }
    }
}
