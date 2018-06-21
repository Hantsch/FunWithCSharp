using System;

namespace UsingCachInCloudArchitecture.Interfaces
{
    /// <summary>
    /// Simple interface to abstract different caching machanisms
    /// </summary>
    public interface ICacheStore
    {
        T Get<T>(string key);

        void Set(string key, object data);

        string TryGetLease(TimeSpan timeout);

        void ReleaseLease(string leaseId);
    }
}
