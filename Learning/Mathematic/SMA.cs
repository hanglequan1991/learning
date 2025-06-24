namespace Learning.Mathematic;

/// <summary>
/// https://www.investopedia.com/terms/s/sma.asp
/// </summary>
public class SMA
{
    double[] _prices;

    public SMA(double[] prices, int length)
    {
        _prices = prices[^length..];
    }

    public double Moment(double price)
    {
        double[] prices = [.. _prices[1..], price];
        return prices.Average();
    }

    public double Next(double price)
    {
        _prices = [.. _prices[1..], price];
        return _prices.Average();
    }
}
