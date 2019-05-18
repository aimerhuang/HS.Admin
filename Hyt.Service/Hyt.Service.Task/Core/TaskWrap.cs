using System;
using System.Linq;
using Hyt.Model;

namespace Hyt.Service.Task.Core
{
    /// <summary>
    /// 任务处理及包装
    /// </summary>
    /// <remarks>2013-10-10 杨浩 添加</remarks>
    public class TaskWrap 
    {
        /// <summary>
        /// 任务配置信息
        /// </summary>
        public SyTaskConfig Task { get; private set; }

        /// <summary>
        /// 执行时间刻度数
        /// </summary>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        public TimeSpan ExecuteTime 
        {
            get { return new TimeSpan(Int64.Parse(this.Task.ExecuteTime)); }
        }

        /// <summary>
        /// 开始时间段
        /// </summary>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        public TimeSpan StartTimeQuantum 
        {
            get
            {
                if (string.IsNullOrEmpty(Task.TimeQuantum))
                    return new TimeSpan(0,0,0);
                try
                {
                    long titks = long.Parse(Task.TimeQuantum.Split(',')[0]);
                    return new TimeSpan(titks);
                }
                catch
                {
                    return new TimeSpan(0, 0, 0);
                }
            }
        }

        /// <summary>
        /// 结束时间段
        /// </summary>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        public TimeSpan EndTimeQuantum 
        {
            get
            {
                if (string.IsNullOrEmpty(Task.TimeQuantum))
                    return new TimeSpan(23,59,59);
                try
                {
                    long titks = long.Parse(Task.TimeQuantum.Split(',')[1]);
                    return new TimeSpan(titks);
                }
                catch
                {
                    return new TimeSpan(23, 59, 59);
                }
            }
        }

        /// <summary>
        /// 初始化任务配置
        /// </summary>
        /// <param name="model"></param>
        public TaskWrap(SyTaskConfig model)
        {
            this.Task = model;
        }

        /// <summary>
        /// 创建任务类型实例
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        private ITask CreateTask ()
        {
            ITask task = null;
            if (Task.Status==1)
            {
                Type type = Type.GetType(Task.TypeName);
                if (type == null)
                {
                    Hyt.Util.Log.LogManager.Instance.WriteLog(string.Format("任务 {0} 无法被正确识别，检查此任务是否已经实现接口ITask", Task.TypeName));
                }
                else
                    task = Activator.CreateInstance(type) as ITask;
            }
            return task;
        }

        /// <summary>
        /// 执行任务的方法
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        public void Execute(object state)
        {
            var config = Hyt.BLL.Sys.SyTaskBo.Instance.GetTask(Task.SysNo);
            if (config.Status == 0)
                return;
            try
            {
                var task = this.CreateTask();
                if (task != null)
                {
                    //正在执行
                    //config.LastMessage = "正在执行";
                    //Hyt.BLL.Sys.SyTaskBo.Instance.UpdateTask(config);

                    DateTime startTime = DateTime.Now;
                    //更新任务最后一次执行的时间
                    config.LastExecuteTime = startTime;
                    Task.LastExecuteTime = startTime;
                   
                    //执行任务
                    task.Execute(state);

                    var lastTask= Hyt.BLL.Sys.SyTaskBo.Instance.GetTask(Task.SysNo);
                    config.Contextdata = lastTask.Contextdata;
                    config.Status = lastTask.Status;
                    DateTime endTime = DateTime.Now;

                    var log = new SyTaskLog
                        {
                            CreateTime = DateTime.Now,
                            ExecuteEndTime = endTime,
                            ExecuteStartTime = startTime,
                            Status = 1,
                            TaskConfigSysNo = Task.SysNo,
                            ExecuteMessage = "执行成功"
                        };
                    config.LastMessage = log.ExecuteMessage;
                    Hyt.BLL.Sys.SyTaskBo.Instance.AddTaskLog(log);
                }
            }
            catch (Exception e)
            {
                config.FailureCount += 1;
                Task.FailureCount = config.FailureCount;

                var log = new SyTaskLog
                    {
                        CreateTime = DateTime.Now,
                        ExecuteEndTime = DateTime.Now,
                        ExecuteStartTime = DateTime.Now,
                        Status = 0,
                        TaskConfigSysNo = Task.SysNo,
                        ExecuteMessage = "执行失败:" + e.Message
                    };

                config.LastMessage = (config.IsAgain == 1 && config.FailureCount >= config.MaxAgainCount)
                                         ? "执行失败:已达最大失败次数"
                                         : log.ExecuteMessage;
                Hyt.Util.Log.LogManager.Instance.WriteLog(@"E:\Pisen\Hyt\log\erp\exception", "日志编号:" + config.SysNo + ",Exception:" + e.Message + ";" + e.Source + ";" + e.StackTrace);
                Hyt.BLL.Sys.SyTaskBo.Instance.AddTaskLog(log);
            }
            finally
            {
                Hyt.BLL.Sys.SyTaskBo.Instance.UpdateTask(config);
            }
        }

        #region old Method

        /// <summary>
        /// 设置执行时间
        /// TimeType为Interval时 SetTime()里面调用SetIntervalSeconds(long seconds)；
        /// TimeType为Hour    时 SetTime()里面调用SetHourExecuteTime(int minutes,int seconds)；
        /// TimeType为Day     时 SetTime()里面调用SetDayExecuteTime(int hours, int minutes, int seconds)；
        /// TimeType为Week    时 SetTime()里面调用SetWeekExecuteTime(DayOfWeek dayOfWeek,int hours, int minutes, int seconds)；
        /// TimeType为Month   时 SetTime()里面调用SetMonthExecuteTime(int day,int hours, int minutes, int seconds)；
        /// TimeType为Year    时 SetTime()里面调用SetYearExecuteTime(int month,int day, int hours, int minutes, int seconds)；
        /// </summary>
        protected void SetTime()
        {

        }

        /// <summary>
        /// TimeType为Interval时 在SetTime()中请调用此方法 设置执行时间,只支持一个月的时间
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        protected void SetIntervalExecuteTime(long seconds)
        {
            if (seconds <= 60)
            {
                Task.ExecuteTime = new TimeSpan(0, 0, (int)seconds).Ticks.ToString();
            }
            else if (seconds <= 60 * 60) //小于等于60分钟
            {
                SetHourExecuteTime((int)(seconds / 60), (int)(seconds % 60));
            }
            else if (seconds <= 24 * 60 * 60) //一天
            {
                SetDayExecuteTime((int)(seconds / 60 * 60), (int)(seconds % 60 * 60), (int)(seconds % 60));
            }
            else if (seconds <= 31 * 24 * 60 * 60) //一个月
            {
                SetMonthExecuteTime((int)(seconds / 24 * 60 * 60), (int)(seconds / 60 * 60), (int)(seconds % 60 * 60), (int)(seconds % 60));
            }
            else
                Task.ExecuteTime ="0";
        }

        /// <summary>
        /// TimeType为Hour时 在SetTime()中请调用此方法 设置执行时间
        /// </summary>
        /// <param name="minutes">分</param>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        protected void SetHourExecuteTime(int minutes, int seconds)
        {
            if (minutes > 59)
                minutes = 59;
            if (seconds > 59)
                seconds = 59;

            Task.ExecuteTime = new TimeSpan(0, minutes, seconds).Ticks.ToString();
        }

        /// <summary>
        /// TimeType为Day时 在SetTime()中请调用此方法 设置执行时间
        /// </summary>
        /// <param name="hours">时</param>
        /// <param name="minutes">分</param>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        protected void SetDayExecuteTime(int hours, int minutes, int seconds)
        {
            if (hours > 23)
                hours = 23;
            if (minutes > 59)
                minutes = 59;
            if (seconds > 59)
                seconds = 59;

            Task.ExecuteTime = new TimeSpan(hours, minutes, seconds).Ticks.ToString();
        }

        /// <summary>
        /// TimeType为Week时 在SetTime()中请调用此方法 设置执行时间
        /// </summary>
        /// <param name="dayOfWeek">周</param>
        /// <param name="hours">时</param>
        /// <param name="minutes">分</param>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        protected void SetWeekExecuteTime(DayOfWeek dayOfWeek, int hours, int minutes, int seconds)
        {
            Task.DayOfWeek = ((int)dayOfWeek).ToString();
            SetDayExecuteTime(hours, minutes, seconds);
        }

        /// <summary>
        /// TimeType为Month时 在SetTime()中请调用此方法 设置执行时间
        /// </summary>
        /// <param name="day">天</param>
        /// <param name="hours">时</param>
        /// <param name="minutes">分</param>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        protected void SetMonthExecuteTime(int day, int hours, int minutes, int seconds)
        {
            if (day > 31)
                day = 31;
            if (hours > 23)
                hours = 23;
            if (minutes > 59)
                minutes = 59;
            if (seconds > 59)
                seconds = 59;

            Task.ExecuteTime = new TimeSpan(day, hours, minutes, seconds).Ticks.ToString();
        }

        /// <summary>
        /// TimeType为Year时 在SetTime()中请调用此方法 设置执行时间
        /// </summary>
        /// <param name="month">周</param>
        /// <param name="day">天</param>
        /// <param name="hours">时</param>
        /// <param name="minutes">分</param>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        protected void SetYearExecuteTime(int month, int day, int hours, int minutes, int seconds)
        {
            if (month > 12)
                month = 12;
            if (day > 31)
                day = 31;
            if (hours > 23)
                hours = 23;
            if (minutes > 59)
                minutes = 59;
            if (seconds > 59)
                seconds = 59;
            Task.Month = month.ToString();
            Task.ExecuteTime = new TimeSpan(day, hours, minutes, seconds).Ticks.ToString();
        }

        #endregion
    }
}
