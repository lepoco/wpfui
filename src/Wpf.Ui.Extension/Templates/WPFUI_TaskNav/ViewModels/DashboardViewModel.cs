using System;

namespace $safeprojectname$.ViewModels
{
    public class DashboardViewModel : Wpf.Ui.Mvvm.ViewModelBase
    {
        private bool _isInitialized = false;

        public string PrimaryButtonText
        {
            get => GetValueOrDefault(String.Empty);
            set => SetValue(value);
        }

        public DashboardViewModel()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            PrimaryButtonText = "Hello World";

            _isInitialized = true;
        }
    }
}
