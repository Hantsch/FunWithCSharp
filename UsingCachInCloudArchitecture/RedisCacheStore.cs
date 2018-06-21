using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsingCachInCloudArchitecture.Interfaces;

namespace UsingCachInCloudArchitecture
{
    public class RedisCacheStore : ICacheStore
    {
        private readonly ConnectionMultiplexer RedisConnection;
        private readonly IDatabase Cache;

        public RedisCacheStore(string connectionString)
        {
            this.RedisConnection = ConnectionMultiplexer.Connect(connectionString);
            this.Cache = this.RedisConnection.GetDatabase();
        }

        public T Get<T>(string key)
        {
            var value = this.Cache.StringGet(key).ToString();
            return JsonConvert.DeserializeObject<T>(value);
        }
        
        public void Set(string key, object data)
        {
            var stringValue = JsonConvert.SerializeObject(
                data,
                new Newtonsoft.Json.Converters.StringEnumConverter());
            this.Cache.StringSet(key, stringValue);
        }

        // bother?
        public void ReleaseLease(string leaseId)
        {
        }

        public string TryGetLease(TimeSpan timeout)
        {
            return string.Empty;
        }
    }
}
