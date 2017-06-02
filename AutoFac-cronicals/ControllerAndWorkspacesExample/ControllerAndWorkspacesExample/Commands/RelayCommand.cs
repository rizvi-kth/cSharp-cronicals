namespace controllerAndWorkspacesExample.Commands
{
    using System;
    using System.Windows.Input;

    public sealed class RelayCommand<T> : IRelayCommand<T>
    {
        private readonly Action<T> _execute;
        private readonly Func<bool> _canExecute;
        
        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute((T)parameter);    
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove  { CommandManager.RequerySuggested -= value; }
        }
    }
}