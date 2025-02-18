using System;
using System.Windows.Input;

namespace SpinningTrainer.ViewModels
{
    public class ViewModelCommand : ICommand
    {
        // Archivos
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecuteAction;
        private ICommand? modifySessionExerciseCommand;

        public ViewModelCommand(ICommand? modifySessionExerciseCommand)
        {
            this.modifySessionExerciseCommand = modifySessionExerciseCommand;
        }

        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction = null)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecuteAction = canExecuteAction;
        }

        // Implementación de ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null || _canExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }

        // Método para invocar CanExecuteChanged manualmente
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
