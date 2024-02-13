using System;
using System.IO;
using WPFclient.Models;

namespace WPFclient.Services
{
    public class DataService
    {
        private TotalInformation OnChanged(object sender, FileSystemEventArgs e)
        {
            return new TotalInformation(e.Name, e.FullPath, e.Name);
        }

    }
}
