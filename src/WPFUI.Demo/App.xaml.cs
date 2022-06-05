// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPFUI.Demo.Models;
using WPFUI.Demo.Services;
using WPFUI.Demo.Services.Contracts;
using WPFUI.Demo.ViewModels;
using WPFUI.Mvvm.Contracts;
using WPFUI.Mvvm.Services;

namespace WPFUI.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// A program abstraction.
    /// </summary>
    private IHost _host;

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        // For more information about .NET generic host see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0
        _host = Host.CreateDefaultBuilder(e.Args)
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            })
            .ConfigureServices(ConfigureServices)
            .Build();

        await _host.StartAsync();
    }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // App Host
        services.AddHostedService<ApplicationHostService>();

        // Theme manipulation
        services.AddSingleton<IThemeService, ThemeService>();

        // Taskbar manipulation
        services.AddSingleton<ITaskbarService, TaskbarService>();

        // Tray icon
        // Just in case you wondering, it  does not work yet
        // !! Experimental
        services.AddSingleton<INotifyIconService, NotifyIconService>();

        // Page resolver service
        services.AddSingleton<IPageService, PageService>();

        // Page resolver service
        services.AddSingleton<ITestWindowService, TestWindowService>();

        // Service containing navigation, same as INavigationWindow... but without window
        services.AddSingleton<INavigationService, NavigationService>();

        // Main window container with navigation
        services.AddScoped<INavigationWindow, Views.Container>();
        services.AddScoped<ContainerViewModel>();

        // Views and ViewModels
        services.AddScoped<Views.Pages.Dashboard>();

        services.AddScoped<Views.Pages.ExperimentalDashboard>();
        services.AddScoped<ExperimentalViewModel>();

        services.AddScoped<Views.Pages.Controls>();

        services.AddScoped<Views.Pages.Menus>();

        services.AddScoped<Views.Pages.Colors>();
        services.AddScoped<ColorsViewModel>();

        services.AddScoped<Views.Pages.Debug>();
        services.AddScoped<DebugViewModel>();

        services.AddScoped<Views.Pages.Buttons>();
        services.AddScoped<ButtonsViewModel>();

        services.AddScoped<Views.Pages.Data>();
        services.AddScoped<DataViewModel>();

        services.AddScoped<Views.Pages.Input>();
        services.AddScoped<InputViewModel>();

        services.AddScoped<Views.Pages.Icons>();
        services.AddScoped<IconsViewModel>();

        // Test windows
        services.AddTransient<Views.Windows.EditorWindow>();
        services.AddTransient<Views.Windows.TaskManagerWindow>();
        services.AddTransient<Views.Windows.SettingsWindow>();
        services.AddScoped<Views.Windows.StoreWindow>();
        services.AddScoped<Views.Windows.ExperimentalWindow>();

        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
        _host = null;
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}
