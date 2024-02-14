using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFclient.Models
{
    public class FileChangeModel:INotifyPropertyChanged
    {
        public FileChangeModel(string fileName, string filePath, string author, DateTime dateChange)
        {
            FileName = fileName;
            FilePath = filePath;
            Author = author;
            DateChange = dateChange;
        }

        private string _fileName;
        private string _filePath;
        private string _author;
        private DateTime _date;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateChange
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
