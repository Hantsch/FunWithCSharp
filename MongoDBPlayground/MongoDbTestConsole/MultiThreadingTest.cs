using System;
using System.Diagnostics;
using System.Threading;
using MongoDB.Driver;

namespace MongoDbTestConsole
{
    internal class MultiThreadingTest
    {
        private const string Collection = "testThreadInserts";
        private static int ThreadsRunning;

        internal void Run(MongoClient mongoClient)
        {
            var db = mongoClient.GetDatabase(Program.Database);
            var collection = db.GetCollection<BlahData>(Collection);

            // try with and without inserting first
            //collection.InsertOne(CreateEntry());

            var threads = 30;

            ThreadPool.SetMinThreads(threads, threads);

            for (int i = 0; i < threads; i++)
            {
                ThreadsRunning++;
                ThreadPool.QueueUserWorkItem(new WaitCallback(Fire), collection);
            }

            while (ThreadsRunning > 0)
            {
                Thread.Sleep(500);
            }

            Console.ReadLine();
        }

        private static void Fire(object state)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] [{Thread.CurrentThread.ManagedThreadId}] Fire!");
            var collection = state as IMongoCollection<BlahData>;
            var watch = Stopwatch.StartNew();

            collection.InsertOne(CreateEntry());

            watch.Stop();
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] [{Thread.CurrentThread.ManagedThreadId}] inserted entry, took: {watch.Elapsed}");
            ThreadsRunning--;
        }

        private static BlahData CreateEntry()
        {
            return new BlahData
            {
                EntryId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Code = "1234",
                Blah = "hello world"
            };
        }
    }
}