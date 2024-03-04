using System.Collections.Generic;
using WPFclient.Models;

namespace WPFclient.Services.Interfaces
{
    public interface IFileChangeDataService
    {
        List<FileChangeInfo> FileChanges { get; set; }
    }
}