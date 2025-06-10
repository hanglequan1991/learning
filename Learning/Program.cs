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
            await SocketClient.UsdFuturesApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", KlineInterval.FiveMinutes, async (stream) =>
            {
                var kline = stream.Data.Data as BinanceStreamKline;
                var sma = new SMA();

                if (kline!.Final)
                {
                    klineDataAccess.SetKlines(klineTableDef, [(KlineRecord)kline!]);
                }

                var smaValue = await sma.Calculate(klineTableDef, (KlineRecord)kline!, kline!.CloseTime, SMA_LENGTH_200);
                Console.WriteLine($"Open: {kline!.OpenPrice}, High: {kline.HighPrice}, Low: {kline.LowPrice}, Close: {kline.ClosePrice}, Volume: {kline.Volume}");
                Console.WriteLine($"SMA: {smaValue}");
            });

            await Task.Delay(TimeSpan.FromMinutes(30));
        }
        catch
        {
            throw;
        }
    }
}
