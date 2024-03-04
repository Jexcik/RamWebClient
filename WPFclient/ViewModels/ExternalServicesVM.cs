using System.Collections.ObjectModel;
using WPFclient.Models;
using WPFclient.Services;
using WPFclient.Services.Interfaces;
using WPFclient.ViewModels.Base;

namespace WPFclient.ViewModels
{
    public class ExternalServicesVM : ViewModelBase
    {
        private readonly IFileChangeDataService _fileChangeDataService;
        public ExternalServicesVM(IFileChangeDataService fileChangeDataService)
        {
            _fileChangeDataService = fileChangeDataService;

            fileChanges = new ObservableCollection<FileChangeInfo>(_fileChangeDataService.FileChanges);
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
