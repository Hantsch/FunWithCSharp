using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace TestingCurrentContext
{
    class Program
    {
        static void Main(string[] args)
        {
            var runs = new List<Task>();
            for (int i = 0; i < 30; i++)
            {
                var name = $"blah{i}";
                runs.Add(ExecuteSomethingAsync(name));
                Thread.Sleep(20);
            }

            Task.WaitAll(runs.ToArray());
            Console.ReadLine();
        }

        private static Task ExecuteSomethingAsync(string executeName)
        {
            return Task.Factory.StartNew(() =>
            {
                //CallContext.SetData("context", executeName); // does not work, only for current thread
                CallContext.LogicalSetData("context", executeName);
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                    //Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - context: {CallContext.GetData("context")}");
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - context: {CallContext.LogicalGetData("context")}");
                });
            });
        }
    }
}
