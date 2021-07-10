// 使用信号量控制两个线程 交替打印奇偶数
using System;
using System.Threading;


namespace semaphore10
{
    class Program
    {
        private static Semaphore semaodd = new Semaphore(1, 1);
        private static Semaphore semaeven = new Semaphore(0, 1);
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

        private static void odd()
        {
            semaodd.WaitOne();
            for(int i = 0; i <= 10; i++)
            {
                if( i % 2 == 0)
                {
                    Console.WriteLine("{0} - {1}", Thread.CurrentThread.ManagedThreadId, i);
                }
            }
            semaeven.Release();
        }

        private static void even()
        {
            semaeven.WaitOne();
            for(int i = 0; i <= 10; i++)
            {
                if(i % 2 == 1)
                {
                    Console.WriteLine("{0} - {1}", Thread.CurrentThread.ManagedThreadId, i);
                }
            }
        }
    }
}
