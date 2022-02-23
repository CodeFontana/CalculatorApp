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
using WpfCalculator.ViewModels;
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
		/// Although in this example, we will only be taking advantage of DI (for the moment).
		/// </summary>
		private IHost _CalcHost;

		/// <summary>
		/// Override of OnStartup(), to build the Host and utilize Dependency Injection, and then
		/// display the main Calculator View.
		/// </summary>
		/// <param name="e"></param>
		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			_CalcHost = new HostBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddTransient<IMathService, MathService>();
					services.AddTransient<CalculatorViewModel>();
					services.AddTransient<CalculatorWindow>();
				})
				.Build();

			await _CalcHost.StartAsync();

			var calcWindow = _CalcHost.Services.GetService<CalculatorWindow>();
			calcWindow.DataContext = _CalcHost.Services.GetService<CalculatorViewModel>();
			calcWindow.Show();
		}

		/// <summary>
		/// Override of OnExit(), to gracefully stop the host before exitting the
		/// application.
		/// </summary>
		/// <param name="e"></param>
		protected override async void OnExit(ExitEventArgs e)
		{
			using (_CalcHost)
			{
				await _CalcHost.StopAsync(TimeSpan.FromSeconds(5));
			}
			base.OnExit(e);
		}
	}
}
