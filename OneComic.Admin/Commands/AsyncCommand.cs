﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OneComic.Admin.Commands
{
    public sealed class AsyncCommand : DelegateCommandBase, IAsyncCommand, INotifyPropertyChanged
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyTask Execution { get; private set; }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute ?? (() => true);
        }

        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && _canExecute?.Invoke() == true;
        }

        public async override void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            var tcs = new TaskCompletionSource<object>();
            Execution = NotifyTask.Create(DoExecuteAsync(tcs.Task, _execute));

            RaiseCanExecuteChanged();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Execution)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExecuting)));

            tcs.SetResult(null);
            await Execution.TaskCompleted;

            RaiseCanExecuteChanged();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExecuting)));
            await Execution.Task;
        }

        public bool IsExecuting => Execution?.IsNotCompleted == true;

        private static async Task DoExecuteAsync(Task precondition, Func<Task> executeAsync)
        {
            await precondition;
            await executeAsync();
        }

    }
}
