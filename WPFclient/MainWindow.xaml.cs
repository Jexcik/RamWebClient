using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using WPFclient.ViewModels;
using WPFclient.ViewModels.Base;

namespace WPFclient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon notifyIcon;
        MainWindowVM viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowVM();
            DataContext = viewModel;
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            //Инициализация TaskBarIcon
            notifyIcon = new TaskbarIcon();
            notifyIcon.Icon = new Icon(@"C:\Users\e.egorov\Source\Repos\RamWebClient\WPFclient\Resources\logoRAM.ico");
            notifyIcon.ToolTipText = "Launcher";

            //Создаем меню и связываем его с командой
            var trayContextMenu = new ContextMenu();
            MenuItem mi_Open = new MenuItem();
            mi_Open.Header = "Открыть";
            mi_Open.Command = viewModel.TrayIconClickCommand;
            trayContextMenu.Items.Add(mi_Open);

            notifyIcon.TrayLeftMouseDown += (sender, e) =>
            {
                ShowNotification("Заголовок уведомления", "Текст уведомления");
            };
            notifyIcon.ContextMenu = trayContextMenu;

            //Подписка на событие открытия основного окна
            viewModel.OnOpenMainWindow += (sender, args) =>
            {
                //Показать основное окно
                this.Show();
            };
        }

        private void ShowNotification(string title, string message)
        {
            notifyIcon.ShowBalloonTip(title, message, BalloonIcon.Info);
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

    }
}
