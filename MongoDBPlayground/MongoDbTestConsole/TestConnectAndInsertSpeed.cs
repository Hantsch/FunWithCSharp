using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;

namespace MongoDbTestConsole
{
    public class TestConnectAndInsertSpeed
    {
        private const string Collection = "testInserts";
        private const string IdFieldName = "entryId";

        public void Run(MongoClient mongoClient)
        {
            var db = mongoClient.GetDatabase(Program.Database);
            var collection = db.GetCollection<BlahData>(Collection);

            var entryId = "1";
            var entry = this.CreateEntry(entryId);

            var watch = Stopwatch.StartNew();
            // this takes longer than the second one because its creating a connection first i assume
            collection.InsertOne(entry);
            watch.Stop();

            Console.WriteLine($"first upsert took: {watch.Elapsed}");

            entryId = "2";
            entry = this.CreateEntry(entryId);

            watch.Restart();
            collection.InsertOne(entry);
            watch.Stop();

            Console.WriteLine($"second upsert took: {watch.Elapsed}");
            Console.ReadKey();
        }

        private BlahData CreateEntry(string id)
        {
            return new BlahData
            {
                EntryId = id,
                CreatedAt = DateTime.UtcNow,
                Code = "1234",
                Blah = "hello world"
            };
        }
    }

    class BlahData
    {
        public string EntryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Code { get; set; }
        public string Blah { get; set; }
    }
}
