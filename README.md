# print_odd_even_sln
两个线程 - 交替打印奇偶数 - 使用c# monitor wait ，pulse实现的有锁版本


## 简单描述一下 -- 基于线程-等待-唤醒机制的sln

1. 首先实现两个方法，一个用于打印某一范围内的偶数，另一个方法用于打印奇数
2. 然后利用monitor提供的wait，pulse在需要自己处理的地方处理，在不需要自己处理的时候主动阻塞，进入锁对象的wait queues，然后发送pulse脉冲信号，告知锁状态，然后让处在锁对象wait queues
   的线程进入ready queues，这个ready queues是能够获得锁、被os调度的，wait queues内的线程不能，且只能通过pulse的方式进入ready queues
3. 这样，通过step2就实现了一个类似于通知的结构， 因为只有两个线程，在运行后，必然一个处于等待，一个处于running，就这样通过脉冲的方式 将 机会 抛来抛去

假设： 线程a -- 奇数 线程b -- 偶数 锁 obj


线程b运行，首先处理 数字 1， 它是奇数，应该有线程a处理，那么先发送pulse，然后wait主动阻塞，进入wait queues，因为只有两个线程，线程b主动阻塞自己，进入waitsleepjoin，那么，可调度的线程就剩下一个a了，它肯定就会获得锁并进入临界区执行，同理，当线程a遇到需要线程b处理的数字时，线程a也会发送pulse，然后wait阻塞自己，进入waitsleepjoin，让线程b执行，这样执行到最后的数字时，假设是5，假设此时在线程b偶数里面，那么线程b按照之前的描述进入wait queues，但是线程a处理完后，因为数字处理循环条件已经结束了，也就是超过数字范围了，它会结束，而临界区加在了这个for-range，那么，它就会结束线程，而另一个处于wait queues内的线程还在苦苦等待线程a传来的pulse，但是等不到了，线程a结束运行了，契约说崩就崩了，这就造成了一个dead lock， 要解决只能在for-range范围之外，再加一个monitor.pulse(obj)，说好的就要做到，不能让别人苦等啊


-------------------------

二进制信号量版本 


1. 使用两个信号量， 一个sema-odd，一个sema-even
2. 两个方法，一个打印odd，一个打印even
3. 初始sema-odd（1， 1）， sema-even（0， 1），这样odd先执行
4. odd先执行后， 最后再将sema-even的信号量有效量 释放一个，以使even得一执行