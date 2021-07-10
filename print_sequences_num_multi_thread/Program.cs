// 多个线程，打印0-100，按照顺序打印
// 假设有五个线程，交替执行，a，b，c，d，e
// 各个线程要打印的数字通过一个static int记录

using System;
using System.Threading;


namespace print_sequences_num_multi_thread
{
    class Program
    {
        // 当前要打印的数字
        static int cur = 0;
        // 三个线程，三个信号量
        static Semaphore s1 = new Semaphore(1, 1);
        static Semaphore s2 = new Semaphore(0, 1);
        static Semaphore s3 = new Semaphore(0, 1);

        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(f1));
            Thread t2 = new Thread(new ThreadStart(f2));
            Thread t3 = new Thread(new ThreadStart(f3));

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Console.WriteLine("Hello World!");
        }

        static void f1()
        {
            while(cur <= 100)
            {
                s1.WaitOne();
                if(cur <= 100)
                    Console.WriteLine("{0} - tId:{1}",cur, Thread.CurrentThread.ManagedThreadId);
                
                cur++;
                s2.Release();
            }
        }
        static void f2()
        {
            while(cur <= 100)
            {
                s2.WaitOne();
                if(cur <= 100)
                    Console.WriteLine("{0} - tid:{1}", cur, Thread.CurrentThread.ManagedThreadId);
                cur++;
                s3.Release();
            }
        }
        static void f3()
        {
            while(cur <= 100)
            {
                s3.WaitOne();
                if(cur <= 100)
                    Console.WriteLine("{0} - tid:{1}", cur, Thread.CurrentThread.ManagedThreadId);
                cur++;
                s1.Release();
            }
        }
    }
}
