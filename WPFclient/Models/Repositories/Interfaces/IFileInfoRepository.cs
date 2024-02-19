using System;
using System.Collections.Generic;

namespace WPFclient.Models.Repositories
{
    public interface IFileInfoRepository
    {
        List<FileChangeInfo> GetAll();
        FileChangeInfo GetById(Guid id);
        void Create(FileChangeInfo fileInfo);
        void Update(FileChangeInfo fileInfo);
        void Delete(FileChangeInfo fileInfo);
    }
}
