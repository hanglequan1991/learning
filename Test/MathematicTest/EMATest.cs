using Learning.Mathematic;

namespace Test.MathematicTest;

[TestClass]
public class EMATest
{
    [TestMethod]
    public void EMA_1_Value()
    {
        var ema = new EMA([1], 10);
        var a = ema.Moment(1);
        var b = ema.Next(1);
        Assert.AreEqual(1, a);
        Assert.AreEqual(1, b);
    }

    [TestMethod]
    public void EMA_9_Value()
    {
        double[] src = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        var ema = new EMA(src, 10);
        var a = ema.Moment(1);
        var b = ema.Next(1);
        Assert.AreEqual(1, b);
        Assert.AreEqual(1, b);
    }

    [TestMethod]
    public void EMA_10_Value()
    {
        double[] src = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var ema = new EMA(src, 10);
        var a = ema.Moment(1);
        var b = ema.Next(1);
        Assert.AreEqual(1, b);
        Assert.AreEqual(1, b);
    }

    [TestMethod]
    public void aaa2()
    {
        var ema = new EMA([double.NaN, double.NaN, double.NaN], 2);
        ema.Next(double.NaN);

        var alpha = 2d / (2d + 1d);
        var results = new List<double>
        {
            ema.Next(1),
            ema.Next(2),
            ema.Next(4),
            ema.Next(8),
            ema.Next(16),
            ema.Next(32),
            ema.Next(64)
        };

        Assert.AreEqual(double.NaN, results[0]);
        Assert.AreEqual(1.5, results[1]);
        // price * k + ema * (1 - k);
        // Assert.AreEqual(, results[2]);
        Assert.AreEqual(alpha * 4 + (1 - alpha) * results[1], results[2]);
        Assert.AreEqual(alpha * 8 + (1 - alpha) * results[2], results[3]);
        Assert.AreEqual(alpha * 16 + (1 - alpha) * results[3], results[4]);
        Assert.AreEqual(alpha * 32 + (1 - alpha) * results[4], results[5]);
        Assert.AreEqual(alpha * 64 + (1 - alpha) * results[5], results[6]);
    }
}
