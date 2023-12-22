using System;

namespace RAMWebServer.Models
{
    public partial class FileSaveDateController
    {
        public class FileData
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
