using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfCalculator.ViewModels;
using WpfCalculator.Views;

namespace WpfCalculator;

public partial class App : Application
{
    private IHost _calcHost;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _calcHost = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<CalculatorViewModel>();
                services.AddTransient<CalculatorWindow>();
            })
            .Build();

        await _calcHost.StartAsync();

        var calcWindow = _calcHost.Services.GetService<CalculatorWindow>();
        calcWindow.DataContext = _calcHost.Services.GetService<CalculatorViewModel>();
        calcWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_calcHost)
        {
            await _calcHost.StopAsync(TimeSpan.FromSeconds(5));
        }
        base.OnExit(e);
    }
}
