using Hyt.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Report
{
    /// <summary>
    /// 网站流量统计
    /// </summary>
    /// <remarks>2013-11-13 邵斌 创建</remarks>
    public abstract class IMkTrafficStatisticsDao : DaoBase<IMkTrafficStatisticsDao>
    {
        /// <summary>
        /// 创建网站流量统计对象
        /// </summary>
        /// <param name="model">流量统计对象</param>
        /// <returns>返回新添加的统计对象系统编号</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public abstract int Create(MkTrafficStatistics model);

        /// <summary>
        /// 获取单个流量统计对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回流量统计对象</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public abstract MkTrafficStatistics Get(int sysNo);

        /// <summary>
        /// 获取所有流量统计列表
        /// </summary>
        /// <returns>返回流量统计分页对象</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public abstract Pager<MkTrafficStatistics> Search(ParaMkTrafficStatisticsFilter filter);

        /// <summary>
        /// 信息统计
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public abstract int Total(ParaMkTrafficStatisticsFilter filter);

        /// <summary>
        /// 时间段内流量访问统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">就是时间</param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public abstract Pager<MkTrafficStatistics> VisitingRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                           DateTime? endTime = null,
                                                                           bool onlyTotal = false);

        /// <summary>
        /// 时间段内访客访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public abstract Pager<MkTrafficStatistics> CustomerRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                           DateTime? endTime = null,
                                                                           bool onlyTotal = false);

        /// <summary>
        /// 时间段内IP地址访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public abstract Pager<MkTrafficStatistics> IpAddressRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                                     DateTime? endTime = null,
                                                                                     bool onlyTotal = false);

        /// <summary>
        /// 时间段内新访客访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public abstract Pager<MkTrafficStatistics> NewUserRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                                   DateTime? endTime = null,
                                                                                   bool onlyTotal = false);

        /// <summary>
        /// 指定时间段内PV访问量
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="spliteNumber">分片段数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public abstract IList<int> PvForRangeTime(DateTime startTime, DateTime endTime, int spliteNumber = 6);

        /// <summary>
        /// 指定时间段内IP访问量
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="spliteNumber">分片段数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public abstract IList<int> IpForRangeTime(DateTime startTime, DateTime endTime, int spliteNumber = 6);

        #region 流量汇总报表

        /// <summary>
        /// 月报表PV,IP对比报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public abstract CBMkTrafficStatisticsPVAndIPMonthReport PvAndIpMothReport(int year, bool isMobilePlatform);

        /// <summary>
        /// 7日报表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>一以当前时间为基点进行查询数据进行前推1周数据，好进行环比和同比</remarks>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public abstract CBMkTrafficStatisticsPVAndIPMonthReport PvAndIp7DayReport(bool isMobilePlatform);

        /// <summary>
        /// 12小时报表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>一以当前时间为基点进行查询数据进行前推1周数据，好进行环比和同比</remarks>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public abstract CBMkTrafficStatisticsPVAndIPMonthReport PvAndIp12HourReport(bool isMobilePlatform);

        /// <summary>
        /// 120分组报表PV,IP对比报表（10分钟为一个单位）
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>一以当前时间为基点进行查询数据进行前推1周数据，好进行环比和同比</remarks>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public abstract CBMkTrafficStatisticsPVAndIPMonthReport PvAndIp10MinuteReport(bool isMobilePlatform);

        #endregion

        #region 简单报表汇总统计对比
        
        /// <summary>
        /// 指定时间段内数据的简单汇总统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回数据 IP数，PV数，页面访问深度，评价页面停留时间，老用户比例</returns>
        /// <remarks>2013-12-13 邵斌 创建</remarks>
        public abstract CBMkTrafficStatisticsSampleAnalysis GetSampleAnalysis(DateTime startTime, DateTime endTime);

        #endregion

        #region 7天汇总

        /// <summary>
        /// 7天内:访问来源比例Top10
        /// </summary>
        /// <returns>来源PV列表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> ViewerSevenDayTotalTop10();

        /// <summary>
        /// 7天内:地区来源比例Top10
        /// </summary>
        /// <returns>来源地PV列表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> LocationSevenDayTotalTop10();

        /// <summary>
        /// 7天内:屏幕分辨率比例Top10
        /// </summary>
        /// <returns>屏幕分辩率PV列表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> ScreenSevenDayTotalTop10();

        /// <summary>
        /// 7天内:访问商品信息Top10
        /// </summary>
        /// <returns>产品PV列表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> ProductSevenDayTotalTop10();

        /// <summary>
        /// 7天内:浏览器比例Top10
        /// </summary>
        /// <returns>浏览器PV列表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> BrowserSevenDayTotalTop10();

        #endregion

        /// <summary>
        /// 页面PV，IP统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="recordNumber">记录条数</param>
        /// <returns>流量报表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> PagePvAndIpTotal(DateTime startTime,
                                                                                       DateTime endTime,
                                                                                       int recordNumber = 10);

        /// <summary>
        /// 页面PV，IP统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="recordNumber">记录条数</param>
        /// <returns>流量报表</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public abstract IList<CBMkTrafficStatisticsPagePVAndIPReport> PageSearchPvAndIpTotal(DateTime startTime,
                                                                                       DateTime endTime,
                                                                                       int recordNumber = 10);

    }
}
