using System.Windows.Input;
using WPFclient.Infrastructure.Commands;
using WPFclient.ViewModels.Base;
using WPFclient.Views;

namespace WPFclient.ViewModels
{
    public class ExternalServTabVM : ViewModelBase
    {
        public ExternalServTabVM()
        {
            OpenMonitoringCommand = new RelayCommand(OpenMonitoring, p => true);
        }
        #region Command
        public ICommand OpenMonitoringCommand { get; }

        private void OpenMonitoring(object parameter)
        {
            MonitoringWindow monitoringWindow = new MonitoringWindow();

            monitoringWindow.DataContext = new ExternalServicesVM();

            monitoringWindow.Show();
        }
        #endregion
    }
}
