using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfCalculator.ViewModels.Commands
{
    public class ButtonCommand : ICommand
    {
        public CalculatorViewModel CalculatorVM { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ButtonCommand(CalculatorViewModel calculatorViewModel)
        {
            CalculatorVM = calculatorViewModel;
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
            CalculatorVM.PerformOperation(buttonCmd);
        }
    }
}
