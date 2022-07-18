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

        protected override void OnViewCommand(object? parameter = null)
        {
            base.OnViewCommand(parameter);

            if (parameter == null || parameter is not String parameterString)
                return;

            switch (parameterString)
            {
                case "theme_light":
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;

                    break;

                case "theme_dark":
                    if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
                    CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;

                    break;
            }
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
