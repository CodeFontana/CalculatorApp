using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCalculator.Services;
using WpfCalculator.ViewModels.Commands;

namespace WpfCalculator.ViewModels
{
    /// <summary>
    /// ViewModel for the CalculatorWindow View.
    /// </summary>
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Math service provided via the default constructor, courtesy of Dependency Injection.
        /// </summary>
        private readonly IMathService MathService;

        /// <summary>
        /// Lefthand value for math operations.
        /// </summary>
        private double _leftNumber = 0;
        
        /// <summary>
        /// Lefthand value for math operations.
        /// </summary>
        private double _rightNumber = 0;

        private MathOperation _SelectedOperation;
        /// <summary>
        /// The currently selected mathmatical operation.
        /// </summary>
        public MathOperation SelectedOperation
        {
            get { return _SelectedOperation; }
            set 
            {
                _SelectedOperation = value;
                OnPropertyChanged("SelectedOperation");
            }
        }

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
        /// The ButtonCommand implements ICommand and will provide event handling and routing
        /// whenever a button on the calculator is pressed.
        /// </summary>
        public ButtonCommand CalculatorCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Implementation of INotifyPropertyChanged, for notifying subscribers to property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Default constructor to initialize main display of the calculator and wire up
        /// event handling for this ViewModel.
        /// </summary>
        public CalculatorViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                ResultDisplay = "8008";
            }
            else
            {
                ResultDisplay = "0";
            }

            CalculatorCommand = new ButtonCommand(this);
        }

        /// <summary>
        /// Non-Default constructor provides dependency injection.
        /// </summary>
        public CalculatorViewModel(IMathService mathService)
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                ResultDisplay = "8008";
            }
            else
            {
                ResultDisplay = "0";
            }

            CalculatorCommand = new ButtonCommand(this);
            MathService = mathService;
        }

        /// <summary>
        /// Called by the ButtomCommand Execute() implementation and carries out the
        /// requested mathematical function, based on the button pressed on the View.
        /// </summary>
        /// <param name="value">String representation of the button press from the View</param>
        public void Calculate(string value)
        {
            switch (value)
            {
                case "AC":
                    _leftNumber = 0;
                    _rightNumber = 0;
                    SelectedOperation = MathOperation.Nothing;
                    ResultDisplay = _leftNumber.ToString();
                    break;
                case "+/-":
                    if (_leftNumber != 0)
                    {
                        _leftNumber *= -1;
                        ResultDisplay = _leftNumber.ToString();
                    }
                    break;
                case "%":
                    if (_leftNumber != 0)
                    {
                        _leftNumber = MathService.Percent(_leftNumber);
                        ResultDisplay = _leftNumber.ToString();
                    }
                    break;
                case "/":
                    SelectedOperation = MathOperation.Division;
                    break;
                case "x":
                    SelectedOperation = MathOperation.Multiplication;
                    break;
                case "-":
                    SelectedOperation = MathOperation.Subtraction;
                    break;
                case "+":
                    SelectedOperation = MathOperation.Addition;
                    break;
                case "=":
                    switch (SelectedOperation)
                    {
                        case MathOperation.Addition:
                            _leftNumber = MathService.Add(_leftNumber, _rightNumber);
                            ResultDisplay = _leftNumber.ToString();
                            break;
                        case MathOperation.Subtraction:
                            _leftNumber = MathService.Subtract(_leftNumber, _rightNumber);
                            ResultDisplay = _leftNumber.ToString();
                            break;
                        case MathOperation.Multiplication:
                            _leftNumber = MathService.Multiply(_leftNumber, _rightNumber);
                            ResultDisplay = _leftNumber.ToString();
                            break;
                        case MathOperation.Division:
                            if (_rightNumber == 0)
                            {
                                _leftNumber = 0;
                                ResultDisplay = "ERROR";
                            }
                            else
                            {
                                _leftNumber = MathService.Divide(_leftNumber, _rightNumber);
                                ResultDisplay = _leftNumber.ToString();
                            }
                            break;
                    }
                    _rightNumber = 0;
                    SelectedOperation = MathOperation.Nothing;
                    break;
                case ".":
                    ResultDisplay = $"{_leftNumber}.";
                    break;
                default:
                    if (int.TryParse(value, out int digit))
                    {
                        if (SelectedOperation == MathOperation.Nothing)
                        {
                            _leftNumber = double.Parse($"{_leftNumber}{digit}");
                            ResultDisplay = _leftNumber.ToString();
                        }
                        else
                        {
                            _rightNumber = double.Parse($"{_rightNumber}{digit}");
                            ResultDisplay = _rightNumber.ToString();
                        }
                    }
                    else
                    {
                        ResultDisplay = "ERROR";
                        SelectedOperation = MathOperation.Nothing;
                        _leftNumber = 0;
                        _rightNumber = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Enumeration of available math operations.
        /// </summary>
        public enum MathOperation
        {
            Nothing,
            Addition,
            Subtraction,
            Multiplication,
            Division
        }
    }
}
