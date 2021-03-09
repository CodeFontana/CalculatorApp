namespace WpfCalculator.Services
{
    public interface IMathService
    {
        double Add(double leftHand, double rightHand);
        double Divide(double leftHand, double rightHand);
        double Multiply(double leftHand, double rightHand);
        double Percent(double value);
        double Subtract(double leftHand, double rightHand);
    }
}