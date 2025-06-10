namespace Learning.Records;
public record TableDefinitionRecord(string Symbol, int IntervalInSec)
{
    public TableDefinitionRecord(string symbol, KlineInterval klineInterval) : this(symbol, (int)klineInterval)
    {

    }
};

public static class TableDefinitionRecordExtension
{
    public static string GetTableName(this TableDefinitionRecord tableDef) => $"{tableDef.Symbol}_{tableDef.IntervalInSec}";
    public static string GetKlineKey(this DateTime time) => $"{time:yyyyMMddHHmmss}";
}

