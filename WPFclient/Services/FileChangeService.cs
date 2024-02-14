using System;
using System.IO;
using WPFclient.Models;
using WPFclient.Services.Interface;

namespace WPFclient.Services
{
    public class FileChangeService : IFileChangeService
    {
        public event EventHandler<FileChangedEventArgs> FileChange;

        private FileSystemWatcher fileSystemWatcher;

        public void StartWatching(string dirPath)
        {
            fileSystemWatcher = new FileSystemWatcher
            {
                Path = dirPath,
                Filter = ".txt",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                EnableRaisingEvents = true
            };
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        }

        public void StopWatching()
        {
            fileSystemWatcher?.Dispose();
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            OnFileChanged(new FileChangedEventArgs
            {
                FileName = e.Name,
                FilePath=e.FullPath,
                DateChange=DateTime.Now,
                Author="ИМЯ_АВТОРА"
            });
        }

        protected virtual void OnFileChanged(FileChangedEventArgs e)
        {
            FileChange?.Invoke(this, e);
        }
    }
}
