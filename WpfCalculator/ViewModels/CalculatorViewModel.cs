using System.Collections.Generic;
using WpfCalculator.ViewModels.Commands;

namespace WpfCalculator.ViewModels;

public class CalculatorViewModel : ViewModelBase
{
    private List<string> _operands;

    private MathOperation _selectedOperation;
    public MathOperation SelectedOperation
    {
        get { return _selectedOperation; }
        set
        {
            _selectedOperation = value;
            OnPropertyChanged(nameof(SelectedOperation));
        }
    }

    private string _resultDisplay;
    public string ResultDisplay
    {
        get { return _resultDisplay; }
        set
        {
            _resultDisplay = value;
            OnPropertyChanged(nameof(ResultDisplay));
        }
    }

    public ButtonCommand CalculatorCommand { get; set; }

    public CalculatorViewModel()
    {
        CalculatorCommand = new ButtonCommand(this);
        _operands = ["0"];
        ResultDisplay = _operands[^1];
    }

    public void ButtonPress(string buttonName)
    {
        switch (buttonName)
        {
            case "AC":
                _operands = ["0"];
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
                            _operands = ["0"];
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
                if (ResultDisplay.Contains('.') == false)
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

    public enum MathOperation
    {
        None,
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
}
