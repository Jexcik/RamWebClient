using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFclient.Models;

namespace WPFclient.Services.Interface
{
    public interface IFileChangeService
    {
        event EventHandler <FileChangedEventArgs> FileChange;

        void StartWatching(string dirPath);
        void StopWatching();
    }
}
