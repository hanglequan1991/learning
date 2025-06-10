using DBreeze.DataTypes;

namespace Learning.DataAccess;

public class KlineDataAccess
{
    public KlineDataAccess()
    {
        _dbEngine = DbContext.Engine;
    }

    readonly DBreeze.DBreezeEngine _dbEngine;

    public List<KlineRecord> GetKlinesByLength(TableDefinitionRecord tableDef, int length, DateTime time)
    {
        using var transaction = _dbEngine.GetTransaction();
        var startTime = time.AddSeconds(-tableDef.IntervalInSec * length);
        var startKey = startTime.GetKlineKey();
        var tableName = tableDef.GetTableName();
        var data = transaction.SelectForwardStartFrom<string, DbMJSON<KlineRecord>>(tableName, startKey, true).Take(length);

        return data.Select(x => x.Value.Get).ToList();
    }

    public void SetKlines(TableDefinitionRecord tableDef, KlineRecord[] klines)
    {
        var tableName = tableDef.GetTableName();
        using var transaction = _dbEngine.GetTransaction();
        foreach (var k in klines)
        {
            transaction.Insert<string, DbMJSON<KlineRecord>>(tableName, k.OpenTime.GetKlineKey(), k);
        }

        transaction.Commit();
    }
}