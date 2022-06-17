using System;
using System.Windows;

namespace $safeprojectname$.ViewModels
{
    public class SettingsViewModel : Wpf.Ui.Mvvm.ViewModelBase
    {
        private bool _isInitialized = false;

        public string AppVersion
        {
            get => GetValueOrDefault(String.Empty);
            set => SetValue(value);
        }

        public Wpf.Ui.Appearance.ThemeType CurrentTheme
        {
            get => GetStructOrDefault(Wpf.Ui.Appearance.ThemeType.Unknown);
            set => SetValue(value);
        }

        public SettingsViewModel()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
            AppVersion = $"$safeprojectname$ - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
        }
    }
}
