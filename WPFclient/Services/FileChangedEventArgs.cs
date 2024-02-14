using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFclient.Models
{
    public class FileChangedEventArgs : EventArgs
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Author { get; set; }
        public DateTime DateChange { get; set; }
    }
}
