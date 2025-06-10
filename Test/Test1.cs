using Binance.Net.Enums;

namespace Test
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestLength()
        {
            var a = (KlineInterval)300;
            Assert.AreEqual(a, KlineInterval.FiveMinutes);
            // prepare
            var now = DateTime.Now;
            now = now.AddSeconds(-now.Second); // remove seconds.
            var length = 200;
            var intervalInSecond = 300; // 5 minutes in seconds

            var mod = now.Minute % (intervalInSecond / 60);
            var startTime = now.AddSeconds(-length * intervalInSecond);
            var actual = startTime.AddSeconds(intervalInSecond * length);
            Assert.AreEqual(now, actual);
        }

        [TestMethod]
        public void TestDbreeze()
        {
            // prepare
            var dbEngine = new DBreeze.DBreezeEngine(".\\db_test");
            using (var transaction = dbEngine.GetTransaction())
            {
                // insert
                var initTime = new DateTime(0);
                var startTime = new DateTime(0);
                for (int i = 0; i < 200; i++) 
                { 
                    var key = $"BTC_{startTime:yyyyMMddHHmmss}";
                    transaction.Insert("TestTable", key, "TestValue");
                    startTime = startTime.AddMinutes(5);
                }
                
                transaction.Commit();
                // get
                var data = transaction.SelectForwardStartFrom<string, string>("TestTable", $"BTC_{initTime:yyyyMMddHHmmss}", true);
                Assert.AreEqual(data.First().Key, $"BTC_{initTime:yyyyMMddHHmmss}");

                var startKey = $"BTC_{initTime.AddMinutes(5):yyyyMMddHHmmss}";
                data = transaction.SelectForwardStartFrom<string, string>("TestTable", startKey, true);
                Assert.AreEqual(data.First().Key, startKey);

                startKey = $"BTC_{initTime.AddMinutes(5).AddMinutes(5):yyyyMMddHHmmss}";
                Assert.AreEqual(data.ElementAt(1).Key, startKey);
                    
            }
        }
    }
}
