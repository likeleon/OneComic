using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace OneComic.Admin.Commands
{
    [Export(typeof(ICommandFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class CommandFactory : ICommandFactory
    {
        public ICommand Create(Action execute)
        {
            return new DelegateCommand(execute, null);
        }

        public ICommand Create(Action execute, Func<bool> canExecute)
        {
            return new DelegateCommand(execute, canExecute);
        }

        public ICommand Create<T>(Action<T> execute)
        {
            return new DelegateCommand<T>(execute, null);
        }

        public ICommand Create<T>(Action<T> execute, Func<T, bool> canExecute)
        {
            return new DelegateCommand<T>(execute, canExecute);
        }
    }
}
