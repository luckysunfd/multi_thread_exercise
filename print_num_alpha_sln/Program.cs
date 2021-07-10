// 三个线程 交替打印123 打印n次
// 三个线程
// 一个线程打印1
// 一个线程打印2
// 一个线程打印3
// 打印n次 - 每个线程打印n次，就是每个线程执行n次

/*
    在一个循环内控制打印次数
    使用三个信号量 控制线程的执行顺序

    打印数字1的先执行
    每个线程各个打印各自的数字

    一个线程释放另一个线程等待的信号
    例如，线程a，b，c
    a-》b-》c——》a这样子

*/

using System;
using System.Threading;


namespace print_num_alpha_sln
{
    class Program
    {
        private static Semaphore sema1 = new Semaphore(1, 1);
        private static Semaphore sema2 = new Semaphore(0, 1);
        private static Semaphore sema3 = new Semaphore(0, 1); 
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ParameterizedThreadStart(print1));
            Thread t2 = new Thread(new ParameterizedThreadStart(print2));
            Thread t3 = new Thread(new ParameterizedThreadStart(print3));
            t1.Start(5);
            t2.Start(5);
            t3.Start(5);

            t1.Join();
            t2.Join();
            t3.Join();

            Console.WriteLine("Hello World!");
        }

        static void print1(object cnt)
        {
            for(int i = 0 ; i < (int)cnt; i++)
            {
                sema1.WaitOne();
                Console.WriteLine("1");
                sema2.Release();
                //sema1.Release(); 一定不要主动release对应信号量的信号灯，这样的话，后面就是运行不可控了
            } 
        }
        static void print2(object cnt)
        {
            for(int i = 0 ; i < (int)cnt; i++)
            {
                sema2.WaitOne();
                Console.WriteLine("2");
                sema3.Release();
                // sema2.Release(); // 同上sema1.release注释
            }
        }
        
        static void print3(object cnt)
        {
            for(int i = 0 ; i < (int)cnt; i++)
            {
                sema3.WaitOne();
                Console.WriteLine("3");
                sema1.Release();
            }
        }
    }
}
