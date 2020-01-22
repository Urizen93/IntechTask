using IntechTask.DesktopClient.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntechTask.DesktopClient.Misc
{
    public sealed class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        private bool _isExecuting;

        public AsyncCommand(
            [NotNull] Func<Task> execute,
            Func<bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute ?? (() => true);
            _errorHandler = errorHandler;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute() => !_isExecuting && _canExecute();

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #region Explicit implementations

        bool ICommand.CanExecute(object _) => CanExecute();

        void ICommand.Execute(object _) => ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);

        #endregion
    }
}