using System;
using System.Threading;


namespace odd_even
{
    class LockMe
    {
    }

    class A 
    {
        private static LockMe lm = new LockMe();

        public static void Odd()
        {
            lock(lm)
            {
                for(int i = 1; i <= 50000; i++)
                {
                    if( i % 2 == 1 )
                    {
                        Console.WriteLine("CurThreadId->{0} - CurNum->{1}", Thread.CurrentThread.ManagedThreadId, i);
                    }
                    else
                    {
                        // 先pulse发通知，再wait阻塞，不然只能死锁
                        Monitor.Pulse(lm);
                        Monitor.Wait(lm);
                    }
                }
                // 该处理的都处理了，但是与我达成契约的那个线程还在等我通知它
                Monitor.Pulse(lm);
            }
        }

        public static void Even()
        {
            lock(lm)
            {
                for(int j = 0; j <= 50000; j++)
                {
                    if( j % 2 == 0 )
                    {
                        Console.WriteLine("CurThreadId->{0} - CurNum->{1}", Thread.CurrentThread.ManagedThreadId, j);
                    }
                    else
                    {
                        Monitor.Pulse(lm);
                        Monitor.Wait(lm);
                    }
                }
                Monitor.Pulse(lm);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(A.Even));
            Thread t2 = new Thread(new ThreadStart(A.Odd));

            t1.Start(); // 线程就如就绪态，等待os调度
            t2.Start(); //
            // 一定要阻塞祝线程，不然很可能工作线程得不到运行机会祝线程就结束了，由于工作线程不是分离线程，所以随着祝线程的结束，资源被回收，工作线程就没机会运行了
            t1.Join();
            t2.Join();

            Console.WriteLine("Hello World!");
        }
    }
}
