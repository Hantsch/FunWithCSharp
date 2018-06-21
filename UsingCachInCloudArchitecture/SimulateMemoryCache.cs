using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsingCachInCloudArchitecture.Interfaces;

namespace UsingCachInCloudArchitecture
{
    /// <summary>
    /// For the purpose of this example this implementation is using
    /// a static dictionary but per thread, as the cloud environment will be simulated by threads.
    /// </summary>
    public class SimulateMemoryCache : ICacheStore
    {
        // our cache duh
        private static Dictionary<int, Dictionary<string, object>> Cache = new Dictionary<int, Dictionary<string, object>>();

        /// <summary>
        /// helps us ensure a dictionary for the thread exists
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> GetCacheForCurrentThread()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            if (!Cache.ContainsKey(threadId))
            {
                Cache.Add(threadId, new Dictionary<string, object>());
            }

            return Cache[threadId];
        }

        public T Get<T>(string key)
        {
            var threadCache = this.GetCacheForCurrentThread();
            if (!threadCache.ContainsKey(key))
            {
                // normaly you would throw an exception as get is expected to return a value
                // for the demonstration we dont want this
                //throw new KeyNotFoundException($"key: {key} not found in cache");
                return default(T);
            }

            return (T)threadCache[key];
        }
        

        public void Set(string key, object data)
        {
            var threadCache = this.GetCacheForCurrentThread();
            threadCache[key] = data;
        }

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
