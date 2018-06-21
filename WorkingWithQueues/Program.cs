using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WorkingWithQueues
{
    class Program
    {
        static Queue<SomeDataContract> MyCoolQueue = new Queue<SomeDataContract>();
        static string[] Messages = new string[]
            {
                "hello",
                "whassup",
                "some text",
                "i am not verry creative"
            };
        static Random Randomizer = new Random();
        static bool RunProgramm = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Start programm by pressing enter, programm will run untill you close it or press any key");
            Console.ReadLine();
            var runningTasks = new List<Task>();
            
            // start a bunch of tasks to write into the queue
            for (int i = 0; i < 10; i++)
            {
                runningTasks.Add(Task.Factory.StartNew(WriteToQueue));
            }

            runningTasks.Add(Task.Factory.StartNew(ReadFromQueue));

            Console.ReadKey();
            RunProgramm = false;
            Task.WaitAll(runningTasks.ToArray());
            Console.WriteLine("Programm is finished, press eny key to exit");
            Console.ReadKey();
        }

        private static void ReadFromQueue()
        {
            while (RunProgramm)
            {
                while (MyCoolQueue.Count > 0)
                {
                    var data = MyCoolQueue.Dequeue();
                    Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] read message (id: {data.SomeUniqueId} from queue: {data.AMessageDuh}");
                }
                Thread.Sleep(Randomizer.Next(100, 200));
            }
        }

        private static void WriteToQueue()
        {
            while (RunProgramm)
            {
                var message = Messages[Randomizer.Next(0, Messages.Length - 1)];
                var data = new SomeDataContract(message);
                Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] enqueue message: {data.SomeUniqueId}");
                MyCoolQueue.Enqueue(data);
                Thread.Sleep(Randomizer.Next(1000, 2000));
            }
        }
    }
}
