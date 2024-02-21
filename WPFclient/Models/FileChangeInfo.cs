using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFclient.ViewModels.Base;

namespace WPFclient.Models
{
    public class FileChangeInfo:ViewModelBase
    {
        public Guid Id { get; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string AuthorCreation { get; set; }

        public string AuthorChange { get; set; }

        public string DateCreation { get; set; }

        public string DateChange { get; set; }

        private string status;

        public string Status
        {
            get => status;

            set
            {
                if(status!=value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public FileChangeInfo() 
        {
            Id = Guid.NewGuid();
        }
    }
}
