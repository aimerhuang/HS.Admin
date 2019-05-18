using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hyt.Service.Task.Core
{
    /// <summary>
    /// 任务执行队列 
    /// </summary>
    /// <remarks>2013-11-11 杨浩 添加</remarks>
    public static class TaskQueue
    {
        /// <summary>
        /// 相同的任务类型加入同一个队列
        /// </summary>
        private static Dictionary<string, Queue<TaskQueueWrap>> _workQueue;
        /// <summary>
        /// 任务类型的执行状态
        /// </summary>
        private static Dictionary<string, bool> _workStatus;
        /// <summary>
        /// 任务类型的等待标识
        /// </summary>
        private static Dictionary<string, bool> _waitStatus;
        /// <summary>
        /// 静态锁对象
        /// </summary>
        private static object lockobj = new object();
        /// <summary>
        /// 最大等待队列
        /// </summary>
        private static int _maxQueue = 20;

        /// <summary>
        /// 初始化
        /// </summary>
        static TaskQueue()
        {
            _workQueue = new Dictionary<string, Queue<TaskQueueWrap>>();
            _workStatus = new Dictionary<string, bool>();
            _waitStatus = new Dictionary<string, bool>();
            var thread = new Thread(Start);
            thread.Start();
        }

        /// <summary>
        /// 启动工作线程
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>2013-11-11 杨浩 添加</remarks>
        private static void Start()
        {
            while (true)
            {
                lock (lockobj)
                {
                    if (_workQueue.Count > 0)
                    {
                        foreach (var item in _workQueue)
                        {
                            var obj = _workQueue[item.Key];
                            Add(item.Key, obj);
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 加入工作队列到执行线程
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="queue">任务执行队列</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-11 杨浩 添加</remarks>
        private static void Add(string taskType, Queue<TaskQueueWrap> queue)
        {
            if (queue.Count == 0)
                return;
            if (!_workStatus.ContainsKey(taskType))
            {
                //设置为正在执行
                _workStatus[taskType] = true;
                var t = new Thread(() => Execute(taskType, queue)) {Name = taskType, IsBackground = true};
                t.Start();
            }
            else
            {
                if (!_workStatus[taskType])
                {
                    //设置为正在执行
                    _workStatus[taskType] = true;
                    var t = new Thread(() => Execute(taskType, queue)) {Name = taskType, IsBackground = true};
                    t.Start();
                }
                //else
                //{
                //    if (!_waitStatus.ContainsKey(taskType))
                //    {
                //        //排队标识
                //        _waitStatus[taskType] = true;
                //        var t = new Thread(() =>
                //            {
                //                //阻塞线程使其相同时刻不能执行相同任务
                //                while (_workStatus[taskType])
                //                {
                //                    Thread.Sleep(200);
                //                }
                //                //设置为正在执行
                //                _workStatus[taskType] = true;
                //                //执行任务
                //                Execute(taskType, queue);
                //                //执行完成后移除排队标识
                //                _waitStatus.Remove(taskType);
                //            });
                //        t.Name = taskType;
                //        t.IsBackground = true;
                //        t.Start();
                //    }
                //}
            }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="queue">任务执行队列</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-11 杨浩 添加</remarks>
        private static void Execute(string taskType, Queue<TaskQueueWrap> queue)
        {
            while (queue.Count>0)
            {
                var work = queue.Dequeue();
                work.TaskWorkItem(work.Contextdata);
            }
            //清除正在执行状态
            _workStatus[taskType] = false;
        }

        /// <summary>
        /// 添加工作委托的方法
        /// </summary>
        /// <param name="taskItem">表示线程池线程要执行的回调方法</param>
        /// <param name="taskType">设置线程名</param>
        /// <param name="state">包含回调方法要使用的信息的对象</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-11 杨浩 添加</remarks>
        public static void AddTaskItem(WaitCallback taskItem, string taskType, object state = null)
        {
            lock (lockobj)
            {
                var isExist = _workQueue.ContainsKey(taskType);
                var wrap = new TaskQueueWrap
                    {
                        Contextdata = state,
                        TaskWorkItem = taskItem
                    };
                if (isExist)
                {
                    var obj = _workQueue[taskType];
                    if (obj.Count < _maxQueue)
                        obj.Enqueue(wrap);
                }
                else
                {
                    var obj = new Queue<TaskQueueWrap>();
                    obj.Enqueue(wrap);
                    _workQueue.Add(taskType, obj);
                }
            }
        }
        
        /// <summary>
        /// 重置
        /// </summary>
        /// <remarks>2013-11-11 杨浩 添加</remarks>
        public static void Reset()
        {
            lock (lockobj)
            {
                if (_workQueue.Count > 0)
                {
                    foreach (var item in _workQueue)
                    {
                        _workQueue[item.Key].Clear();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 执行对象
    /// </summary>
    /// <remarks>2013-11-11 杨浩 添加</remarks>
    public class TaskQueueWrap
    {
        /// <summary>
        /// 线程体委托
        /// </summary>
        public WaitCallback TaskWorkItem;

        /// <summary>
        /// 委托状态数据
        /// </summary>
        public object Contextdata { get; set; }
    }

}
