using System;
using System.Windows;
using WPFclient.Infrastructure.Commands.Base;
using WPFclient.Views;

namespace WPFclient.Infrastructure.Commands
{

    public class CloseWindow : Command
    {
        protected override void Execute(object parameter) => (parameter as Window ?? App.FocusedWindow ?? App.ActivedWindow)?.Close();

        protected override bool CanExecute(object parameter) => (parameter as Window ?? App.FocusedWindow ?? App.ActivedWindow) != null;
    }
}
