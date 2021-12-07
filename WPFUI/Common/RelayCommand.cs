// ICommand implementation example provided by Microsoft.
// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.input.icommand?view=winrt-22000

using System;
using System.Windows.Input;

namespace WPFUI.Common
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The
    /// default return value for the <see cref="CanExecute"/> method is <see langword="true"/>.
    /// <para>
    /// <see href="https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.input.icommand?view=winrt-22000"/>
    /// </para>
    /// </summary>
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;

        readonly Func<bool> _canExecute;

        /// <summary>
        /// Event occuring when encapsulated canExecute method is changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Creates new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action to be executed.</param>
        public RelayCommand(Action execute) : this(execute, null)
        {
            // Delegated to RelayCommand(Action execute, Func<bool> canExecute)
        }

        /// <summary>
        /// Creates new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action with <see cref="object"/> parameter to be executed.</param>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
            // Delegated to RelayCommand(Action<object> execute, Func<bool> canExecute)
        }

        /// <summary>
        /// Creates new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action to be executed.</param>
        /// <param name="canExecute">Encapsulated method determining whether to execute action.</param>
        /// <exception cref="ArgumentNullException">Exception occurring when no <see cref="Action"/> is defined.</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = p => execute();
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action with <see cref="object"/> parameter to be executed.</param>
        /// <param name="canExecute">Encapsulated method determining whether to execute action.</param>
        /// <exception cref="ArgumentNullException">Exception occurring when no <see cref="Action"/> is defined.</exception>
        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        /// <summary>
        /// Gets encapsulated canExecute function.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        /// <summary>
        /// Triggers action with provided parameter.
        /// </summary>
        /// <param name="parameter">Argument to be passed.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}