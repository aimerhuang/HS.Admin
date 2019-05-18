using System;
using System.Collections.Generic;
using System.Threading;

namespace Hyt.Service.Task.Core
{
    /// <summary>
    /// 线程池管理
    /// </summary>
    /// <remarks>2013-10-10 杨浩 添加</remarks>
    [Obsolete("此类作废不启用")]
    public class TaskPool : IDisposable
    {
        private int max = 10; //最大线程数
        private int min = 1;  //最小线程数
        private int increment = 1; //当活动线程不足的时候新增线程的增量

        private Dictionary<string, TreadWrap> publicpool; //所有的线程
        private Queue<TreadWrap> freequeue;  //空闲线程队列
        private Dictionary<string, TreadWrap> working;   //正在工作的线程
        private Queue<Waititem> waitlist;  //等待执行工作队列

        /// <summary>
        /// 唯一实例入口
        /// </summary>
        private static readonly TaskPool _instance = new TaskPool();
        public static TaskPool Instance 
        {
            get { return _instance; }
        }
        
        //设置最大线程数
        public void Setmaxthread(int Value)
        {
            lock (this)
            {
                max = Value;
            }
        }
        //设置最小线程数
        public void Setminthread(int Value)
        {
            lock (this)
            {
                min = Value;
            }
        }
        //设置增量
        public void Setincrement(int Value)
        {
            lock (this)
            {
                increment = Value;
            }
        }
        //初始化线程池
        private TaskPool()
        {
            publicpool = new Dictionary<string, TreadWrap>();
            working = new Dictionary<string, TreadWrap>();
            freequeue = new Queue<TreadWrap>();
            waitlist = new Queue<Waititem>();
            TreadWrap t = null;
            //先创建最小线程数的线程
            for (int i = 0; i < min; i++)
            {
                t = new TreadWrap();
                //注册线程完成时触发的事件
                t.WorkComplete += new Action<TreadWrap>(t_WorkComplete);
                //加入到所有线程的字典中
                publicpool.Add(t.Key, t);
                //因为还没加入具体的工作委托就先放入空闲队列
                freequeue.Enqueue(t);
            }
        }
        //线程执行完毕后的触发事件
        void t_WorkComplete(TreadWrap obj)
        {
            lock (this)
            {
                //首先因为工作执行完了，所以从正在工作字典里删除
                working.Remove(obj.Key);
                //检查是否有等待执行的操作，如果有等待的优先执行等待的任务
                if (waitlist.Count > 0)
                {
                    //先要注销当前的线程，将其从线程字典删除
                    publicpool.Remove(obj.Key);
                    obj.Close();
                    //从等待任务队列提取一个任务
                    Waititem item = waitlist.Dequeue();
                    TreadWrap nt = null;
                    //如果有空闲的线程，就是用空闲的线程来处理
                    if (freequeue.Count > 0)
                    {
                        nt = freequeue.Dequeue();
                    }
                    else
                    {
                        //如果没有空闲的线程就再新创建一个线程来执行
                        nt = new TreadWrap();
                        publicpool.Add(nt.Key, nt);
                        nt.WorkComplete += new Action<TreadWrap>(t_WorkComplete);
                    }
                    //设置线程的执行委托对象和上下文对象
                    nt.TaskWorkItem = item.Works;
                    nt.Contextdata = item.Context;
                    //添加到工作字典中
                    working.Add(nt.Key, nt);
                    //唤醒线程开始执行
                    nt.Active();
                }
                else
                {
                    //如果没有等待执行的操作就回收多余的工作线程
                    if (freequeue.Count > min)
                    {
                        //当空闲线程超过最小线程数就回收多余的这一个
                        publicpool.Remove(obj.Key);
                        obj.Close();
                    }
                    else
                    {
                        //如果没超过就把线程从工作字典放入空闲队列
                        obj.Contextdata = null;
                        freequeue.Enqueue(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 添加工作委托的方法
        /// </summary>
        /// <param name="taskItem">表示线程池线程要执行的回调方法</param>
        /// <param name="threadName">设置线程名</param>
        /// <param name="state">包含回调方法要使用的信息的对象</param>
        public void AddTaskItem(WaitCallback taskItem,string threadName=null, object state=null)
        {
            lock (this)
            {
                TreadWrap t = null;
                int len = publicpool.Values.Count;
                //如果线程没有到达最大值
                if (len < max)
                {
                    //如果空闲列表非空
                    if (freequeue.Count > 0)
                    {
                        //从空闲队列pop一个线程
                        t = freequeue.Dequeue();
                        //加入工作字典
                        working.Add(t.Key, t);
                        //设置执行委托
                        t.TaskWorkItem = taskItem;
                        //设置状态对象
                        t.Contextdata = state;
                        //唤醒线程开始执行
                        t.Active();
                        return;
                    }
                    else
                    {
                        //如果没有空闲队列了，就根据增量创建线程
                        for (int i = 0; i < increment; i++)
                        {
                            //判断线程的总量不能超过max
                            if ((len + i) <= max)
                            {
                                t = new TreadWrap();
                                //设置完成响应事件
                                t.WorkComplete += new Action<TreadWrap>(t_WorkComplete);
                                //加入线程字典
                                publicpool.Add(t.Key, t);
                                //加入空闲队列
                                freequeue.Enqueue(t);
                            }
                            else
                            {
                                break;
                            }
                        }
                        //从空闲队列提出出来设置后开始执行
                        t = freequeue.Dequeue();
                        working.Add(t.Key, t);
                        t.TaskWorkItem = taskItem;
                        t.Contextdata = state;
                        t.Active();
                        return;
                    }
                }
                else
                {
                    //如果线程达到max就把任务加入等待队列
                    waitlist.Enqueue(new Waititem() { Context = state, Works = taskItem });
                }
            }
        }

        //回收资源
        public void Dispose()
        {
            //throw new NotImplementedException();
            foreach (TreadWrap t in publicpool.Values)
            {
                //关闭所有的线程
                using (t) { t.Close(); }
            }
            publicpool.Clear();
            working.Clear();
            waitlist.Clear();
            freequeue.Clear();
        }
        //存储等待队列的类
        class Waititem
        {
            public WaitCallback Works { get; set; }
            public object Context { get; set; }
        }
    }
    //线程包装器类
    class TreadWrap : IDisposable
    {
        private AutoResetEvent locks; //线程锁
        private Thread T;  //线程对象
        public WaitCallback TaskWorkItem; //线程体委托
        private bool _working;  //线程是否工作
        public object Contextdata
        {
            get;
            set;
        }
        public event Action<TreadWrap> WorkComplete;  //线程完成一次操作的事件
        //用于字典的Key
        public string Key
        {
            get;
            set;
        }
        //初始化包装器
        public TreadWrap()
        {
            //设置线程一进入就阻塞
            locks = new AutoResetEvent(false);
            Key = Guid.NewGuid().ToString();
            //初始化线程对象
            T = new Thread(Work);
            T.Name = Key;
            T.IsBackground = true;
            _working = true;
            Contextdata = new object();
            //开启线程
            T.Start();
        }
        //唤醒线程
        public void Active()
        {
            locks.Set();
        }
        //设置执行委托和状态对象
        public void SetWorkItem(WaitCallback action, object context)
        {
            TaskWorkItem = action;
            Contextdata = context;
        }
        //线程体包装方法
        private void Work()
        {
            while (_working)
            {
                try
                {
                    //阻塞线程
                    locks.WaitOne();
                    TaskWorkItem(Contextdata);
                    //完成一次执行，触发事件
                    WorkComplete(this);
                }
                catch (ThreadAbortException e)
                {
                    //TODO:异常处理
                }
            }
        }
        //关闭线程
        public void Close()
        {
            _working = false;
        }
        //回收资源
        public void Dispose()
        {
            //throw new NotImplementedException();
            try
            {
                T.Abort();
            }
            catch { }
        }
    }
}