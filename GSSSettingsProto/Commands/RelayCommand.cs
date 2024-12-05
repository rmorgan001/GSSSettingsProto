using System.Windows.Input;

namespace GSSSettingsProto.Commands
{
    //using System.Windows.Input;
    public class RelayCommand(Action<object> executeAction) : ICommand
    {
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter)
        {
            if (parameter != null) executeAction(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
