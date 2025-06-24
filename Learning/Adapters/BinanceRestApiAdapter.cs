using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Futures;

namespace Learning.Adapters;

public class BinanceRestApiAdapter : IDisposable
{
    readonly BinanceRestClient _client;
    public BinanceRestApiAdapter()
    {
        _client = new BinanceRestClient(options =>
        {
            options.Environment = Binance.Net.BinanceEnvironment.Live;
        });

    }

    public async Task<KlineRecord[]> FetchKlines(TableDefinitionRecord tableDef, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
    {
        Console.WriteLine("Fetch Fapi");
        var klines = await _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(tableDef.Symbol, (KlineInterval)tableDef.IntervalInSec, startTime, endTime, limit);
        return klines.Data.Select(k => (KlineRecord)(k as BinanceFuturesUsdtKline)!).ToArray();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}