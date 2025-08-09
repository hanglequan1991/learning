using System.Linq;
namespace Learning.Mathematic;

public class MAX : IMath
{
    private double[] _prices;

    public MAX(double[] prices, int length)
    {
        if (prices.Length > length)
        {
            _prices = prices[^length..];
        }
        else
        {
            _prices = [.. prices];
        }
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