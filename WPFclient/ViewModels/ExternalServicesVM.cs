﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFclient.Infrastructure.Commands;
using WPFclient.Models;
using WPFclient.Models.Repositories;
using WPFclient.ViewModels.Base;
using WPFclient.Views;

namespace WPFclient.ViewModels
{
    public class ExternalServicesVM : ViewModelBase
    {
        private FileSystemWatcher fileSystemWatcher;

        private ObservableCollection<FileChangeInfo> fileChanges;

        public ObservableCollection<FileChangeInfo> FileChanges
        {
            get => fileChanges;
            set
            {
                fileChanges = value;
                OnPropertyChanged(nameof(FileChanges));
            }
        }

        private DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

        FileInfoInMemoryRepository fileInfoInMemoryRepository;

        private const string filePath = @"I:\03. Проекты\IDE-0156 РД_Кумроч_ЗИФ-ОИ_1-я оч_БГК\4. Работа\BIM Проект\02_Общие данные\04_1_КЖ";

        public ExternalServicesVM()
        {
            FileInfo[] filesInfo = directoryInfo.GetFiles("*.txt");

            fileInfoInMemoryRepository = new FileInfoInMemoryRepository();

            fileInfoInMemoryRepository.GetAll().AddRange(filesInfo.Select(fi => new FileChangeInfo()
            {
                Status = "Created",
                FileName = fi.Name,
                FilePath = fi.FullName,
                AuthorCreation = GetFileChangeAuthor(fi.FullName).Item1.Split('\\').Last(),
                AuthorChange = GetFileChangeAuthor(fi.FullName).Item2.Split('\\').Last(),
                DateCreation = fi.CreationTime.ToString("HH:mm:ss dd.MM.yyyy"),
                DateChange = fi.LastWriteTime.ToString("HH:mm:ss dd.MM.yyyy")
            }));

            fileChanges = new ObservableCollection<FileChangeInfo>(fileInfoInMemoryRepository.GetAll());

            InitializeFileSystemWatcher(filePath);

            OpenMonitoringCommand = new RelayCommand(OpenMonitoring, p => true);
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

            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;

            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (FileChanges.FirstOrDefault(fc => fc.FileName == e.Name) == null)
                {
                    FileChangeInfo fileChangeInfo = AddFileChangeInfo(e.FullPath, "Created");

                    FileChanges.Add(fileChangeInfo);
                }
                else
                {
                    FileChangeInfo fileChangeInfo = FileChanges.FirstOrDefault(fc => fc.FileName == e.Name);

                    fileChangeInfo.Status = e.ChangeType.ToString();

                    fileChangeInfo.DateChange = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
                }
            });
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {

        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {

            //FileChanges.Remove(fileChangeInfo);
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {

        }

        private FileChangeInfo AddFileChangeInfo(string filePath, string action)
        {
            FileChangeInfo fileChangeInfo = new FileChangeInfo();

            (string fileOwner, string lastModifiedBy) = GetFileChangeAuthor(filePath);

            fileChangeInfo.FileName = Path.GetFileName(filePath);
            fileChangeInfo.FilePath = filePath;
            fileChangeInfo.DateChange = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
            fileChangeInfo.AuthorCreation = fileOwner;
            fileChangeInfo.AuthorChange = lastModifiedBy;
            fileChangeInfo.Status = action;

            return fileChangeInfo;
        }


        private (string, string) GetFileChangeAuthor(string filePath)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            string creationAuthor = fileInfo.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            string lastModifiedAuthor = File.GetAccessControl(filePath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            return (creationAuthor, lastModifiedAuthor);
        }

        #endregion

        #region Command
        public ICommand OpenMonitoringCommand { get; }

        private void OpenMonitoring(object parameter)
        {
            MonitoringWindow monitoringWindow = new MonitoringWindow();

            monitoringWindow.Show();
        }

        #endregion
    }
}
