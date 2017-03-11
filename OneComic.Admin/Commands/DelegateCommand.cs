using System;

namespace OneComic.Admin.Commands
{
    public sealed class DelegateCommand : DelegateCommandBase
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute ?? (() => true);
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() == true;
        }

        public override void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
