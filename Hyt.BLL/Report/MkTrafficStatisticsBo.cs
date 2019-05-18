using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Report;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Report
{
    /// <summary>
    /// 网站流量统计
    /// </summary>
    /// <remarks>2013-11-13 邵斌 创建</remarks>
    public class MkTrafficStatisticsBo : BOBase<MkTrafficStatisticsBo>
    {
        /// <summary>
        /// 创建网站流量统计对象
        /// </summary>
        /// <param name="model">流量统计对象</param>
        /// <returns>返回新添加的统计对象系统编号</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public int Create(MkTrafficStatistics model)
        {
            model.IpLocation = GetIPLocation(model.IpAddress);
            model.SysNo = IMkTrafficStatisticsDao.Instance.Create(model);
            return model.SysNo;
        }

        /// <summary>
        /// 获取单个流量统计对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回流量统计对象</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public MkTrafficStatistics Get(int sysNo)
        {
            return IMkTrafficStatisticsDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 获取所有流量统计列表
        /// </summary>
        /// <returns>返回流量统计分页对象</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public Pager<MkTrafficStatistics> Search(ParaMkTrafficStatisticsFilter filter)
        {
            return IMkTrafficStatisticsDao.Instance.Search(filter);
        }

        /// <summary>
        /// 信息统计
        /// </summary>
        /// <param name="filter">统计查询调价</param>
        /// <returns>当日PV数</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public int Total(ParaMkTrafficStatisticsFilter filter)
        {
            return IMkTrafficStatisticsDao.Instance.Total(filter);
        }

        /// <summary>
        /// 时间段内流量访问统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">就是时间</param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public Pager<MkTrafficStatistics> VisitingRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                           DateTime? endTime = null,
                                                                           bool onlyTotal = false)
        {
            return IMkTrafficStatisticsDao.Instance.VisitingRangeTimeStatisticsTotal(startTime, endTime, onlyTotal);
        }

        /// <summary>
        /// 时间段内访客访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public Pager<MkTrafficStatistics> CustomerRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                           DateTime? endTime = null,
                                                                           bool onlyTotal = false)
        {
            return IMkTrafficStatisticsDao.Instance.CustomerRangeTimeStatisticsTotal(startTime, endTime, onlyTotal);
        }

        /// <summary>
        /// 时间段内IP地址访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public Pager<MkTrafficStatistics> IPAddressRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                            DateTime? endTime = null,
                                                                            bool onlyTotal = false)
        {
            return IMkTrafficStatisticsDao.Instance.IpAddressRangeTimeStatisticsTotal(startTime, endTime, onlyTotal);
            
        }

        /// <summary>
        /// 时间段内新访客访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public Pager<MkTrafficStatistics> NewUserRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                          DateTime? endTime = null,
                                                                          bool onlyTotal = false)
        {
            return IMkTrafficStatisticsDao.Instance.NewUserRangeTimeStatisticsTotal(startTime, endTime, onlyTotal);
        }

        /// <summary>
        /// 指定时间段内PV和IP对比
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束数据</param>
        /// <param name="SpliteNumber">分片段数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public IDictionary<string,IList<int>> PVAndIPForRangeTime(DateTime startTime, DateTime endTime, int SpliteNumber = 6)
        {
            IDictionary<string, IList<int>> result = new Lucene.Net.Support.Dictionary<string, IList<int>>();
            result.Add("PV", IMkTrafficStatisticsDao.Instance.PvForRangeTime(startTime, endTime, SpliteNumber));
            result.Add("IP", IMkTrafficStatisticsDao.Instance.IpForRangeTime(startTime, endTime, SpliteNumber));
            return result;
        }

        /// <summary>
        /// 读取流量简报数据
        /// </summary>
        /// <returns>返回报表流量结果对象集合</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public IList<CBMkTrafficStatisticsSampleAnalysis> GetSampleReportData()
        {
            IList<CBMkTrafficStatisticsSampleAnalysis> resultList = new List<CBMkTrafficStatisticsSampleAnalysis>();

            DateTime startDate = DateTime.Now;
            //当日简报
            resultList.Add(IMkTrafficStatisticsDao.Instance.GetSampleAnalysis(
                DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 01:00:00"),
                DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 23:59:59")));

            //昨日
            startDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            resultList.Add(IMkTrafficStatisticsDao.Instance.GetSampleAnalysis(
                 DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 01:00:00"),
                 DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 23:59:59")));

            //上周同期
            startDate = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            resultList.Add(IMkTrafficStatisticsDao.Instance.GetSampleAnalysis(
                 DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 01:00:00"),
                 DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 23:59:59")));

            //7天内
            startDate = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            resultList.Add(IMkTrafficStatisticsDao.Instance.GetSampleAnalysis(
                 DateTime.Parse(startDate.ToString("yyyy-MM-dd") + " 01:00:00"),
                 DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59")));

            return resultList;

        }

        /// <summary>
        /// 月报表PV,IP对比报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public CBMkTrafficStatisticsPVAndIPMonthReport GetPVAndIPMothReport(int year, bool isMobilePlatform)
        {
            return IMkTrafficStatisticsDao.Instance.PvAndIpMothReport(year,isMobilePlatform);
        }

        /// <summary>
        /// 7天表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public CBMkTrafficStatisticsPVAndIPMonthReport GetPVAndIP7DayReport(bool isMobilePlatform)
        {
            return IMkTrafficStatisticsDao.Instance.PvAndIp7DayReport(isMobilePlatform);
        }

        /// <summary>
        /// 12小时表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public CBMkTrafficStatisticsPVAndIPMonthReport GetPVAndIP12HourReport(bool isMobilePlatform)
        {
            return IMkTrafficStatisticsDao.Instance.PvAndIp12HourReport(isMobilePlatform);
        }

        /// <summary>
        /// 10分钟小时表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public CBMkTrafficStatisticsPVAndIPMonthReport GetPVAndIP10MinuteReport(bool isMobilePlatform)
        {
            return IMkTrafficStatisticsDao.Instance.PvAndIp10MinuteReport(isMobilePlatform);
        }

        /// <summary>
        /// 7天内:访问来源比例Top10
        /// </summary>
        /// <returns>返回七天Top10结果集合</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public IList<CBMkTrafficStatisticsPagePVAndIPReport> GetViewerSevenDayTotalTop10()
        {
            return IMkTrafficStatisticsDao.Instance.ViewerSevenDayTotalTop10();
        }

        /// <summary>
        /// 7天内:地区来源比例Top10
        /// </summary>
        /// <returns>返回七天Top10结果集合</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public IList<CBMkTrafficStatisticsPagePVAndIPReport> GetLocationSevenDayTotalTop10()
        {
            return IMkTrafficStatisticsDao.Instance.LocationSevenDayTotalTop10();
        }

        /// <summary>
        /// 7天内:访问商品信息Top10
        /// </summary>
        /// <returns>返回七天Top10结果集合</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public IList<CBMkTrafficStatisticsPagePVAndIPReport> GetProductSevenDayTotalTop10()
        {
            return IMkTrafficStatisticsDao.Instance.ProductSevenDayTotalTop10();
        }

        /// <summary>
        /// 7天内:屏幕分辩率信息Top10
        /// </summary>
        /// <returns>返回七天Top10结果集合</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public IList<CBMkTrafficStatisticsPagePVAndIPReport> GetScreenSevenDayTotalTop10()
        {
            return IMkTrafficStatisticsDao.Instance.ScreenSevenDayTotalTop10();
        }

        /// <summary>
        /// 7天内:浏览器信息Top10
        /// </summary>
        /// <returns>返回七天Top10结果集合</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public IList<CBMkTrafficStatisticsPagePVAndIPReport> GetBrowserSevenDayTotalTop10()
        {
            return IMkTrafficStatisticsDao.Instance.BrowserSevenDayTotalTop10();
        }

        #region 私有方法
        

        /// <summary>
        /// 通过IP地址获取IP归属地
        /// </summary>
        /// <param name="ip">ＩＰ地址</param>
        /// <returns>返回ＩＰ地址归属地</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        private string GetIPLocation(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return "四川省";
            }
            return "四川省";
        }

        #endregion
    }
}
