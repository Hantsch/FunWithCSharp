using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MongoDbTestConsole
{
    public class TTLExample
    {
        // https://docs.mongodb.com/manual/core/index-ttl/
        // create index at collection via atlas
        // fields
        // { "createdAt": 1 }
        // options
        // { background : true, expireAfterSeconds : 10 }

        // documents get removed but not exactly after 10 seconds as the ttl monitor by default runs every 60 seconds
        // http://hassansin.github.io/working-with-mongodb-ttl-index

        private const string Collection = "ttlexample";

        public void Run(MongoClient client)
        {
            var db = client.GetDatabase(Program.Database);
            var collection = db.GetCollection<BsonDocument>(Collection);

            //var indexKey = Builders<BsonDocument>.IndexKeys.Ascending("createdAt");
            //var indexModel = new CreateIndexModel<BsonDocument>(indexKey, new CreateIndexOptions() { ExpireAfter = TimeSpan.FromSeconds(10) });
            //var resposne = collection.Indexes.CreateOne(indexModel);

            var entry = new BsonDocument
            {
                {"createdAt", DateTime.UtcNow},
                {"message", "this is a test message"},
            };

            collection.InsertOne(entry);

            var watch = Stopwatch.StartNew();

            while (collection.Find(Builders<BsonDocument>.Filter.Eq("_id", entry["_id"])).ToList().Count > 0)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }

            Console.WriteLine();
            watch.Stop();

            Console.WriteLine($"collection empty after: {watch.Elapsed}");
            Console.ReadLine();
        }
    }
}
