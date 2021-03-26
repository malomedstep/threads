using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;


namespace ProcessesThreads {
    // public class Server {
    //     public ConcurrentBag<string> Players { get; set; } 
    // }


    public static class LockExample1 {
        private static Queue<int> _queue = new Queue<int>();

        private static object sync1 = new object(); // 

        public static void ThreadFunction1() {
            var rand = new Random();
            while (true) {
                lock (sync1) {
                    _queue.Enqueue(rand.Next());
                }

                // var taken = Monitor.TryEnter(sync1);
                // try {
                //     _queue.Enqueue(rand.Next());  
                // }
                // finally {
                //     if (taken) {
                //         Monitor.Exit(sync1);
                //     }
                // }


                Thread.Sleep(10);
            }
        }

        public static void ThreadFunction2() {
            var rand = new Random();
            while (true) {
                lock (sync1) {
                    _queue.Enqueue(rand.Next());
                }

                Thread.Sleep(10);
            }
        }

        public static void ThreadFunction3() {
            while (true) {
                lock (sync1) {
                    if (_queue.Count != 0) {
                        _queue.Dequeue();
                    }
                }

                Thread.Sleep(15);
            }
        }


        static Mutex mutex; // mutual exclusion
        
        static void Run() {
            mutex = new Mutex(false, "sadfsad");
            var t1 = new Thread(ThreadFunction1);
            var t2 = new Thread(ThreadFunction2);
            var t3 = new Thread(ThreadFunction3);
            t1.Start();
            t2.Start();
            t3.Start();

            Console.ReadLine();

            // var srv = new Server();
            // srv.Players.Add("shalom");
        }
    }

    public static class LockExample2 {
        private static long _counter;
        private static readonly object sync = new object();
        private static void ThreadFunc() {
            for (var i = 0; i < 100000000; i++) {
                lock (sync) {
                    _counter++;
                }
            }
        }
        
        public static void Run() {
            var t1 = new Thread(ThreadFunc);
            var t2 = new Thread(ThreadFunc);
            var sw = new Stopwatch();
            sw.Start();
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            sw.Stop();
            Console.WriteLine(_counter);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
    
    public static class MutexExample2 {
        private static long _counter;
        private static Mutex _mutex = new Mutex();
        private static void ThreadFunc() {
            for (var i = 0; i < 100000000; i++) {
                _mutex.WaitOne();
                _counter++;
                _mutex.ReleaseMutex();
            }
        }
        
        public static void Run() {
            var t1 = new Thread(ThreadFunc);
            var t2 = new Thread(ThreadFunc);
            var sw = new Stopwatch();
            sw.Start();
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            sw.Stop();
            Console.WriteLine(_counter);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
    public class Program {
        static void Main(string[] args) {
            MutexExample2.Run();
        }
    }
}