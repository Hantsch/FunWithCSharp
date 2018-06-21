using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using UsingCachInCloudArchitecture.Interfaces;

namespace UsingCachInCloudArchitecture
{
    /// <summary>
    /// Example implementing of a chaching system utilizing Azure Storage accounts.
    /// </summary>
    public class ConcurentCacheUsingAzureStorage : ICacheStore
    {
        private const string CacheContextName = "cache_lease";

        private readonly CloudBlobContainer LeaseContainer;

        public ConcurentCacheUsingAzureStorage(string storageConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            this.LeaseContainer = blobClient.GetContainerReference("mychache");
            this.LeaseContainer.CreateIfNotExists();
        }

        public T Get<T>(string key)
        {
            var blob = this.LeaseContainer.GetBlockBlobReference(String.Format("{0}.json", key));
            var content = blob.DownloadText();

            if (content == null)
            {
                throw new KeyNotFoundException($"key: {key}, not found in cache");
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        public void Set(string key, object data)
        {
            var blob = this.LeaseContainer.GetBlockBlobReference(String.Format("{0}.json", key));
            var stringValue = JsonConvert.SerializeObject(
                data,
                new Newtonsoft.Json.Converters.StringEnumConverter());
            blob.UploadText(stringValue);
        }

        // i am locking the whole container, this is not the best scenario
        public string TryGetLease(TimeSpan timeout)
        {
            var timout = DateTime.UtcNow.Add(timeout);
            while (DateTime.UtcNow < timout)
            {
                try
                {
                    return this.LeaseContainer.AcquireLease(TimeSpan.FromSeconds(30), null);
                }
                catch
                {
                    Thread.Sleep(200);
                }
            }
            throw new TimeoutException("get lease timed out");
        }

        public void ReleaseLease(string leaseId)
        {
            this.LeaseContainer.ReleaseLease(AccessCondition.GenerateLeaseCondition(leaseId));
        }
    }
}
