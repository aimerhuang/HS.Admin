using System;
using System.ComponentModel;

namespace Hyt.Service.Task.Core
{
    /// <summary>
    /// 任务执行时间类型
    /// </summary>
    [Serializable]
    public enum TaskTimeType
    {
        /// <summary>
        /// 间隔一定时间执行
        /// </summary>
        [Description("间隔一定时间执行")]
        Interval = 0,

        /// <summary>
        /// 每个小时里的固定时间执行
        /// </summary>
        [Description("每个小时里的固定时间执行")]
        Hour = 1,

        /// <summary>
        /// 每天固定时间执行
        /// </summary>
        [Description("每天固定时间执行")]
        Day = 2,

        /// <summary>
        /// 每周的固定时间执行
        /// </summary>
        [Description("每周的固定时间执行")]
        Week = 3,

        /// <summary>
        /// 每月的固定时间执行
        /// </summary>
        [Description("每月的固定时间执行")]
        Month = 4,

        /// <summary>
        /// 每年的固定时间执行
        /// </summary>
        [Description("每年的固定时间执行")]
        Year = 5,

    }
}
