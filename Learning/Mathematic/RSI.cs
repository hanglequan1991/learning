namespace Learning.Mathematic;

public class RSI : IMath
{
    private double _rsi;
    private double[] _prices;
    private double _avgGain;
    private double _avgLoss;

    private int _length = 14; // default periods

    public RSI(double[] prices, int length = 14)
    {
        _length = length;
        _rsi = new();
        _prices = prices[..^length];
        var currPrices = prices[..^length];
        var totalGains = 0d;
        var totalLosses = 0d;

        for (var i = 1; i < length; i++)
        {
            var prevPrice = currPrices[i - 1];
            var currPrice = currPrices[i];

            var change = currPrice - prevPrice;
            totalGains += change > 0 ? change : 0;
            totalLosses += change < 0 ? Math.Abs(change) : 0;
        }

        _avgGain = totalGains / _length;
        _avgLoss = totalLosses / _length;

        _rsi = 100 - (100 / (1 + (_avgGain / _avgLoss)));
    }

    private (double AvgGain, double AvgLoss) CalculateChange(int length)
    {
        var totalGains = 0d;
        var totalLosses = 0d;
        for (var i = 1; i < length; i++)
        {
            var prevPrice = _prices[i - 1];
            var currPrice = _prices[i];

            var change = currPrice - prevPrice;
            totalGains += change > 0 ? change : 0;
            totalLosses += change < 0 ? Math.Abs(change) : 0;
        }
        return (totalGains / length, totalLosses / length);
    }

    public double Next(double price)
    {
        _rsi = Moment(price);
        _prices = [.. _prices[1.._length], price];
        (_avgGain, _avgLoss) = CalculateChange(_length);

        return _rsi;
    }

    public double Moment(double price)
    {
        var change = price - _prices[^1];
        var gain = change > 0 ? change : 0;
        var loss = change < 0 ? Math.Abs(change) : 0;
        return 100 - (100 / (1 + (_avgGain * (_length - 1) + gain) / (_avgLoss * (_length - 1) + loss)));
    }

    public override string ToString()
    {
        return $"{nameof(RSI)}: {_rsi}";
    }
}
