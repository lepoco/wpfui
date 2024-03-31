// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui;

/// <summary>
/// Represents a UI application.
/// </summary>
public class UiApplication
{
    private static UiApplication? _uiApplication;

    private readonly Application? _application;

    private ResourceDictionary? _resources;

    private Window? _mainWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="UiApplication"/> class.
    /// </summary>
    public UiApplication(Application application)
    {
        _application = application;
    }

    /// <summary>
    /// Gets a value indicating whether the application is running outside of the desktop app context.
    /// </summary>
    public bool IsApplication => _application is not null;

    /// <summary>
    /// Gets the current application.
    /// </summary>
    public static UiApplication Current
    {
        get
        {
            _uiApplication ??= new UiApplication(Application.Current);

            return _uiApplication;
        }
    }

    /// <summary>
    /// Gets or sets the application's main window.
    /// </summary>
    public Window? MainWindow
    {
        get => _application?.MainWindow ?? _mainWindow;
        set
        {
            if (_application != null)
            {
                _application.MainWindow = value;
            }

            _mainWindow = value;
        }
    }

    /// <summary>
    /// Gets or sets the application's resources.
    /// </summary>
    public ResourceDictionary Resources
    {
        get
        {
            if (_resources is null)
            {
                _resources = new();

                try
                {
                    Wpf.Ui.Appearance.ApplicationAccentColorManager.ApplySystemAccent();
                    var themesDictionary = new Markup.ThemesDictionary();
                    var controlsDictionary = new Markup.ControlsDictionary();
                    _resources.MergedDictionaries.Add(themesDictionary);
                    _resources.MergedDictionaries.Add(controlsDictionary);
                }
                catch
                {
                }
            }

            return _application?.Resources ?? _resources;
        }

        set
        {
            if (_application is not null)
            {
                _application.Resources = value;
            }

            _resources = value;
        }
    }

    /// <summary>
    /// Gets or sets the application's main window.
    /// </summary>
    public object TryFindResource(object resourceKey)
    {
        return Resources[resourceKey];
    }

    /// <summary>
    /// Turns the application's into shutdown mode.
    /// </summary>
    public void Shutdown()
    {
        _application?.Shutdown();
    }
}
