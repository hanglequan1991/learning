namespace Learning.Mathematic;

/// <summary>
/// https://www.investopedia.com/ask/answers/122314/what-exponential-moving-average-ema-formula-and-how-ema-calculated.asp
/// </summary>
public class EMA
{
    int _length;

    double _ema;

    double _sma;

    /// <summary>
    /// Weighted Multiplier = 2 / (selected time period + 1)
    /// </summary>
    double k => 2.0d / (_length + 1.0d);

    public EMA(double[] src, int length)
    {
        _length = length;
        _ema = src[0.._length].Average();

        for (int i = _length; i < src.Length; i++)
        {
            _ema = Next(src[i]);
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
        return price * k + _ema * (1 - k);
    }

    public double Next(double price)
    {
        _ema = Moment(price);
        return _ema;
    }
}
