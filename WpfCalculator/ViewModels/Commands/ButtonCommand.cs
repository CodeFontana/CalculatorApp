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

        public bool CanExecute(object parameter)
        {
            string buttonCmd = parameter as string;

            if (string.IsNullOrWhiteSpace(buttonCmd))
            {
                return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            string buttonCmd = parameter as string;
            _calculatorViewModel.ButtonPress(buttonCmd);
        }
    }
}
