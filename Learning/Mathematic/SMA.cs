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
        if (klines.Length == 0 || cachedKlines.Count < lengthToFetch)
        {
            klines = await _restApiAdapter.FetchKlines(tableDef, time.AddSeconds(-tableDef.IntervalInSec * lengthToFetch), time, lengthToFetch);
            _klineDataAccess.SetKlines(tableDef, klines);
        }
        
        if (klines.Length != length)
        {
            throw new Exception("Fetch Klines length, can't calculate SMA correctly");
        }

        var closePrices = klines.Select(x => x.ClosePrice).ToList();
        return (closePrices.Select(k => Convert.ToDecimal(k)).Sum() + current.ClosePrice) / length;
    }

    public void Dispose()
    {
        _restApiAdapter.Dispose();
    }
}
