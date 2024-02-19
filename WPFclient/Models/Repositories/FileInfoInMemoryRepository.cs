using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFclient.Models.Repositories
{
    public class FileInfoInMemoryRepository : IFileInfoRepository
    {
        private List<FileChangeInfo> _files = new List<FileChangeInfo>();

        /// <summary>
        /// Метод для добавления экземпляра в коллекцию
        /// </summary>
        /// <param name="fileInfo">Экземпляр объекта хранящего информацию о файле</param>
        public void Create(FileChangeInfo fileInfo)
        {
            var existingFileInfo = _files.FirstOrDefault(fi => fi.Id == fileInfo.Id);

            if (existingFileInfo == null)
            {
                _files.Add(fileInfo);
            }
        }

        /// <summary>
        /// Метод для удаления из списка записи хранящей информацию о файле
        /// </summary>
        /// <param name="fileInfo">Объект хронящий информацию о файле</param>
        public void Delete(FileChangeInfo fileInfo)
        {
            var existingFileInfo = _files.FirstOrDefault(fi => fi.Id == fileInfo.Id);

            _files.Remove(existingFileInfo);
        }

        /// <summary>
        /// Метод для получения всех экземпляров записей хранящих информацию о файлах
        /// </summary>
        /// <returns>Возвращает коллекцию объектов хранящих информациию о файле</returns>
        public List<FileChangeInfo> GetAll()
        {
            return _files;
        }

        /// <summary>
        /// Получение экземпляра записи хронящей информацию о файле
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Возвращает экземпляр записи хранящей информацию о файле</returns>
        public FileChangeInfo GetById(Guid id)
        {
            return _files.FirstOrDefault(fi => fi.Id == id);
        }

        /// <summary>
        /// Обновления экземпляра записи хранящей информацию о записи
        /// </summary>
        /// <param name="fileInfo">Экземпляр хранящий обновленную информацию</param>
        public void Update(FileChangeInfo fileInfo)
        {
            var existingFileInfo =_files.FirstOrDefault(fi=>fi.FileName==fileInfo.FileName);

            if(existingFileInfo != null) 
            {
                existingFileInfo.FileName = fileInfo.FileName;

                existingFileInfo.FilePath = fileInfo.FilePath;

                existingFileInfo.DateCreation = fileInfo.DateCreation;

                existingFileInfo.DateChange = fileInfo.DateChange;

                existingFileInfo.AuthorCreation = fileInfo.AuthorCreation;

                existingFileInfo.AuthorChange = fileInfo.AuthorChange;

                existingFileInfo.Status = fileInfo.Status;
            }
        }
    }
}
