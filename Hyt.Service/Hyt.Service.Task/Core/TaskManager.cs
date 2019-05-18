using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Service.Task.Core
{
    /// <summary>
    /// 任务计划管理
    /// </summary>
    /// <remarks>2013-10-10 杨浩 添加</remarks>
    public class TaskManager
    {
        //任务列表
        private readonly static List<TaskWrap> _allTasks = new List<TaskWrap>();
        //单例
        private readonly static TaskManager _instance = new TaskManager();
        public static TaskManager Instance 
        {
            get { return _instance; }
        }

        /// <summary>
        /// 获取所有任务列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private IList<SyTaskConfig> AllTask
        {
            get { return Hyt.BLL.Sys.SyTaskBo.Instance.GetAll(); }
        }

        /// <summary>
        /// 重新加载所有任务
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        public void Reload()
        {
            _allTasks.Clear();
            foreach (SyTaskConfig taskConfig in AllTask)
            {
                var taskbase = new TaskWrap(taskConfig);

                _allTasks.Add(taskbase);
            }
        }

        /// <summary>
        /// 初始化加载所有任务
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        public void Init()
        {
            Reload();

            StartTaskThread();
        }

        /// <summary>
        /// 开始线程执行所有任务
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private  void StartTaskThread()
        {
            var thread = new System.Threading.Thread(InThreadTaskWorker)
                {
                    Name = "任务计划主线程",
                    IsBackground = true
                };
            thread.Start();
           // Hyt.Util.Log.LogManager.Instance.WriteLog(string.Format("后台任务计划线程开始启动,{0}", DateTime.Now.ToString()));
        }

        /// <summary>
        /// 执行线程
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private  void InThreadTaskWorker()
        {
            while (true)
            {
                try
                {
                    ExecuteInThreadTasks();
                }
                catch (Exception e)
                {
                    Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, e.Message, LogStatus.系统日志目标类型.用户, Model.SystemPredefined.User.SystemUser, e);
                }

                System.Threading.Thread.Sleep(200);
            }
        }

        /// <summary>
        /// 获取是否阻塞侦听
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private bool GetMessage()
        {
            bool result;
            if (_allTasks == null)
                result = false;
            else
            {
                result = _allTasks.Any(x => x.Task.Status == 1);
            }
            return result;
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private static void ExecuteInThreadTasks()
        {
            foreach (TaskWrap task in _allTasks)
            {
                if (task.Task.Status==0)
                    continue;

                if (IsExecuteTime(task))
                    ExecuteTask(task);
            }
        }

        /// <summary>
        /// 执行单个任务
        /// </summary>
        /// <param name="tw">当前任务</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private static void ExecuteTask(TaskWrap tw)
        {
            //任务加入线程池(此策略作废)
            //TaskPool.Instance.AddTaskItem(tw.Execute, tw.Task.TaskName, null);

            //任务加入线程池队列
            TaskQueue.AddTaskItem(tw.Execute, tw.Task.TypeName,tw.Task.Contextdata);
        }

        /// <summary>
        /// 检查任务是否到了可以执行的时间
        /// </summary>
        /// <param name="tw">当前任务</param>
        /// <returns>bool</returns>
        /// <remarks>2013-10-10 杨浩 添加</remarks>
        private static bool IsExecuteTime(TaskWrap tw)
        {
            //判断是否已达最大失败次数
            if (tw.Task.IsAgain == 1)
            {
                if (tw.Task.FailureCount >= tw.Task.MaxAgainCount)
                {
                    //禁用任务
                    var index = _allTasks.IndexOf(tw);
                    _allTasks[index].Task.Status = 0;
                    return false;
                }
            }
            DateTime executeTime;
            switch (tw.Task.Timetype)
            {
                case (int) TaskTimeType.Interval:
                    //判断执行时间段
                    var now = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    if (tw.StartTimeQuantum <= now && now <= tw.EndTimeQuantum)
                    {
                        if (tw.Task.LastExecuteTime.AddSeconds(tw.ExecuteTime.TotalSeconds) <= DateTime.Now)
                            return true;
                    }
                    break;
                case (int) TaskTimeType.Hour:
                    executeTime = new DateTime(tw.Task.LastExecuteTime.Year, tw.Task.LastExecuteTime.Month,
                                               tw.Task.LastExecuteTime.Day, tw.Task.LastExecuteTime.Hour,
                                               tw.ExecuteTime.Minutes,
                                               tw.ExecuteTime.Seconds);
                    if (executeTime.AddHours(1) <= DateTime.Now)
                        return true;
                    break;
                case (int) TaskTimeType.Day:
                    executeTime = new DateTime(tw.Task.LastExecuteTime.Year, tw.Task.LastExecuteTime.Month,
                                               tw.Task.LastExecuteTime.Day, tw.ExecuteTime.Hours,
                                               tw.ExecuteTime.Minutes,
                                               tw.ExecuteTime.Seconds);
                    if (executeTime.AddDays(1) < DateTime.Now)
                        return true;
                    break;
                case (int) TaskTimeType.Week:
                    if (DateTime.Now.DayOfWeek == (DayOfWeek) int.Parse(tw.Task.DayOfWeek))
                    {
                        if (tw.Task.LastExecuteTime.Year == DateTime.Now.Year &&
                            tw.Task.LastExecuteTime.Month == DateTime.Now.Month &&
                            tw.Task.LastExecuteTime.Day == DateTime.Now.Day)
                        {
                        }
                        else
                        {
                            executeTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                       tw.ExecuteTime.Hours,
                                                       tw.ExecuteTime.Minutes,
                                                       tw.ExecuteTime.Seconds);
                            if (executeTime <= DateTime.Now)
                                return true;
                        }
                    }
                    break;
                case (int) TaskTimeType.Month:
                    executeTime = new DateTime(tw.Task.LastExecuteTime.Year, tw.Task.LastExecuteTime.Month,
                                               tw.ExecuteTime.Days,
                                               tw.ExecuteTime.Hours,
                                               tw.ExecuteTime.Minutes,
                                               tw.ExecuteTime.Seconds);
                    if (executeTime.AddMonths(1) < DateTime.Now)
                        return true;
                    break;
                case (int) TaskTimeType.Year:
                    executeTime = new DateTime(tw.Task.LastExecuteTime.Year, int.Parse(tw.Task.Month),
                                               tw.ExecuteTime.Days,
                                               tw.ExecuteTime.Hours,
                                               tw.ExecuteTime.Minutes,
                                               tw.ExecuteTime.Seconds);
                    if (executeTime.AddYears(1) < DateTime.Now)
                        return true;
                    break;
            }
            return false;
        }
    }
}
