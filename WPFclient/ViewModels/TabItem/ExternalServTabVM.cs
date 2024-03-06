using System.Windows.Input;
using WPFclient.Infrastructure.Commands;
using WPFclient.ViewModels.Base;
using WPFclient.Views;

namespace WPFclient.ViewModels.TabItem
{
    public class ExternalServTabVM : ViewModelBase
    {
        private readonly ExternalServicesVM _externalServicesVM;

        public ExternalServTabVM(ExternalServicesVM externalServicesVM)
        {
            _externalServicesVM = externalServicesVM;

            OpenMonitoringCommand = new RelayCommand(OpenMonitoring, p => true);
        }

        #region Command
        public ICommand OpenMonitoringCommand { get; }

        private void OpenMonitoring(object parameter)
        {
            MonitoringWindow monitoringWindow = new MonitoringWindow();

            monitoringWindow.DataContext = _externalServicesVM;

            monitoringWindow.WindowState = System.Windows.WindowState.Maximized;

            monitoringWindow.Show();
        }
        #endregion
    }
}
