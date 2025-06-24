namespace Learning.Mathematic;

public class MACD
{
    private EMA _longTermEma;
    private EMA _shortTermEma;

    public MACD(double[] source, int shortTermPeriod, int longTermPeriod)
    {
        _shortTermEma = new EMA(source, shortTermPeriod);
        _longTermEma = new EMA(source, longTermPeriod);
    }

    public double Next(double price)
    {
        return _shortTermEma.Next(price) - _longTermEma.Next(price);
    }

    public double Moment(double price)
    {
        return _shortTermEma.Moment(price) - _longTermEma.Moment(price);
    }
}