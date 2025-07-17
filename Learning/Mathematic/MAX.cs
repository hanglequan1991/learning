using System.Linq;
namespace Learning.Mathematic;

public class MAX
{
    private double[] _prices;

    public MAX(double[] prices, int length)
    {
        _prices = prices[^length..];
    }

    public double Moment(double price)
    {
        double[] next = [.. _prices[1..], price];
        return next.Max();
    }

    public double Next(double price)
    {
        _prices = [.. _prices[1..], price];
        return _prices.Max();
    }
}