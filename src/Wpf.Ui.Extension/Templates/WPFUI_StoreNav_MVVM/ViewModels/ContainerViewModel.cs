using System;

namespace $safeprojectname$.ViewModels
{
    public class ContainerViewModel : Wpf.Ui.Mvvm.ViewModelBase
    {
        private bool _isInitialized = false;

        public string ApplicationTitle
        {
            get => GetValueOrDefault(String.Empty);
            set => SetValue(value);
        }

        public ContainerViewModel()
        {
            if(!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "WPF UI - DemoApp3";

            _isInitialized = true;
        }
    }
}
