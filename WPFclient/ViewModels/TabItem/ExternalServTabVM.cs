using System.Windows.Input;
using WPFclient.Infrastructure.Commands;
using WPFclient.Services;
using WPFclient.Services.Interfaces;
using WPFclient.ViewModels.Base;
using WPFclient.Views;

namespace WPFclient.ViewModels.TabItem
{
    public class ExternalServTabVM : ViewModelBase
    {
        private readonly IFileChangeDataService _fileChangeDataService;

        public ExternalServTabVM(IFileChangeDataService fileChangeDataService)
        {
            _fileChangeDataService = fileChangeDataService;

            OpenMonitoringCommand = new RelayCommand(OpenMonitoring, p => true);
        }

        #region Command
        public ICommand OpenMonitoringCommand { get; }

        private void OpenMonitoring(object parameter)
        {
            MonitoringWindow monitoringWindow = new MonitoringWindow();

            monitoringWindow.DataContext = _fileChangeDataService;

            monitoringWindow.WindowState=System.Windows.WindowState.Maximized;

            monitoringWindow.Show();
        }
        #endregion
    }
}
