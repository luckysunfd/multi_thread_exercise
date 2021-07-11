using System;
using System.Threading;

/*
1. 需要四个线程，每个线程干自己的事情
   比如： 线程a 整除3 
         线程b 整除5
         线程c 整除3、5both
         线程d 剩下的数字 直接 输出

    每当开始一个新的数字，让线程d先开始执行，判断是否是自己要处理的，然后根据不同的情况，唤醒通知不同的线程执行它们的任务
    别的线程执行完，再唤醒通知线程d，重复前面步骤，直到到达n的限定

    每个线程内的循环是必须的，因为如果没有循环，线程直接就退出了，还一层作用就是防止出现数字大于最大边界了，但是却进入循环内
    处于阻塞状态、但是调度线程却已经退出了这种情况，

    另外，线程内的if语句也是必须的，防止出现线程阻塞空等状态

*/


namespace alternately_print_string
{
    class Program
    {
        static void Main()
        {
            var a  = new Fizzbuzz(20);
            Thread t1 = new Thread(a.Buzz);
            Thread t2 = new Thread(a.Fizz);
            Thread t3 = new Thread(a.FizzBuzz);
            Thread t4 = new Thread(a.Number);

            t1.Start();
            t3.Start();
            t4.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            Console.WriteLine("world");
        }
    }
    class Fizzbuzz 
    {
        static int cur;
        private int n;
        static Semaphore f = new Semaphore(0, 1);
        static Semaphore b = new Semaphore(0, 1);
        static Semaphore fb = new Semaphore(0, 1);
        static Semaphore num = new Semaphore(1, 1);
        public Fizzbuzz(int n)
        {
            this.n = n;
        }

        // 被3 整除 输出fizz
        // 被5 整除 输出 buzz
        // 同时被3、5 整除，输出fizzbuzz
        // 其他 数字 正常输出
        public void Fizz()
        {
            // printFizz outputs "Fizz"
            for(int i = 0; i <= n; i++) // 这层循环不能少，放置最后的边界进入，一直处于某个线程阻塞状态
            {
                if(i % 3 == 0 && i % 5 != 0)
                {
                    f.WaitOne();
                    Console.WriteLine("fizz");
                    num.Release();
                }
            }
        }

        public void Buzz()
        {
            // printBuzz outputs "Buzz"
            for(int i = 0; i <= n; i++) // 这层循环不能少，放置最后的边界进入，一直处于某个线程阻塞状态
            {
                if(i % 5 == 0 && i % 3 != 0) // 自己先整到自己要处理的数字，等信号
                {
                    b.WaitOne();
                    Console.WriteLine("buzz");
                    num.Release();
                }
            }
        }
        public void FizzBuzz()
        {
            // printFizzBuzz outpus "fizzbuzz"
                 for(int i = 0; i <= n; i++) // 这层循环不能少，放置最后的边界进入，一直处于某个线程阻塞状态
            {
                if(i % 3 == 0 && i % 5 == 0)
                {
                    fb.WaitOne();
                    Console.WriteLine("ffbb");
                    num.Release();
                }
            }
    
        }

        public void Number()
        {
            // printNumber outputs "x", where x is an integer
            for(cur = 0 ; cur <= n; cur++)
            {
                num.WaitOne();
                if( cur % 3 == 0 && cur % 5 != 0)
                {
                    f.Release();
                }
                else if( cur % 5 == 0 && cur % 3 != 0)
                {
                    b.Release();
                }else if( cur % 15 == 0)
                {
                    fb.Release();
                }else 
                {
                    Console.WriteLine(cur);
                    num.Release();
                }
            }
        }
    }
}
