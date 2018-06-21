using AspNetCoreWebMefDI.Interfaces;
using System.Collections.Generic;
using System.Composition;

namespace AspNetCoreWebMefDI.Services
{
    [Export(typeof(IValueProvider))]
    public class DictinaryValueProvider : IValueProvider
    {
        private readonly static Dictionary<string, string> ValueStore = new Dictionary<string, string>()
        {
            { "sayHello", "Hello World!" }
        };

        public DictinaryValueProvider()
        {

        }

        public string GetValue(string keyName)
        {
            if (!ValueStore.ContainsKey(keyName))
            {
                throw new KeyNotFoundException($"could not find value for: {keyName}");
            }

            return ValueStore[keyName];
        }
    }
}
