using MongoDB.Driver;

namespace MongoDbTestConsole
{
    class Program
    {
        private const string ConnectionString = "mongodb+srv://test:test1234@mytestcluster-m2hjn.mongodb.net/test?retryWrites=true";

        internal const string Database = "testdb";

        static void Main(string[] args)
        {
            var mongoClient = CreateConnection();

            //new TTLExample().Run(mongoClient);
            //new Transactions().Run(mongoClient);
            //new GeoLocation().Run(mongoClient);
            //new TestArraySlice().Run(mongoClient);
            //new TestConnectAndInsertSpeed().Run(mongoClient);
            new MultiThreadingTest().Run(mongoClient);
        }

        private static MongoClient CreateConnection()
        {
            return new MongoClient(ConnectionString);
        }
    }
}
