using System;
using System.IO;
using WPFclient.Models;

namespace WPFclient.Services
{
    public class DataService
    {
        private TotalInformation OnChanged(object sender, FileSystemEventArgs e)
        {
            return new TotalInformation()
            {
                ModelName=e.Name,
                FilePath=e.FullPath,
            };
        }

    }
}
