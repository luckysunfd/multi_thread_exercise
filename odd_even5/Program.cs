using System;
using System.Threading;

// 使用信号量 实现 交替打印 奇偶数 两个线程 

namespace odd_even5
{
    class Program
    {
        private static Semaphore s1 = new Semaphore(0, 1);
        private static Semaphore s2 = new Semaphore(1, 1);

        private static void odd()
        {
            for(int i = 0; i <= 10; i++)
            {
                if( i % 2 == 0 )
                {
                    s1.WaitOne();
                    Console.WriteLine(i);
                }else{
                    s2.Release();
                }
            }
        }

        private static void even()
        {
            for(int i = 0; i <= 10; i++)
            {
                if( i % 2 == 1)
                {
                    s2.WaitOne();
                    Console.WriteLine(i);
                }else{
                    s1.Release();
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
