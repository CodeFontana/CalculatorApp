using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfCalculator.Services;
using WpfCalculator.Views;

namespace WpfCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
{
        /// <summary>
        /// Use .NET generic Host to encapsulate Dependency Injection (DI), Logging and Configuration.
        /// Although in this example, we will only be taking advantage of DI.
        /// </summary>
        private IHost _host;

        /// <summary>
        /// Default constructor for adding services to DI container.
        /// </summary>
        public App()
        {
            _host = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IMathService, MathService>();
                    services.AddSingleton<CalculatorWindow>();
                })
                .Build();
        }

        /// <summary>
        /// Application startup, as specified in App.xaml, to start the Host and display
        /// the Calculator window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var calcWindow = _host.Services.GetService<CalculatorWindow>();
            calcWindow.Show();
        }

        /// <summary>
        /// Application exit, as specified in the App.xaml, to ensure the Host
        /// is properly stopped.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
