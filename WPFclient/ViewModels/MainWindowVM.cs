﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using WPFclient.Infrastructure.Commands;
using WPFclient.Models;
using WPFclient.ViewModels.Base;
using WPFclient.Views;

namespace WPFclient.ViewModels
{
    public class MainWindowVM : ViewModel
    {
        #region Таймер
        private DispatcherTimer updateTimer;

        private void UpdateTimerInterval()
        {
            //Установка интервала в зависимости от выбранного RadioButton
            if (IsWhileLoading)
            {
                updateTimer.Tick -= UpdateTimer_Tick;
                updateTimer.Interval = TimeSpan.FromSeconds(10);
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
        private string title = "Главное окно";
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
                textInfo = value;
                OnPropertyChanged(nameof(TextInfo));
            }
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
        #endregion

        public MainWindowVM()
        {
            AuthorizationCommand = new RelayCommand(AuthenticateAndDownload, p => true);

            //Инициализация таймера
            updateTimer = new DispatcherTimer();
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
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

                        TextInfo += serverLastModified[i].ToString();
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
