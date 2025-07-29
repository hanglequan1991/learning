namespace Learning.Mathematic;

public class MIN : IMath
{
    private double[] _prices;

    public MIN(double[] prices, int length)
    {
        _prices = prices[^length..];
    }

    public double Moment(double price)
    {
        double[] next = [.. _prices[1..], price];
        return next.Min();
    }

    public double Next(double price)
    {
        _prices = [.. _prices[1..], price];
        return _prices.Min();
    }
}