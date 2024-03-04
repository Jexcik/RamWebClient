using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using WPFclient.Models;
using WPFclient.Services.Interfaces;

namespace WPFclient.Services
{
    public class FileChangeDataService : IFileChangeDataService
    {
        private DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

        public List<FileChangeInfo> FileChanges { get; set; }

        private const string filePath = @"I:\03. Проекты\IDE-0156 РД_Кумроч_ЗИФ-ОИ_1-я оч_БГК\4. Работа\BIM Проект\02_Общие данные";

        public FileChangeDataService()
        {
            FileChanges = new List<FileChangeInfo>();

            FileInfo[] filesInfo = directoryInfo.GetFiles("*.rvt", SearchOption.AllDirectories);

            FileChanges.AddRange(filesInfo.Select(fi => new FileChangeInfo()
            {
                Status = "Created",
                FileName = fi.Name,
                FilePath = fi.FullName,
                AuthorCreation = GetFileChangeAuthor(fi.FullName).Item1.Split('\\').Last(),
                AuthorChange = GetFileChangeAuthor(fi.FullName).Item2.Split('\\').Last(),
                DateCreation = fi.CreationTime.ToString("HH:mm:ss dd.MM.yyyy"),
                DateChange = fi.LastWriteTime.ToString("HH:mm:ss dd.MM.yyyy")
            }));

            InitializeFileSystemWatcher(filePath);
        }
        private void InitializeFileSystemWatcher(string path)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = path;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fileSystemWatcher.Filter = "*.rvt*";

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
            FileInfo fileInfo = new FileInfo(filePath);
            string creationAuthor = fileInfo.GetAccessControl().GetOwner(typeof(System.Security.Principal.NTAccount))?.ToString() ?? "Владелец файла не найден!";
            string lastModifiedAuthor = File.GetAccessControl(filePath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            return (creationAuthor, lastModifiedAuthor);
        }
    }
}
