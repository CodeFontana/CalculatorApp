using System;
using System.Windows.Input;

namespace WpfCalculator.ViewModels.Commands
{
    public class ButtonCommand : ICommand
    {
        private readonly CalculatorViewModel _calculatorViewModel;

        public ButtonCommand(CalculatorViewModel calculatorViewModel)
        {
            _calculatorViewModel = calculatorViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => parameter is string s && !string.IsNullOrWhiteSpace(s);

        public void Execute(object parameter)
        {
            if (parameter is not string buttonCmd || string.IsNullOrWhiteSpace(buttonCmd))
                return;

            _calculatorViewModel.ButtonPress(buttonCmd);
        }
    }
}
