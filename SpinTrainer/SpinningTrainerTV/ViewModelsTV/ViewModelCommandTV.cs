using System.Windows.Input;

namespace SpinningTrainerTV.ViewModelsTV
{
    public class ViewModelCommandTV : ICommand
    {
        // Archivos
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecuteAction;
        private ICommand? modifySessionExerciseCommand;

        public ViewModelCommandTV(ICommand? modifySessionExerciseCommand)
        {
            this.modifySessionExerciseCommand = modifySessionExerciseCommand;
        }

        public ViewModelCommandTV(Action<object> executeAction, Predicate<object> canExecuteAction = null)
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
