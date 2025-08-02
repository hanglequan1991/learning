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

    public EMA(double[] src, int length)
    {
        _length = length;
        _emaLine = new double[src.Length];
        Array.Fill(_emaLine, double.NaN);
        if (src.Length < length)
        {
            _emaLine = [.. _emaLine[1..src.Length], src[0..^2].Average()];
            Next(src[^1]);
        }
        else
        {
            var firstValueIndex = 0;
            for (int i = 0; i < src.Length; i++)
            {
                if (!double.IsNaN(src[i]))
                {
                    firstValueIndex = i;
                    break;
                }
            }
            _emaLine = [.. _emaLine[1..src.Length], src[firstValueIndex..(_length + firstValueIndex)].Average()];
            for (int i = _length + firstValueIndex; i < src.Length; i++)
            {
                Next(src[i]);
            }
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
        var ema = _emaLine[^1];
        return price * k + ema * (1 - k);
    }

    public double Next(double price)
    {
        var ema = Moment(price);
        _emaLine = [.. _emaLine[1.._emaLine.Length], ema];
        return _emaLine[^1];
    }

    public double[] GetLines() => [.. _emaLine];
    public double FirstNotNaN => _emaLine.Where(_ => !double.IsNaN(_)).FirstOrDefault();
}
