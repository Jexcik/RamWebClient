using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using WPFclient.Models;
using WPFclient.Models.Repositories;
using WPFclient.ViewModels.Base;

namespace WPFclient.ViewModels
{
    public class ExternalServicesVM : ViewModel
    {
        private FileSystemWatcher fileSystemWatcher;

        public ObservableCollection<FileChangeInfo> fileChanges;

        private DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

        FileInfoInMemoryRepository fileInfoInMemoryRepository;

        private const string filePath = @"I:\03. Проекты\IDE-0156 РД_Кумроч_ЗИФ-ОИ_1-я оч_БГК\4. Работа\BIM Проект\02_Общие данные\04_1_КЖ";

        public ExternalServicesVM()
        {
            FileInfo[] filesInfo = directoryInfo.GetFiles("*.rvt");

            fileInfoInMemoryRepository = new FileInfoInMemoryRepository();

            fileInfoInMemoryRepository.GetAll().AddRange(filesInfo.Select(fi => new FileChangeInfo()
            {
                Status = "New",
                FileName = fi.Name,
                FilePath = fi.FullName,
                AuthorCreation = GetFileChangeAuthor(fi.FullName).Item1,
                AuthorChange = GetFileChangeAuthor(fi.FullName).Item2,
                DateCreation = fi.CreationTime.ToString("HH:mm:ss dd.MM.yyyy"),
                DateChange = fi.LastWriteTime.ToString("HH:mm:ss dd.MM.yyyy")
            }));

            fileChanges = new ObservableCollection<FileChangeInfo>(fileInfoInMemoryRepository.GetAll());

            InitializeFileSystemWatcher(filePath);
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
            AddFileChangeInfo(e.FullPath, e.Name);
        }
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            AddFileChangeInfo(e.FullPath, e.Name);
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {

        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {

        }

        private void AddFileChangeInfo(string filePath, string action)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                (string fileOwner, string lastModifiedBy) = GetFileChangeAuthor(filePath);

                var fileChangeInfo = new Models.FileChangeInfo
                {
                    FileName = Path.GetFileName(filePath),
                    FilePath = filePath,
                    DateChange = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"),
                    AuthorCreation = fileOwner,
                    AuthorChange = lastModifiedBy,
                    Status = action
                };
                fileChanges.Add(fileChangeInfo);
            });
        }


        private (string, string) GetFileChangeAuthor(string filePath)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            string creationAuthor = fileInfo.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            string lastModifiedAuthor = File.GetAccessControl(filePath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            return (creationAuthor, lastModifiedAuthor);
        }

        #endregion
    }
}
