using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    /// <summary>
    /// 基于阻塞队列的生产者消费者C#并发设计
    ///这是从上文的<<图文并茂的生产者消费者应用实例demo>>整理总结出来的，具体就不说了，直接给出代码，注释我已经加了，原来的code请看<<.Net中的并行编程-7.基于BlockingCollection实现高性能异步队列>>，我改成适合我的版本了，直接给code：
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProcessQueue<T>
    {
        private BlockingCollection<T> _queue;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellToken;
        //内部线程池
        private List<Thread> _threadCollection;

        //队列是否正在处理数据
        private int _isProcessing;
        //有线程正在处理数据
        private const int Processing = 1;
        //没有线程处理数据
        private const int UnProcessing = 0;
        //队列是否可用   单个线程下用while来判断，多个线程下用if判断，随后while循环队列的数量
        private volatile bool _enabled = true;
        //内部处理线程数量
        private int _internalThreadCount;
        // 消费者处理事件
        public event Action<T> ProcessItemEvent;
        //处理异常，需要三个参数，当前队列实例，异常，当时处理的数据
        public event Action<dynamic, Exception, T> ProcessExceptionEvent;

        public ProcessQueue()
        {
            _queue = new BlockingCollection<T>();
            _cancellationTokenSource = new CancellationTokenSource();
            _internalThreadCount = 3;
            _cancellToken = _cancellationTokenSource.Token;
            _threadCollection = new List<Thread>();
        }

        public ProcessQueue(int internalThreadCount)
            : this()
        {
            this._internalThreadCount = internalThreadCount;
        }

        /// <summary>
        /// 队列内部元素的数量 
        /// </summary>
        public int GetInternalItemCount()
        {
            //return _queue.Count;
            return _threadCollection.Count;
        }
        //生产者生产
        public void Enqueue(T items)
        {
            if (items == null)
            {
                throw new ArgumentException("items");
            }

            _queue.Add(items);
            DataAdded();
        }

        public void Flush()
        {
            StopProcess();

            while (_queue.Count != 0)
            {
                T item = default(T);
                if (_queue.TryTake(out item))
                {
                    try
                    {
                        ProcessItemEvent(item);
                    }
                    catch (Exception ex)
                    {
                        OnProcessException(ex, item);
                    }
                }
            }
        }
        // 通知消费者消费队列元素
        private void DataAdded()
        {
            if (_enabled)
            {
                if (!IsProcessingItem())
                {
                    Console.WriteLine("DataAdded");
                    ProcessRangeItem();
                    StartProcess();
                }
            }
        }

        //判断是否队列有线程正在处理 
        private bool IsProcessingItem()
        {
            // 替换第一个参数, 如果相等
            //int x = Interlocked.CompareExchange(ref _isProcessing, Processing, UnProcessing);
            return !(Interlocked.CompareExchange(ref _isProcessing, Processing, UnProcessing) == UnProcessing);
        }
        // 多消费者消费
        private void ProcessRangeItem()
        {
            for (int i = 0; i < this._internalThreadCount; i++)
            {
                ProcessItem();
            }
        }
        // 开启消费处理
        private void ProcessItem()
        {
            Thread currentThread = new Thread((state) =>
            {
                T item = default(T);
                while (_enabled)
                {
                    try
                    {
                        try
                        {
                            if (!_queue.TryTake(out item))
                            {
                                Console.WriteLine("阻塞队列为0时的item: {0}", item);
                                Console.WriteLine("ok!!!");
                                break;
                            }
                            // 处理事件
                            ProcessItemEvent(item);
                        }
                        catch (OperationCanceledException ex)
                        {
                            //DebugHelper.DebugView(ex.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        OnProcessException(ex, item);
                    }
                }
            });
            _threadCollection.Add(currentThread);
        }
        // 开启消费者
        private void StartProcess()
        {
            //Console.WriteLine("线程的数量: {0}", _threadCollection.Count);
            foreach (var thread in _threadCollection)
            {
                thread.Start();
                thread.IsBackground = true;
            }
        }
        // 终止运行
        private void StopProcess()
        {
            this._enabled = false;
            foreach (var thread in _threadCollection)
            {
                if (thread.IsAlive)
                {
                    thread.Join();
                }
            }
            _threadCollection.Clear();
        }

        private void OnProcessException(Exception ex, T item)
        {
            var tempException = ProcessExceptionEvent;
            Interlocked.CompareExchange(ref ProcessExceptionEvent, null, null);

            if (tempException != null)
            {
                ProcessExceptionEvent(this, ex, item);
            }
        }

    }
}

