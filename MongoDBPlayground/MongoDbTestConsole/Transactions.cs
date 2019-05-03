using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace MongoDbTestConsole
{
    public class Transactions
    {
        private const string Collection = "maintable";

        public void Run(MongoClient client)
        {
            var db = client.GetDatabase(Program.Database);
            var collection = db.GetCollection<BsonDocument>(Collection);

            collection.DeleteMany("{ 'lastName': 'blah' }");

            var personName = "Test";

            var entry = new BsonDocument
            {
                { "createdAt", DateTime.UtcNow },
                { "firstName", personName },
                { "lastName", "blah" }
            };

            collection.InsertOne(entry);

            Console.Write("press enter to start session and change value");
            Console.ReadLine();

            using (var session = db.Client.StartSession())
            {
                session.StartTransaction(new TransactionOptions(readConcern: ReadConcern.Snapshot, writeConcern: WriteConcern.WMajority));

                collection.UpdateOne(session, "{ 'lastName': 'blah' }", Builders<BsonDocument>.Update.Set("firstName", "Dummy"));

                Console.Write("press y to commit transaction, otherwise abort");
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Y)
                {
                    session.CommitTransaction();
                }
                else
                {
                    session.AbortTransaction();
                }
            }
            
        }
    }
}
