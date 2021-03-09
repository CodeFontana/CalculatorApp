using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCalculator.ViewModels.Commands;

namespace WpfCalculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _resultDisplay;
        /// <summary>
        /// The main display of the calculator.
        /// </summary>
        public string ResultDisplay
        {
            get { return _resultDisplay; }
            set 
            { 
                _resultDisplay = value;
                OnPropertyChanged("ResultDisplay");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonCommand CalculatorCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Implementation of INotifyPropertyChanged, for notifying subscribers to property changes.
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Default constructor to initialize main display of the calculator.
        /// </summary>
        public CalculatorViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                _resultDisplay = "8008";
            }
            else
            {
                _resultDisplay = "0";
            }

            CalculatorCommand = new ButtonCommand(this);
        }

        public void PerformOperation(string value)
        {
            ResultDisplay = value;
        }
    }
}
