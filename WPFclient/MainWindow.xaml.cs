using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UIFramework;
using WPFclient.Models;
using WPFclient.ViewModels;

namespace WPFclient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon notifyIcon;

        ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();

            DataContext = viewModel;

            InitializeTray(viewModel.MainWindowVM);
        }

        private void InitializeTray(MainWindowVM mainWindowVM)
        {
            //Инициализация TaskBarIcon
            notifyIcon = new TaskbarIcon();
            notifyIcon.Icon = new Icon(Properties.Resource.logoRAM, new System.Drawing.Size(16, 16));
            notifyIcon.ToolTipText = "Launcher";
            notifyIcon.LeftClickCommand = mainWindowVM.LeftClickCommand;

            //Создаем меню и связываем его с командой
            var trayContextMenu = new ContextMenu();

            MenuItem mi_Open = new MenuItem();
            mi_Open.Header = "Открыть";
            mi_Open.Command = mainWindowVM.TrayOpenClickCommand;

            MenuItem mi_Close = new MenuItem();
            mi_Close.Header = "Выход";
            mi_Close.Command = mainWindowVM.TrayCloseClickCommand;

            trayContextMenu.Items.Add(mi_Open);
            trayContextMenu.Items.Add(mi_Close);

            notifyIcon.ContextMenu = trayContextMenu;

            //Подписка на событие открытия основного окна
            mainWindowVM.OnOpenMainWindow += (sender, args) =>
            {
                //Показать основное окно
                this.Show();
                this.WindowState = WindowState.Normal;
                this.Activate();
            };
            mainWindowVM.OnCloseMainWindow += (sender, args) => this.Close();

            mainWindowVM.OnShowNotification += (sender, args) => ShowNotification("Revit", mainWindowVM.TextInfo);
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
