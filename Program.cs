using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;
            Console.WriteLine("Start Run");
            Task task1 = Task.Factory.StartNew((ct1) =>
            {
                Console.WriteLine("Start Task 1");
                CancellationToken ct1Local = (CancellationToken)ct1;
                Task task2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Start Task 2");
                    while (true)
                    {
                        Console.Write(".");
                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("End Task 2");
                });
                try
                {
                    task2.Wait(ct1Local);
                }
                catch { }
                Thread.SpinWait(50000000); // relase resources
                Console.WriteLine("End Task 1");
            }, ct);
            Console.ReadLine();
            cts.Cancel();
            task1.Wait();
            Console.WriteLine("End Run");
            Console.ReadLine();
        }
    }
}
