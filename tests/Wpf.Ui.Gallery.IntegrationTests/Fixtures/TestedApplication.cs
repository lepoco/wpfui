// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using FlaUI.Core;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace Wpf.Ui.Gallery.IntegrationTests.Fixtures;

/// <summary>
/// Class managing the lifecycle of the tested application implementing <see cref="IAsyncLifetime"/>.
/// Uses <see cref="UIA3Automation"/> for UI automation.
/// </summary>
public sealed class TestedApplication : IAsyncLifetime
{
    private const string ExecutableName = "Wpf.Ui.Gallery.exe";

    private readonly AutomationBase automation = new UIA3Automation();

    private Application? app;

    private Window? mainWindow;

    /// <summary>
    /// Gets the wrapper for an application which should be automated.
    /// </summary>
    public Application? Application => app;

    /// <summary>
    /// Gets the main window of the applications process.
    /// </summary>
    public Window? MainWindow => mainWindow ??= app?.GetMainWindow(automation);

    /// <inheritdoc />
    public ValueTask InitializeAsync()
    {
        if (app is not null)
        {
            app.Close();
            app.Dispose();
        }

        string path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            ExecutableName
        );

        if (!File.Exists(path))
        {
            throw new InvalidOperationException(
                $"Unable to find the application executable at path \"{path}\"."
            );
        }

        app = Application.Launch(path);
        app.WaitWhileMainHandleIsMissing(TimeSpan.FromMinutes(1));

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        if (app is not null)
        {
            if (!app.HasExited)
            {
                app.Close();
            }

            // ReSharper disable once AccessToDisposedClosure
            Retry.WhileFalse(() => app?.HasExited ?? true, TimeSpan.FromSeconds(2), ignoreException: true);

            app.Dispose();

            app = null;
        }

        automation?.Dispose();

        return ValueTask.CompletedTask;
    }
}
