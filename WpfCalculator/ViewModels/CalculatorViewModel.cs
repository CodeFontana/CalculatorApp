using System;
using System.Globalization;
using WpfCalculator.ViewModels.Commands;

namespace WpfCalculator.ViewModels;

public class CalculatorViewModel : ViewModelBase
{
    private const string ErrorDisplay = "ERROR";
    private const string NumericFormat = "G15";
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
            OnPropertyChanged();
        }
    }

    private string _resultDisplay = "0";
    public string ResultDisplay
    {
        get => _resultDisplay;
        set
        {
            _resultDisplay = value;
            OnPropertyChanged();
        }
    }

    public ButtonCommand CalculatorCommand { get; }

    public CalculatorViewModel()
    {
        CalculatorCommand = new ButtonCommand(this);
        ClearAll();
    }

    public void ButtonPress(string buttonName)
    {
        if (string.IsNullOrWhiteSpace(buttonName))
        {
            return;
        }

        // If currently in an error state, any input except AC resets first.
        if (ResultDisplay == ErrorDisplay && buttonName != "AC")
        {
            ClearAll();
        }

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
        {
            return;
        }

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
        {
            ResultDisplay += ".";
        }
    }

    private void ToggleSign()
    {
        if (!TryParseDisplay(out decimal value))
        {
            return;
        }

        // Toggling the sign is part of the current entry: from this point on,
        // subsequent digits should append to (not replace) the displayed value.
        _resetEntryOnNextDigit = false;

        if (value == 0m)
        {
            // Special-case: flip the visible sign so the user can begin typing a negative number.
            ResultDisplay = ResultDisplay.StartsWith('-') ? "0" : "-0";
            _isEntering = true;
            return;
        }

        value = -value;
        ResultDisplay = Format(value);
        _isEntering = true;
    }

    private void Percent()
    {
        if (!TryParseDisplay(out decimal value))
        {
            return;
        }

        // Common calculator behavior: percent transforms the current entry into a percentage of the left
        // value if an op is pending; otherwise it's just value/100.
        if (SelectedOperation != MathOperation.None && _leftValue.HasValue)
        {
            value = _leftValue.Value * (value / 100m);
        }
        else
        {
            value /= 100m;
        }

        ResultDisplay = Format(value);
        _isEntering = true;
        _resetEntryOnNextDigit = false;
    }

    private void OperatorPressed(MathOperation op)
    {
        if (!TryParseDisplay(out decimal entryValue))
        {
            return;
        }

        if (_leftValue is null)
        {
            _leftValue = entryValue;
        }
        else if (SelectedOperation != MathOperation.None && !_resetEntryOnNextDigit)
        {
            // chaining: if we have left + pending op + current entry, evaluate immediately
            _rightValue = entryValue;

            if (!TryCompute(_leftValue.Value, _rightValue.Value, SelectedOperation, out decimal computed))
            {
                Error();
                return;
            }

            _leftValue = computed;
            ResultDisplay = Format(computed);
        }

        SelectedOperation = op;

        _rightValue = null;
        _resetEntryOnNextDigit = true;
        _isEntering = false;

        // When an operator is pressed, repeat-equals context should be updated to the operator (but no RHS yet).
        _lastOperation = MathOperation.None;
        _lastRightValue = null;
    }

    private void EqualsPressed()
    {
        if (SelectedOperation == MathOperation.None)
        {
            return;
        }

        if (_leftValue is null)
        {
            if (!TryParseDisplay(out decimal parsedLeft))
            {
                return;
            }

            _leftValue = parsedLeft;
        }

        // Determine RHS:
        // - If user entered a RHS, use it.
        // - Else if we have a last RHS from prior '=', repeat it.
        // - Else if RHS missing, use left as RHS (matches common "+ =" doubling, "* =" squaring, "/ =" -> 1).
        decimal rhs;
        if (!_resetEntryOnNextDigit && TryParseDisplay(out decimal currentEntry))
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

        if (!TryCompute(_leftValue.Value, rhs, SelectedOperation, out decimal result))
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
        ResultDisplay = ErrorDisplay;
        _leftValue = null;
        _rightValue = null;
        SelectedOperation = MathOperation.None;
        _resetEntryOnNextDigit = true;
        _isEntering = false;
    }

    private static bool TryCompute(decimal left, decimal right, MathOperation op, out decimal result)
    {
        result = 0m;

        try
        {
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
                    {
                        return false;
                    }
                    result = left / right;
                    return true;

                default:
                    result = left;
                    return true;
            }
        }
        catch (OverflowException)
        {
            // decimal.MaxValue exceeded - surface as calculator error rather than crashing.
            return false;
        }
    }

    private bool TryParseDisplay(out decimal value)
    {
        // Display may contain a lone trailing '-' or '-0' sentinel during sign entry - treat as zero.
        string raw = ResultDisplay;
        if (raw is "-" or "-0")
        {
            value = 0m;
            return true;
        }

        return decimal.TryParse(raw, NumberStyles.Number, s_culture, out value);
    }

    private static string Format(decimal value)
    {
        // G15 gives a sensible default precision without the long trailing strings that the default
        // decimal.ToString can produce (e.g. 1 / 3) and collapses trailing zeros from scale-preserving
        // operations (e.g. 3.2 + 4.8 -> "8" instead of "8.0").
        return value.ToString(NumericFormat, s_culture);
    }
}
