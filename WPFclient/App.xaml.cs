using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using WPFclient.Services;
using WPFclient.Services.Interfaces;
using WPFclient.ViewModels;
using WPFclient.ViewModels.TabItem;

namespace WPFclient.Views
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        public static bool IsDesignMode { get; private set; } = true;

        private static IHost _Host;

        public static IHost Host = _Host ?? Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            base.OnStartup(e);

            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            using (Host) await Host.StopAsync();
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddServices();

            services.AddViewModel();
        }

        public static string CurrentDirectory => IsDesignMode
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
