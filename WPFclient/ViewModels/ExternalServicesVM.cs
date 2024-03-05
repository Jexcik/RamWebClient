using System.Collections.ObjectModel;
using WPFclient.Models;
using WPFclient.Services.Interfaces;
using WPFclient.ViewModels.Base;

namespace WPFclient.ViewModels
{
    public class ExternalServicesVM : ViewModelBase
    {
        public ExternalServicesVM(IFileChangeDataService fileChangeDataService)
        {
            fileChanges = new ObservableCollection<FileChangeInfo>(fileChangeDataService.FileChanges);
        }

        private ObservableCollection<FileChangeInfo> fileChanges;

        public ObservableCollection<FileChangeInfo> FileChanges
        {
            get => fileChanges;
            set
            {
                fileChanges = value;
                OnPropertyChanged(nameof(FileChanges));
            }
        }

        private string _title = "Папка обмен";

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }



    }
}
