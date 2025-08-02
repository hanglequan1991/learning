namespace Learning.Mathematic;

public class MACD
{
    private int _slowPeriod;
    private int _fastPeriod;
    private int _signalPeriod;
    private EMA _slowEMA;
    private EMA _fastEMA;
    private EMA _signalLine;
    private double[] _source;

    public MACD(double[] source, int fastPeriod, int slowPeriod, int signalPeriod)
    {
        _source = source;
        _fastPeriod = fastPeriod;
        _slowPeriod = slowPeriod;
        _signalPeriod = signalPeriod;

        var max = new int[3] { _fastPeriod, _slowPeriod, _signalPeriod }.Max();

        _fastEMA = new EMA(source, fastPeriod);
        _slowEMA = new EMA(source, slowPeriod);

        var macdLines = new List<double>();
        for (int i = 0; i < _source.Length; i++)
        {
            macdLines.Add(_fastEMA.GetLines()[i] - _slowEMA.GetLines()[i]);
        }

        _signalLine = new EMA(macdLines.ToArray(), signalPeriod);
    }

    public (double signal, double macd, double histogram) Next(double price)
    {
        var fastEMA = _fastEMA.Next(price);
        var slowEMA = _slowEMA.Next(price);
        var macd = fastEMA - slowEMA;
        var signalLine = _signalLine.Next(macd);
        return (signalLine, macd, macd - signalLine);
    }

    public (double signal, double macd, double histogram) Moment(double price)
    {
        var fastEMA = _fastEMA.Moment(price);
        var slowEMA = _slowEMA.Moment(price);
        var macd = fastEMA - slowEMA;
        var signalLine = _signalLine.Moment(macd);
        return (signalLine, macd, macd - signalLine);
    }
}