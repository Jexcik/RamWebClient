using System;

namespace WPFclient.Models
{
    public class FileData
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string LocalFileFolder { get; set; }
        public string ChangeInfo { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"В плагине {FileName} добавлены следующие изменения {ChangeInfo}\n\n";
        }
    }
}
