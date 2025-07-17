using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot.Socket;
using Learning.DataAccess;
using Learning.Mathematic;

namespace Learning;


internal class Program
{
    const int SMA_LENGTH_200 = 200;
    static async Task Main()
    {
        try
        {
            var klineDataAccess = new KlineDataAccess();
            var klineTableDef = new TableDefinitionRecord("BTCUSDT", KlineInterval.FiveMinutes);

            var SocketClient = new BinanceSocketClient(options =>
            {
                options.Environment = Binance.Net.BinanceEnvironment.Live;
            });

            var restClient = new BinanceRestClient(options =>
            {
                options.Environment = Binance.Net.BinanceEnvironment.Live;
            });

            var klines = await restClient.UsdFuturesApi.ExchangeData.GetKlinesAsync("BTCUSDT", KlineInterval.FiveMinutes);
            var length = 200;
            var closedPrices = klines.Data.Where(x => x.CloseTime < DateTime.UtcNow).Select(x => (double)x.ClosePrice).ToArray();
            var sma = new SMA(closedPrices, length);
            var ema = new EMA(closedPrices, 25);
            var macd = new MACD(closedPrices, 12, 26);
            var min = new MIN(closedPrices, length);
            var max = new MAX(closedPrices, length);
            var rsi = new RSI(closedPrices, 14);
            await SocketClient.UsdFuturesApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", KlineInterval.FiveMinutes, (stream) =>
            {
                try
                {
                    var kline = stream.Data.Data as BinanceStreamKline;
                    var closePrice = kline!.ClosePrice;
                    var smaValue = sma.Moment((double)kline!.ClosePrice);
                    var emaValue = ema.Moment((double)kline.ClosePrice);
                    var macdValue = macd.Moment((double)kline.ClosePrice);
                    var minValue = min.Moment((double)closePrice);
                    var maxValue = max.Moment((double)closePrice);
                    var rsiValue = rsi.Moment((double)closePrice);
                    if (kline!.Final)
                    {
                        smaValue = sma.Next((double)kline.ClosePrice);
                        emaValue = ema.Next((double)kline.ClosePrice);
                        macdValue = macd.Next((double)kline.ClosePrice);
                        minValue = min.Next((double)closePrice);
                        maxValue = max.Next((double)closePrice);
                        rsiValue = rsi.Next((double)closePrice);
                    }

                    Console.WriteLine($"Open: {kline!.OpenPrice}, High: {kline.HighPrice}, Low: {kline.LowPrice}, Close: {kline.ClosePrice}, Volume: {kline.Volume}");
                    Console.WriteLine($"SMA: {smaValue}");
                    Console.WriteLine($"EMA: {emaValue}");
                    Console.WriteLine($"MACD: {macdValue}");
                    Console.WriteLine($"MIN: {minValue}, MAX: {maxValue}");
                    Console.WriteLine($"RSI: {rsiValue}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });

            await Task.Delay(TimeSpan.FromMinutes(30));
        }
        catch
        {
            throw;
        }
    }
}
