using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Controls;
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
        private readonly MainWindowVM viewModel;
        private FileSystemWatcher fileSystemWatcher;
        private ObservableCollection<FileChangeModel> fileChanges;

        private readonly DirectoryInfo directoryInfo;

        const string filePath = @"I:\03. Проекты\IDE-0156 РД_Кумроч_ЗИФ-ОИ_1-я оч_БГК\4. Работа\BIM Проект\02_Общие данные\04_1_КЖ";

        public MainWindow()
        {
            InitializeComponent();

            directoryInfo = new DirectoryInfo(filePath);

            FileInfo[] filesInfo = directoryInfo.GetFiles("*.rvt");

            var customFiles = filesInfo.Select(fi => new FileChangeModel
            {
                FileName = fi.Name,
                FilePath = fi.FullName,
                AuthorCreation= GetFileChangeAuthor(fi.FullName).Item1,
                AuthorChange= GetFileChangeAuthor(fi.FullName).Item2,
                DateCreation = fi.CreationTime.ToString("HH:mm:ss dd.MM.yyyy"),
                DateChange=fi.LastWriteTime.ToString("HH:mm:ss dd.MM.yyyy")
            });

            fileChanges = new ObservableCollection<FileChangeModel>(customFiles);

            fileChangesDataGrid.ItemsSource = fileChanges;

            InitializeFileSystemWatcher(filePath);

            viewModel = new MainWindowVM();

            DataContext = viewModel;

            InitializeTray();

        }

        private void InitializeTray()
        {
            //Инициализация TaskBarIcon
            notifyIcon = new TaskbarIcon();
            notifyIcon.Icon = new Icon(Properties.Resource.logoRAM, new System.Drawing.Size(16, 16));
            notifyIcon.ToolTipText = "Launcher";
            notifyIcon.LeftClickCommand = viewModel.LeftClickCommand;

            //Создаем меню и связываем его с командой
            var trayContextMenu = new ContextMenu();

            MenuItem mi_Open = new MenuItem();
            mi_Open.Header = "Открыть";
            mi_Open.Command = viewModel.TrayOpenClickCommand;

            MenuItem mi_Close = new MenuItem();
            mi_Close.Header = "Выход";
            mi_Close.Command = viewModel.TrayCloseClickCommand;

            trayContextMenu.Items.Add(mi_Open);
            trayContextMenu.Items.Add(mi_Close);

            notifyIcon.ContextMenu = trayContextMenu;

            //Подписка на событие открытия основного окна
            viewModel.OnOpenMainWindow += (sender, args) =>
            {
                //Показать основное окно
                this.Show();
                this.WindowState = WindowState.Normal;
                this.Activate();
            };
            viewModel.OnCloseMainWindow += (sender, args) => this.Close();

            viewModel.OnShowNotification += (sender, args) => ShowNotification("Revit", viewModel.TextInfo);
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

        #region Внешние службы
        private void InitializeFileSystemWatcher(string path)
        {
            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = path;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fileSystemWatcher.Filter = "*.*";

            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {

        }
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {

        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {

        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {

        }

        private void AddFileChangeInfo(string filePath, string action)
        {

            Dispatcher.Invoke(() =>
            {
                (string fileOwner, string lastModifiedBy) = GetFileChangeAuthor(filePath);

                var fileChangeInfo = new FileChangeModel
                {
                    FileName = Path.GetFileName(filePath),
                    FilePath = filePath,
                    DateChange = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"),
                    AuthorCreation=fileOwner,
                    AuthorChange = lastModifiedBy,
                    Action = action
                };
                fileChanges.Add(fileChangeInfo);
            });
        }

        private (string, string) GetFileChangeAuthor(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string creationAuthor = fileInfo.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            string lastModifiedAuthor = System.IO.File.GetAccessControl(filePath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            return (creationAuthor, lastModifiedAuthor);
        }

        #endregion

    }
}
