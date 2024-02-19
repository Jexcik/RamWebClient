using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFclient.Models
{
    public class FileChangeInfo
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string AuthorCreation { get; set; }

        public string AuthorChange { get; set; }

        public string DateCreation { get; set; }

        public string DateChange { get; set; }

        public string Status { get; set; }

        public FileChangeInfo() 
        {
            Id = Guid.NewGuid();
        }
    }
}
