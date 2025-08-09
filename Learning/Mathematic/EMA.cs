namespace Learning.Mathematic;

/// <summary>
/// https://www.investopedia.com/ask/answers/122314/what-exponential-moving-average-ema-formula-and-how-ema-calculated.asp
/// </summary>
public class EMA : IMath
{
    int _length;

    double[] _emaLine;

    /// <summary>
    /// Weighted Multiplier = 2 / (selected time period + 1)
    /// </summary>
    double k => 2.0d / (_length + 1.0d);

    public EMA(double[] prices, int length)
    {
        _length = length;
        _emaLine = new double[prices.Length];
        Array.Fill(_emaLine, double.NaN);
        foreach (var price in prices)
        {
            Next(price);
        }
    }

    /// <summary>
    /// EMA = Price(t) * k + EMA(y) * (1-k)
    /// t = today
    /// y = yesterday
    /// N = number of days in EMA (length)
    /// k = 2 / (N + 1)
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public double Moment(double price)
    {
        var lastArray = _emaLine[^(_length - 1)..];
        if (lastArray.Any(_ => double.IsNaN(_)))
        {
            return double.NaN;
        }

        if (_emaLine.Count(_ => !double.IsNaN(_)) == _length - 1)
        {
            double[] averageArray = [.. lastArray, price];
            return averageArray.Average();
        }
        var ema = _emaLine[^1];
        return price * k + ema * (1 - k);
    }

    public double Next(double price)
    {
        var ema = Moment(price);
        _emaLine = [.. _emaLine[1.._emaLine.Length], double.IsNaN(ema) ? price : ema];
        return ema;
    }

    public double[] GetLines() => [.. _emaLine];
}
