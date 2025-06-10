namespace Learning.Constants;

public static class KlineConstants
{
    public const string TABLE_NAME_KLINES = "BTCUSDT";
    public static string GetKlineKey(int interval, DateTime time) => $"{interval}_{time:yyyyMMddHHmmss}";

}