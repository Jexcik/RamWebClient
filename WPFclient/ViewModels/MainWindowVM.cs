using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WPFclient.Infrastructure.Commands;
using WPFclient.Models;
using WPFclient.ViewModels.Base;
using WPFclient.Views;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace WPFclient.ViewModels
{
    public class MainWindowVM : ViewModel
    {
        #region Центр обновлений

        #region Трей
        public ICommand TrayOpenClickCommand { get; set; }
        public ICommand TrayCloseClickCommand { get; set; }
        public ICommand LeftClickCommand { get; set; }

        //Логика обработки клика на значке в трее
        private void TrayOpenClick(object parameter)
        {
            OnOpenMainWindow?.Invoke(this, EventArgs.Empty);
        }
        private void TrayCloseClick(object parameter)
        {
            OnCloseMainWindow?.Invoke(this, EventArgs.Empty);
        }

        private void LeftClick(object parameter)
        {
            OnShowNotification?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler OnOpenMainWindow;
        public event EventHandler OnCloseMainWindow;
        public event EventHandler OnShowNotification;
        #endregion

        #region Таймер
        private DispatcherTimer updateTimer;
        private void UpdateTimerInterval()
        {
            //Установка интервала в зависимости от выбранного RadioButton
            if (IsWhileLoading)
            {
                updateTimer.Tick -= UpdateTimer_Tick;
                updateTimer.Interval = TimeSpan.FromSeconds(60);
                updateTimer.Tick += UpdateTimer_Tick;
            }
            if (IsEveryTenMinutes)
            {
                updateTimer.Tick -= UpdateTimer_Tick;
                updateTimer.Interval = TimeSpan.FromSeconds(600);
                updateTimer.Tick += UpdateTimer_Tick;
            }
            if (IsEachHour)
            {
                updateTimer.Tick -= UpdateTimer_Tick;
                updateTimer.Interval = TimeSpan.FromSeconds(3600);
                updateTimer.Tick += UpdateTimer_Tick;
            }

            //Запуск таймера
            updateTimer.Start();
        }


        #region RadioButton
        private bool isWhileLoading;
        public bool IsWhileLoading
        {
            get => isWhileLoading;
            set
            {
                if (isWhileLoading != value)
                {
                    isWhileLoading = value;
                    OnPropertyChanged(nameof(IsWhileLoading));
                    UpdateTimerInterval();
                }
            }
        }

        private bool isEveryTenMinutes;
        public bool IsEveryTenMinutes
        {
            get => isEveryTenMinutes;
            set
            {
                if (isEveryTenMinutes != value)
                {
                    isEveryTenMinutes = value;
                    OnPropertyChanged(nameof(IsEveryTenMinutes));
                    UpdateTimerInterval();
                }
            }
        }

        private bool isEachHour;
        public bool IsEachHour
        {
            get => isEachHour;
            set
            {
                if (isEachHour != value)
                {
                    isEachHour = value;
                    OnPropertyChanged(nameof(IsEachHour));
                    UpdateTimerInterval();
                }
            }
        }

        #endregion


        #endregion

        #region ТекстБоксы
        private string title = "AutoUpdater";
        public string Title
        {
            get => title;
            set
            {
                if (Equals(title, value))
                {
                    return;
                }
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string textInfo;
        public string TextInfo
        {
            get => textInfo;
            set
            {
                if (textInfo != value)
                {
                    textInfo = value;
                    OnPropertyChanged(nameof(TextInfo));
                }
            }
        }

        private string version = "Версия модуля:1.0";
        public string Version
        {
            get => version;
        }
        #endregion

        #region Command
        public ICommand AuthorizationCommand { get; }
        private async void AuthenticateAndDownload(object parameter)
        {
            var loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                string username = loginWindow.UserNameTextBox.Text;
                string password = loginWindow.PasswordBox.ToString();
                string updateFilePath = "путь_к_файлу_обновления";

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {

                }
                else
                {

                }
            }
        }

        public ICommand UpdateCommand { get; }
        private event EventHandler OnUpdate;
        private void Update(object parameter)
        {
            OnUpdate += UpdateTimer_Tick;
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }

        public ICommand UnloadCommand { get; }

        private void Unload(object parameter)
        {

        }
        #endregion

        #endregion

        #region Внешние службы
        public ObservableCollection<TotalInformation> FileInform { get; set; }= new ObservableCollection<TotalInformation>()
        {
            new TotalInformation("KUM","....","ABC"),
            new TotalInformation("KZM","www.Kremlin.ru","IFG")
        };
        #endregion

        public MainWindowVM()
        {
            AuthorizationCommand = new RelayCommand(AuthenticateAndDownload, p => true);

            //Принудительное обновление плагинов через анонимный метод
            UpdateCommand = new RelayCommand(Update, p => true);

            //Инициализация таймера
            updateTimer = new DispatcherTimer();

            TrayOpenClickCommand = new RelayCommand(TrayOpenClick, p => true);

            TrayCloseClickCommand = new RelayCommand(TrayCloseClick, p => true);

            LeftClickCommand = new RelayCommand(LeftClick, p => true);

            UnloadCommand = new RelayCommand(Unload, p => true);

            //FileInform = new ObservableCollection<TotalInformation>
            //{
            //    new TotalInformation("KUM","....","ABC"),
            //    new TotalInformation("KZM","www.Kremlin.ru","IFG")
            //};
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            Process process = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.Equals("Revit"));
            try
            {
                //Получение актуальной информации о файлах на сервере
                List<FileData> serverLastModified = await ApiManager.GetServerFilesLastModifiedDateAsync();

                //Получение локальной даты последнего изменения файла
                List<DateTime> localLastModified = new List<DateTime>();

                foreach (FileData file in serverLastModified)
                {
                    string localFilePath = $"{file.LocalFileFolder.Replace("%username%", ApiManager.GetLocalUserName())}\\{file.FileName}.dll";
                    localLastModified.Add(File.GetLastWriteTime(localFilePath));
                }
                List<string> fileExist = new List<string>
                {
                    ".dll",
                    ".txt",
                    ".png",
                };

                //Сравнивание дат
                for (int i = 0; i < serverLastModified.Count; i++)
                {
                    if (serverLastModified[i].Date > localLastModified[i])
                    {
                        if (process == null)
                        {
                            int counter = 3;
                            if (serverLastModified[i].FileName.Contains("RibbonRAM"))
                            {
                                fileExist.Clear();
                                fileExist.AddRange(new string[] { ".dll", ".addin" });
                                counter = 2;
                            }

                            for (int j = 0; j < counter; j++)
                            {
                                await ApiManager.DownloadFileAsync(serverLastModified[i].FileName + fileExist[j], serverLastModified[i].LocalFileFolder);
                            }

                            TextInfo += $"В плагин {serverLastModified[i].ToString()}";
                            OnShowNotification?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            TextInfo = "Вышли новые обновления!\nДля установки закройте Revit.";
                            OnShowNotification?.Invoke(this, EventArgs.Empty);
                            TextInfo = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверки обновлений: {ex.Message}");
            }
        }
    }
}
