using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace MongoDbTestConsole
{
    // https://stackoverflow.com/questions/29932723/how-to-limit-an-array-size-in-mongodb
    public class TestArraySlice
    {
        private const string Collection = "arrayslice";

        public void Run(MongoClient client)
        {
            var db = client.GetDatabase(Program.Database);
            var collection = db.GetCollection<BsonDocument>(Collection);

            var entry = new BsonDocument
            {
                {"createdAt", DateTime.UtcNow},
                {"code", "1234"},
                {"data", new BsonArray(0)}
            };

            var entryFilter = Builders<BsonDocument>.Filter.Eq("code", "1234");

            collection.ReplaceOne(entryFilter, entry, new UpdateOptions { IsUpsert = true });

            for (int i = 1; i <= 6; i++)
            {
                collection.UpdateOne(entryFilter, Builders<BsonDocument>.Update.PushEach("data", new[] { "test" + i }, -5));
            }

            var dbValue = collection.Find<BsonDocument>(entryFilter).FirstOrDefault();
            Console.WriteLine(dbValue.ToJson());

            Console.WriteLine("done, press enter to exit.");
            Console.ReadLine();
        }
    }
}
