using System;

namespace OneComic.Admin.Commands
{
    public sealed class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) == true;
        }

        public override void Execute(object parameter)
        {
            _execute.Invoke((T)parameter);
        }
    }
}
