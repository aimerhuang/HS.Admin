using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 日期工具类
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class DateUtil
    {
        /// <summary>
        /// 计算周一
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>周一</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        public static DateTime GetMonday(DateTime date)
        {
            var i = DayOfWeek.Monday - date.DayOfWeek;
            if (i == 1) i = -6;  //Sunday为0，此时Monday-Sunday=1，必须-6。
            return date.AddDays(i);
        }

        /// <summary>
        /// 计算周日
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>周日</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        public static DateTime GetSunday(DateTime date)
        {
            return GetMonday(date).AddDays(6);
        }

        /// <summary>
        /// 计算月初
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>月初</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        public static DateTime GetMonthFirst(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 计算月末
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>月末</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        public static DateTime GetMonthLast(DateTime date)
        {
            return GetMonthFirst(date).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 将时间转换时间截
        /// </summary>
        /// <param name="time">要转换的时间</param>
        /// <remarks>2016-7-27 杨浩 创建</remarks>
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

    }
}
