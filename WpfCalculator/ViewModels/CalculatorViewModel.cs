using System.Collections.Generic;
using WpfCalculator.Services;
using WpfCalculator.ViewModels.Commands;

namespace WpfCalculator.ViewModels
{
    /// <summary>
    /// ViewModel for the CalculatorWindow View.
    /// </summary>
    public class CalculatorViewModel : ObservableObject
    {
        /// <summary>
        /// Math service provided via the default constructor, courtesy of Dependency Injection.
        /// </summary>
        private readonly IMathService MathService;

        /// <summary>
        /// List of operands.
        /// </summary>
        private List<string> _operands;

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

        /// <summary>
        /// Default constructor, only used in Design-Time mode, to display a sample value
        /// on the calculator screen.
        /// </summary>
        public CalculatorViewModel()
        {
            CalculatorCommand = new ButtonCommand(this);
            _operands = new List<string>();
            _operands.Add("8008");
            ResultDisplay = _operands[^1];
        }

        /// <summary>
        /// Runtime constructor for the ViewModel.
        /// </summary>
        /// <param name="mathService">Service that provides mathmatical functions.</param>
        public CalculatorViewModel(IMathService mathService)
        {
            MathService = mathService;
            CalculatorCommand = new ButtonCommand(this);
            _operands = new List<string>();
            _operands.Add("0");
            ResultDisplay = _operands[^1];
        }

        /// <summary>
        /// Handles button presses from the View.
        /// </summary>
        /// <param name="buttonName">Name of button pushed on the View.</param>
        public void ButtonPress(string buttonName)
        {
            switch (buttonName)
            {
                case "AC":
                    _operands = new List<string>();
                    _operands.Add("0");
                    SelectedOperation = MathOperation.None;
                    ResultDisplay = "0";
                    break;
                case "+/-":
                    _operands[^1] = (double.Parse(ResultDisplay) * -1).ToString();
                    ResultDisplay = _operands[^1].ToString();
                    break;
                case "%":
                    _operands[^1] = (double.Parse(ResultDisplay) / 100).ToString();
                    ResultDisplay = _operands[^1].ToString();
                    break;
                case "/":
                    SelectedOperation = MathOperation.Division;
                    _operands[^1] = (double.Parse(ResultDisplay)).ToString();
                    _operands.Add("");
                    break;
                case "x":
                    SelectedOperation = MathOperation.Multiplication;
                    _operands[^1] = (double.Parse(ResultDisplay)).ToString();
                    _operands.Add("");
                    break;
                case "-":
                    SelectedOperation = MathOperation.Subtraction;
                    _operands[^1] = (double.Parse(ResultDisplay)).ToString();
                    _operands.Add("");
                    break;
                case "+":
                    SelectedOperation = MathOperation.Addition;
                    _operands[^1] = (double.Parse(ResultDisplay)).ToString();
                    _operands.Add("");
                    break;
                case "=":
                    switch (SelectedOperation)
                    {
                        case MathOperation.Addition:
                            if (_operands.Count == 2 && string.IsNullOrWhiteSpace(_operands[1]))
                            {
                                _operands[0] = (double.Parse(_operands[0]) + double.Parse(_operands[0])).ToString();
                                SelectedOperation = MathOperation.Addition;
                                ResultDisplay = _operands[0];
                            }
                            else
                            {
                                _operands[0] = (double.Parse(_operands[0]) + double.Parse(_operands[1])).ToString();
                                _operands.RemoveAt(1);
                                SelectedOperation = MathOperation.None;
                                ResultDisplay = _operands[0];
                            }
                            break;
                        case MathOperation.Subtraction:
                            if (_operands.Count == 2 && string.IsNullOrWhiteSpace(_operands[1]))
                            {
                                _operands[0] = (double.Parse(_operands[0]) - double.Parse(_operands[0])).ToString();
                                SelectedOperation = MathOperation.Subtraction;
                                ResultDisplay = _operands[0];
                            }
                            else
                            {
                                _operands[0] = (double.Parse(_operands[0]) - double.Parse(_operands[1])).ToString();
                                _operands.RemoveAt(1);
                                SelectedOperation = MathOperation.None;
                                ResultDisplay = _operands[0];
                            }
                            break;
                        case MathOperation.Multiplication:
                            if (_operands.Count == 2 && string.IsNullOrWhiteSpace(_operands[1]))
                            {
                                _operands[0] = (double.Parse(_operands[0]) * double.Parse(_operands[0])).ToString();
                                SelectedOperation = MathOperation.Multiplication;
                                ResultDisplay = _operands[0];
                            }
                            else
                            {
                                _operands[0] = (double.Parse(_operands[0]) * double.Parse(_operands[1])).ToString();
                                _operands.RemoveAt(1);
                                SelectedOperation = MathOperation.None;
                                ResultDisplay = _operands[0];
                            }
                            break;
                        case MathOperation.Division:
                            if (_operands.Count == 2 && string.IsNullOrWhiteSpace(_operands[1]))
                            {
                                _operands[0] = (double.Parse(_operands[0]) / double.Parse(_operands[0])).ToString();
                                SelectedOperation = MathOperation.Division;
                                ResultDisplay = _operands[0];
                            }
                            else if (_operands.Count == 2 && double.Parse(_operands[1]) == 0)
                            {
                                _operands = new List<string>();
                                _operands.Add("0");
                                SelectedOperation = MathOperation.None;
                                ResultDisplay = "ERROR";
                            }
                            else
                            {
                                _operands[0] = (double.Parse(_operands[0]) / double.Parse(_operands[1])).ToString();
                                _operands.RemoveAt(1);
                                SelectedOperation = MathOperation.None;
                                ResultDisplay = _operands[0];
                            }
                            break;
                    }
                    break;
                case ".":
                    if (ResultDisplay.Contains(".") == false)
                    {
                        ResultDisplay = $"{ResultDisplay}.";
                    }
                    break;
                default:
                    if (_operands[^1].Equals(""))
                    {
                        ResultDisplay = double.Parse($"{buttonName}").ToString();
                        _operands[^1] = ResultDisplay;
                    }
                    else
                    {
                        ResultDisplay = double.Parse($"{ResultDisplay}{buttonName}").ToString();
                        _operands[^1] = ResultDisplay;
                    }
                    break;
            }
        }

        /// <summary>
        /// Enumeration of available math operations.
        /// </summary>
        public enum MathOperation
        {
            None,
            Addition,
            Subtraction,
            Multiplication,
            Division
        }
    }
}
