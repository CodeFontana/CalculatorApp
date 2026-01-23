using System;
using System.Globalization;
using WpfCalculator.ViewModels.Commands;

namespace WpfCalculator.ViewModels;

public class CalculatorViewModel : ViewModelBase
{
    private static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    private decimal? _leftValue;
    private decimal? _rightValue;
    private decimal? _lastRightValue; // for repeat '='
    private MathOperation _lastOperation = MathOperation.None;
    private bool _isEntering; // typing digits into current entry
    private bool _resetEntryOnNextDigit; // set after operator / equals / error

    private MathOperation _selectedOperation;
    public MathOperation SelectedOperation
    {
        get => _selectedOperation;
        set
        {
            _selectedOperation = value;
            OnPropertyChanged(nameof(SelectedOperation));
        }
    }

    private string _resultDisplay = "0";
    public string ResultDisplay
    {
        get => _resultDisplay;
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
        ClearAll();
    }

    public void ButtonPress(string buttonName)
    {
        if (string.IsNullOrWhiteSpace(buttonName))
            return;

        // If currently in an error state, any input except AC resets first.
        if (ResultDisplay == "ERROR" && buttonName != "AC")
            ClearAll();

        switch (buttonName)
        {
            case "AC":
                ClearAll();
                break;

            case "+/-":
                ToggleSign();
                break;

            case "%":
                Percent();
                break;

            case ".":
                EnterDecimalPoint();
                break;

            case "/":
                OperatorPressed(MathOperation.Division);
                break;

            case "x":
                OperatorPressed(MathOperation.Multiplication);
                break;

            case "-":
                OperatorPressed(MathOperation.Subtraction);
                break;

            case "+":
                OperatorPressed(MathOperation.Addition);
                break;

            case "=":
                EqualsPressed();
                break;

            default:
                DigitPressed(buttonName);
                break;
        }
    }

    private void ClearAll()
    {
        _leftValue = null;
        _rightValue = null;
        _lastRightValue = null;
        _lastOperation = MathOperation.None;

        SelectedOperation = MathOperation.None;

        ResultDisplay = "0";
        _isEntering = false;
        _resetEntryOnNextDigit = true;
    }

    private void DigitPressed(string token)
    {
        if (token.Length != 1 || token[0] < '0' || token[0] > '9')
            return;

        if (_resetEntryOnNextDigit)
        {
            ResultDisplay = "0";
            _resetEntryOnNextDigit = false;
            _isEntering = false;
        }

        if (!_isEntering || ResultDisplay == "0")
        {
            ResultDisplay = token;
            _isEntering = true;
            return;
        }

        ResultDisplay += token;
    }

    private void EnterDecimalPoint()
    {
        if (_resetEntryOnNextDigit)
        {
            ResultDisplay = "0";
            _resetEntryOnNextDigit = false;
            _isEntering = false;
        }

        if (!_isEntering)
        {
            ResultDisplay = "0.";
            _isEntering = true;
            return;
        }

        if (!ResultDisplay.Contains('.', StringComparison.Ordinal))
            ResultDisplay += ".";
    }

    private void ToggleSign()
    {
        if (!TryParseDisplay(out var value))
            return;

        value = -value;
        ResultDisplay = Format(value);
        _isEntering = true;
    }

    private void Percent()
    {
        if (!TryParseDisplay(out var value))
            return;

        // Common calculator behavior: percent transforms the current entry into a percentage of left value if an op is pending;
        // otherwise it's just value/100.
        if (SelectedOperation != MathOperation.None && _leftValue.HasValue)
            value = _leftValue.Value * (value / 100m);
        else
            value /= 100m;

        ResultDisplay = Format(value);
        _isEntering = true;
    }

    private void OperatorPressed(MathOperation op)
    {
        // Commit current entry into left/right depending on state.
        if (!TryParseDisplay(out var entryValue))
            return;

        if (_leftValue is null)
        {
            _leftValue = entryValue;
        }
        else if (SelectedOperation != MathOperation.None && !_resetEntryOnNextDigit)
        {
            // chaining: if we have left + pending op + current entry, evaluate immediately
            _rightValue = entryValue;

            if (!TryCompute(_leftValue.Value, _rightValue.Value, SelectedOperation, out var computed))
            {
                Error();
                return;
            }

            _leftValue = computed;
            ResultDisplay = Format(computed);
        }

        SelectedOperation = op;

        // start a new entry for the RHS
        _rightValue = null;
        _resetEntryOnNextDigit = true;
        _isEntering = false;

        // When an operator is pressed, repeat-equals context should be updated to the operator (but no RHS yet)
        _lastOperation = MathOperation.None;
        _lastRightValue = null;
    }

    private void EqualsPressed()
    {
        if (SelectedOperation == MathOperation.None)
        {
            // repeat '=' without pending op: no-op
            return;
        }

        if (_leftValue is null)
        {
            if (!TryParseDisplay(out var parsedLeft))
                return;

            _leftValue = parsedLeft;
        }

        // Determine RHS:
        // - If user entered a RHS, use it.
        // - Else if we have a last RHS from prior '=', repeat it.
        // - Else if RHS missing, use left as RHS (matches common "+ =" doubling behavior, "* =" squaring, "/ =" -> 1 (except 0)).
        decimal rhs;
        if (!_resetEntryOnNextDigit && TryParseDisplay(out var currentEntry))
        {
            rhs = currentEntry;
            _lastRightValue = rhs;
            _lastOperation = SelectedOperation;
        }
        else if (_lastOperation == SelectedOperation && _lastRightValue.HasValue)
        {
            rhs = _lastRightValue.Value;
        }
        else
        {
            rhs = _leftValue.Value;
            _lastRightValue = rhs;
            _lastOperation = SelectedOperation;
        }

        if (!TryCompute(_leftValue.Value, rhs, SelectedOperation, out var result))
        {
            Error();
            return;
        }

        _leftValue = result;
        _rightValue = null;

        ResultDisplay = Format(result);

        _resetEntryOnNextDigit = true;
        _isEntering = false;
    }

    private void Error()
    {
        ResultDisplay = "ERROR";
        _leftValue = null;
        _rightValue = null;
        SelectedOperation = MathOperation.None;
        _resetEntryOnNextDigit = true;
        _isEntering = false;
    }

    private static bool TryCompute(decimal left, decimal right, MathOperation op, out decimal result)
    {
        result = 0m;

        switch (op)
        {
            case MathOperation.Addition:
                result = left + right;
                return true;

            case MathOperation.Subtraction:
                result = left - right;
                return true;

            case MathOperation.Multiplication:
                result = left * right;
                return true;

            case MathOperation.Division:
                if (right == 0m)
                    return false;
                result = left / right;
                return true;

            default:
                result = left;
                return true;
        }
    }

    private bool TryParseDisplay(out decimal value)
        => decimal.TryParse(ResultDisplay, NumberStyles.Number, s_culture, out value);

    private static string Format(decimal value)
        => value.ToString(s_culture);

    public enum MathOperation
    {
        None,
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
}
