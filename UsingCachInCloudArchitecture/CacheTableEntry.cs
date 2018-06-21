using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace UsingCachInCloudArchitecture
{
    public class CacheTableEntry : TableEntity
    {
        // requirede for serialization
        public CacheTableEntry()
            : base()
        { }

        public CacheTableEntry(string key, object value)
            : base(key, string.Empty)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Use methods to retrieve value.
        /// </summary>
        public string CacheValue { get; set; }

        public void SetValue(object value)
        {
            var stringValue = JsonConvert.SerializeObject(
                value,
                new Newtonsoft.Json.Converters.StringEnumConverter());

            this.CacheValue = stringValue;
        }

        public T GetValue<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.CacheValue);
        }
    }
}
