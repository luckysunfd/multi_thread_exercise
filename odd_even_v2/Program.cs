using System;
using System.Threading;


namespace odd_even_v2
{
    class LockMe {}

    /*
     * 由于最终的输出是交替输出奇偶数，而奇偶数的规律是1234...，n-1,n,n+1..., 这样
     * 最终的输出效果是这样就可以，类似于按照顺序输出一串有序数字
     *
     * 所以移除线程方法内的数字的奇偶判断，然后每当一个线程输出一个数字后，这个数字在线程内increment，然后唤醒另一个线程，输出这个数字，然后这个数字在被唤醒的线程内increment，这样循环，直到到达限定的终止值
     * 
     * 这种方法采用了一个共享的变量，可以显著减少算法执行的复杂量级
     */
    class Sln
    {
        // 使用一个共享变量，这个变量的值就是当前要判断输出的数字
        private static int num = 1;

        private static LockMe lm = new LockMe();

        public static void f1()
        {
            lock(lm)
            {
                while(num <= 100)
                {
                    Console.WriteLine("{0} - {1}", Thread.CurrentThread.ManagedThreadId, num);
                    num++;
                    Monitor.Pulse(lm);
                    Monitor.Wait(lm); // 当前线程在这里阻塞，并进入wait queues，等到pulse唤醒进入ready queues，得到锁后即可执行
                }
                Monitor.Pulse(lm); // 防止出现死锁 
            }
        }

        public static void f2()
        {
            lock(lm)
            {
                while(num <= 100)
                {
                    Console.WriteLine("{0} - {1}", Thread.CurrentThread.ManagedThreadId, num);
                    num++;
                    Monitor.Pulse(lm);
                    Monitor.Wait(lm);
                }
                Monitor.Pulse(lm);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(Sln.f1));
            Thread t2 = new Thread(new ThreadStart(Sln.f2));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Hello World!");
        }
    }
}
