using System;
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

        private string _dateChange;
        public string DateChange
        {
            get => _dateChange;

            set
            {
                if(_dateChange !=value)
                {
                    _dateChange = value;
                    OnPropertyChanged(nameof(DateChange));
                }
            }
        }

        private string _status;

        public string Status
        {
            get => _status;

            set
            {
                if(_status!=value)
                {
                    _status = value;
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
