using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfCalculator.ViewModels;
using WpfCalculator.Views;

namespace WpfCalculator;

public partial class App : Application
{
    private IHost? _calcHost;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            _calcHost = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<CalculatorViewModel>();
                    services.AddTransient<CalculatorWindow>();
                })
                .Build();

            await _calcHost.StartAsync();

            CalculatorWindow calcWindow = _calcHost.Services.GetRequiredService<CalculatorWindow>();
            calcWindow.DataContext = _calcHost.Services.GetRequiredService<CalculatorViewModel>();
            calcWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start application: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(-1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_calcHost is not null)
        {
            try
            {
                using (_calcHost)
                {
                    await _calcHost.StopAsync(TimeSpan.FromSeconds(5));
                }
            }
            catch
            {
                // Swallow shutdown failures so they don't mask the real exit reason.
            }
        }

        base.OnExit(e);
    }
}
