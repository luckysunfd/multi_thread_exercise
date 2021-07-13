using System;
using System.Threading;
// 两个线程， 交替执行，线程a先输出偶数，线程b紧跟着输出奇数，循环往复，直到到达终止点

namespace odd_evenv4
{
    class Program
    {
        private static ManualResetEvent mreodd = new ManualResetEvent(false);
        private static ManualResetEvent mreeven = new ManualResetEvent(true);

        private static void odd()
        {
            for(int i = 0; i <= 10; i++)
            {
                if(i % 2 == 0)
                {   mreodd.WaitOne();
                    Console.WriteLine("{0} - {1}", Thread.CurrentThread.ManagedThreadId, i);
                    mreodd.Reset();
                }else{
                    mreeven.Set();
                }
            }
        }

        private static void even()
        {
            for(int i = 0; i <= 10; i++)
            {
                if( i % 2 == 1)
                {
                    mreeven.WaitOne();
                    Console.WriteLine("{0} - {1}", Thread.CurrentThread.ManagedThreadId ,i);
                    mreeven.Reset();
                }else{
                    mreodd.Set();
                }
            }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(odd));
            Thread t2 = new Thread(new ThreadStart(even));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Hello World!");
        }
    }
}
