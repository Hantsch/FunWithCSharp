using System;
using System.Collections.Generic;
using UsingCachInCloudArchitecture.Interfaces;

namespace UsingCachInCloudArchitecture
{
    /// <summary>
    /// This simulates a chach that is synchronized,
    /// we are simulating instances via thrads so i synchronize the threads by locking
    /// a synch object
    /// </summary>
    public class SimulatedConcurrentCache : ICacheStore
    {
        private static Dictionary<string, object> Cache = new Dictionary<string, object>();
        private static volatile object _Sync = new object();

        public T Get<T>(string key)
        {
            lock (_Sync)
            {
                if (!Cache.ContainsKey(key))
                {
                    throw new KeyNotFoundException($"key: {key} not found in cache");
                }
                return (T)Cache[key];
            }
        }

        public void Set(string key, object data)
        {
            lock (_Sync)
            {
                Cache[key] = data;
            }
        }

        // while still using threads, the lock is enough to ensure concurrency
        // don't bother
        public string TryGetLease(TimeSpan timeout)
        {
            return string.Empty;
        }

        // don't bother
        public void ReleaseLease(string leaseId)
        { }
    }
}