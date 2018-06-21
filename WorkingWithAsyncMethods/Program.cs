using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkingWithAsyncMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            var doStuff = Run();
            Task.WaitAll(doStuff);
            Console.ReadKey();
        }

        private static async Task Run()
        {
            Console.WriteLine($"starting test");
            var doSomething = DoSomething();
            var calculateSomething = CalculateSomething();

            var result = await calculateSomething;
            Console.WriteLine($"this is the result: {result}");
        }

        private static async Task DoSomething()
        {
            Console.WriteLine($"i am doing something");
            Thread.Sleep(2000);
        }

        private static async Task<int> CalculateSomething()
        {
            Console.WriteLine($"i am calculating something");
            Thread.Sleep(800);
            return 7;
        }
    }
}
