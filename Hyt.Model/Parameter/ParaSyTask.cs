using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 任务配置
    /// </summary>
    /// <remarks>2014-1-15 杨浩 创建</remarks>
    public class ParaSyTask//:Model.SyTaskConfig
    {
        /// <summary>
        /// 任务配置信息
        /// </summary>
        public SyTaskConfig Config { get; set; }

        #region 以下是辅助对象属性

        /// <summary>
        /// 开始时间段
        /// </summary>
        public TimeSpan StartTimeQuantum { get; set; }

        /// <summary>
        /// 结束时间段
        /// </summary>
        public TimeSpan EndTimeQuantum { get; set; }

        /// <summary>
        /// 间隔时间(秒)当时间类型=间隔类型
        /// </summary>
        public Int64 IntervalTime { get; set; }

        /// <summary>
        /// 每天固定时间执行
        /// </summary>
        public TimeSpan DayExecuteTime { get; set; }
        /// <summary>
        /// 每周的固定时间执行
        /// </summary>
        public TimeSpan DayOfWeekExecuteTime { get; set; }

        /// <summary>
        /// 天(每月的固定时间执行)
        /// </summary>
        public int MonthOfDayExecuteTime { get; set; }

        /// <summary>
        /// 时间(每月的固定时间执行)
        /// </summary>
        public TimeSpan MonthHmsExecuteTime { get; set; }

        #endregion
    }
}
