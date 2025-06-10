using Binance.Net.Interfaces;
using Binance.Net.Objects.Models;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Spot.Socket;

namespace Learning.Records;

public record KlineRecord(
    decimal QuoteVolume,
    DateTime CloseTime,
    decimal Volume,
    decimal ClosePrice,
    decimal LowPrice,
    decimal HighPrice,
    DateTime OpenTime,
    decimal OpenPrice
)
{
    public static explicit operator KlineRecord(BinanceKlineBase kline)
    {
        return new(kline.QuoteVolume, kline.CloseTime, kline.Volume, kline.ClosePrice, kline.LowPrice, kline.HighPrice, kline.OpenTime, kline.OpenPrice);
    }

    public static explicit operator KlineRecord(BinanceStreamKline kline)
    {
        return new(kline.QuoteVolume, kline.CloseTime, kline.Volume, kline.ClosePrice, kline.LowPrice, kline.HighPrice, kline.OpenTime, kline.OpenPrice);
    }

    public static explicit operator KlineRecord(BinanceFuturesUsdtKline kline)
    {
        return new(kline.QuoteVolume, kline.CloseTime, kline.Volume, kline.ClosePrice, kline.LowPrice, kline.HighPrice, kline.OpenTime, kline.OpenPrice);
    }
}

