using Learning.Adapters;
using Learning.DataAccess;

namespace Learning.Mathematic;

//https://www.investopedia.com/terms/s/sma.asp
public class SMA : IDisposable
{
    readonly BinanceRestApiAdapter _restApiAdapter;
    readonly KlineDataAccess _klineDataAccess;

    public SMA()
    {
        _restApiAdapter = new BinanceRestApiAdapter();
        _klineDataAccess = new KlineDataAccess();
    }

    public async Task<decimal?> Calculate(TableDefinitionRecord tableDef, KlineRecord current, DateTime time, int length)
    {
        // check on DataAccess
        var lengthToFetch = length - 1;
        var cachedKlines = _klineDataAccess.GetKlinesByLength(tableDef, length - 1, time);
        var klines = cachedKlines.ToArray();

        // cannot find a cache
        if (klines.Length == 0)
        {
            klines = await _restApiAdapter.FetchKlines(tableDef, time.AddSeconds(-tableDef.IntervalInSec * lengthToFetch), time, lengthToFetch);
            _klineDataAccess.SetKlines(tableDef, klines);
        }
        else if (cachedKlines.Count < lengthToFetch)
        {
            // fetch missing klines
            var lastKline = cachedKlines.Last();
            var missingKlines = await _restApiAdapter.FetchKlines(tableDef, lastKline.CloseTime, time);
            _klineDataAccess.SetKlines(tableDef, missingKlines);
            klines = [.. klines, .. missingKlines];
        }

        var closePrices = klines.Select(x => x.ClosePrice).ToList();

        return (closePrices.Select(k => Convert.ToDecimal(k)).Sum() + current.ClosePrice) / length;
    }

    public void Dispose()
    {
        _restApiAdapter.Dispose();
    }
}
