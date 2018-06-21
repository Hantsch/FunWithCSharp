using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsingCachInCloudArchitecture.Interfaces;

namespace UsingCachInCloudArchitecture
{
    class Program
    {
        private static string CounterKeyName = "access_counter";

        static void Main(string[] args)
        {
            ICacheStore cache = new SimulateMemoryCache();
            RunScenario(cache);
            var memoryCacheCount = cache.Get<int>(CounterKeyName);

            cache = new SimulatedConcurrentCache();
            RunScenario(cache);
            var concurentCacheCount = cache.Get<int>(CounterKeyName);

            cache = new ConcurentCacheUsingAzureStorage("...");
            RunScenario(cache);
            var azureCacheCount = cache.Get<int>(CounterKeyName);

            cache = new RedisCacheStore("...");
            RunScenario(cache);
            var redisCacheCount = cache.Get<int>(CounterKeyName);
            Console.WriteLine();

            // a not synchronized cache might have different values, this will cause you issues
            Console.WriteLine($"memory cache scenario, value of accesscount: {memoryCacheCount}");
            Console.WriteLine($"sample of how a cache should behave, value of accesscount: {concurentCacheCount}");
            Console.WriteLine($"concurrent cache with azure storage, value of accesscount: {azureCacheCount}");
            Console.WriteLine($"redis cache, value of accesscount: {redisCacheCount}");

            Console.WriteLine();
            Console.WriteLine("press [enter] to exit");
            Console.ReadLine();
        }

        private static void RunScenario(ICacheStore cache)
        {
            // set counter to 0 before running scenario
            cache.Set(CounterKeyName, 0);
            var tasks = new List<Task>();
            Console.WriteLine("starting \"simulated cloud instances\"");
            tasks.Add(Task.Factory.StartNew(() => RunInstance1(cache)));
            tasks.Add(Task.Factory.StartNew(() => RunInstance2(cache)));
            tasks.Add(Task.Factory.StartNew(() => RunInstance3(cache)));

            Task.WaitAll(tasks.ToArray());
        }

        private static void RunInstance1(ICacheStore cache)
        {
            var instanceId = 1;
            Console.WriteLine($"{instanceId}: ------- instance started -------");

            Console.WriteLine($"{instanceId}: get requst");
            Thread.Sleep(100);
            var lease = cache.TryGetLease(TimeSpan.FromSeconds(20));
            var counter = cache.Get<int>(CounterKeyName);
            counter++;
            cache.Set(CounterKeyName, counter);
            cache.ReleaseLease(lease);
            Console.WriteLine($"{instanceId}: processed");
            Thread.Sleep(300);

            Console.WriteLine($"{instanceId}: get requst");
            Thread.Sleep(150);
            lease = cache.TryGetLease(TimeSpan.FromSeconds(20));
            counter = cache.Get<int>(CounterKeyName);
            counter++;
            cache.Set(CounterKeyName, counter);
            cache.ReleaseLease(lease);
            Console.WriteLine($"{instanceId}: processed");

            Console.WriteLine($"{instanceId}: access count: {cache.Get<int>(CounterKeyName)}");
            Console.WriteLine($"{instanceId}: ------- instance shutdown -------");
        }

        private static void RunInstance2(ICacheStore cache)
        {
            var instanceId = 2;
            Console.WriteLine($"{instanceId}: ------- instance started -------");

            Console.WriteLine($"{instanceId}: get requst");
            Thread.Sleep(300);
            var lease = cache.TryGetLease(TimeSpan.FromSeconds(20));
            var counter = cache.Get<int>(CounterKeyName);
            counter++;
            cache.Set(CounterKeyName, counter);
            cache.ReleaseLease(lease);
            Console.WriteLine($"{instanceId}: processed");
            Thread.Sleep(200);

            Console.WriteLine($"{instanceId}: access count: {cache.Get<int>(CounterKeyName)}");
            Console.WriteLine($"{instanceId}: ------- instance shutdown -------");
        }

        private static void RunInstance3(ICacheStore cache)
        {
            var instanceId = 3;
            Console.WriteLine($"{instanceId}: ------- instance started -------");

            Console.WriteLine($"{instanceId}: get requst");
            Thread.Sleep(200);
            var lease = cache.TryGetLease(TimeSpan.FromSeconds(20));
            var counter = cache.Get<int>(CounterKeyName);
            counter++;
            cache.Set(CounterKeyName, counter);
            cache.ReleaseLease(lease);
            Console.WriteLine($"{instanceId}: processed");
            Thread.Sleep(200);

            Console.WriteLine($"{instanceId}: get requst");
            Thread.Sleep(250);
            lease = cache.TryGetLease(TimeSpan.FromSeconds(20));
            counter = cache.Get<int>(CounterKeyName);
            counter++;
            cache.Set(CounterKeyName, counter);
            cache.ReleaseLease(lease);
            Console.WriteLine($"{instanceId}: processed");
            Thread.Sleep(100);

            Console.WriteLine($"{instanceId}: get requst");
            Thread.Sleep(150);
            lease = cache.TryGetLease(TimeSpan.FromSeconds(20));
            counter = cache.Get<int>(CounterKeyName);
            counter++;
            cache.Set(CounterKeyName, counter);
            cache.ReleaseLease(lease);
            Console.WriteLine($"{instanceId}: processed");
            Thread.Sleep(200);

            Console.WriteLine($"{instanceId}: access count: {cache.Get<int>(CounterKeyName)}");
            Console.WriteLine($"{instanceId}: ------- instance shutdown -------");
        }
    }
}
