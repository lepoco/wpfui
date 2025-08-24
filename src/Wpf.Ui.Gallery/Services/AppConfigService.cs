// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Text.Json;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.Services;

/// <summary>
/// Application configuration service.
/// </summary>
public class AppConfigService
{
    private readonly string _configFilePath;
    private readonly AppConfig _config;

    public AppConfigService()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataPath, "Wpf.Ui.Gallery");

        if (!Directory.Exists(appFolder))
        {
            _ = Directory.CreateDirectory(appFolder);
        }

        _configFilePath = Path.Combine(appFolder, "config.json");
        _config = LoadConfig();
    }

    public AppConfig Config => _config;

    private AppConfig LoadConfig()
    {
        if (File.Exists(_configFilePath))
        {
            try
            {
                string json = File.ReadAllText(_configFilePath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading config: {ex.Message}");
                return new AppConfig();
            }
        }
        return new AppConfig();
    }

    private void SaveConfig(AppConfig config)
    {
        try
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFilePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving config: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates the navigation pane width and saves the configuration
    /// </summary>
    /// <param name="newWidth">The new width value</param>
    public void UpdateNavigationPaneWidth(double newWidth)
    {
        _config.NavigationPaneWidth = newWidth;
        SaveConfig(_config);
    }
}
