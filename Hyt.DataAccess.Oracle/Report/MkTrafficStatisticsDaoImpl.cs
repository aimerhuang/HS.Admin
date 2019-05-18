using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Report;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Report
{

    /// <summary>
    /// 网站流量统计
    /// </summary>
    /// <remarks>2013-11-13 邵斌 创建</remarks>
    public class MkTrafficStatisticsDaoImpl : IMkTrafficStatisticsDao
    {
        /// <summary>
        /// 创建网站流量统计对象
        /// </summary>
        /// <param name="model">流量统计对象</param>
        /// <returns>返回新添加的统计对象系统编号</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public override int Create(MkTrafficStatistics model)
        {
            return Context.Insert<MkTrafficStatistics>("MkTrafficStatistics", model).AutoMap(m => m.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取单个流量统计对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回流量统计对象</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public override MkTrafficStatistics Get(int sysNo)
        {
            return Context.Select<MkTrafficStatistics>("*")
                       .From("MkTrafficStatistics")
                       .Where("SysNo=@SysNo")
                       .Parameter("SysNo", sysNo)
                       .QuerySingle();
        }

        /// <summary>
        /// 获取所有流量统计列表
        /// </summary>
        /// <param name="filter">过滤参数</param>
        /// <returns>返回流量统计分页对象</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public override Pager<MkTrafficStatistics> Search(ParaMkTrafficStatisticsFilter filter)
        {
            /*
            SysNo
            VisitorsSign
            UrlAddress
            VisitingTime between a and b                              
            charindex(lower(IpAddress),lower(:IpAddress)) == 1
            IpLocation
            charindex(lower(BrowserType),lower(:BrowserType)) > 0
            Screen
            LoadTime between a and b
            CustomerSysNo
            Gender
            Age between a and b
            SourceDomain
            SeoKeyword
             * */

            /*
             * 动态查询
             */

            return null;
        }

        /// <summary>
        /// 信息统计
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>PV数</returns>
        /// <remarks>2013-11-13 邵斌 创建</remarks>
        public override int Total(ParaMkTrafficStatisticsFilter filter)
        {

            var query = Context.Select<int>("count(sysno)").From("MkTrafficStatistics");
            StringBuilder whereString = new StringBuilder("1=1");

            //筛选过滤浏览器信息
            if (!string.IsNullOrWhiteSpace(filter.BrowserType))
            {
                whereString.Append("and charindex(BrowserType,@BrowserType)>0");
                query.Parameter("BrowserType", filter.BrowserType);
            }

            //过滤用户
            if (filter.CustomerSysNo > 0)
            {
                whereString.Append("and CustomerSysNo=@CustomerSysNo");
                query.Parameter("CustomerSysNo", filter.CustomerSysNo);
            }

            //过滤加载时间大于
            if (filter.StartLoadTime > 0)
            {
                whereString.Append("and LoadTime>@StartLoadTime");
                query.Parameter("StartLoadTime", filter.StartLoadTime - 1);
            }

            //过滤加载时间小于
            if (filter.EndLoadTime > 0)
            {
                whereString.Append("and LoadTime<@EndLoadTime");
                query.Parameter("EndLoadTime", filter.EndLoadTime + 1);
            }

            //过滤访问时间大于
            if (filter.StartVisitingTime.Ticks > 0)
            {
                whereString.Append("and VisitingTime > @StartVisitingTime");
                query.Parameter("StartVisitingTime", filter.StartVisitingTime.Subtract(new TimeSpan(0, 0, 0, 1)));
            }

            //过滤访问时间小于
            if (filter.EndVisitingTime.Ticks > 0)
            {
                whereString.Append("and VisitingTime < @EndVisitingTime");
                query.Parameter("EndVisitingTime", filter.StartVisitingTime.AddSeconds(1));
            }

            //性别
            if (filter.Gender > 0)
            {
                whereString.Append("and Gender = Gender");
                query.Parameter("Gender", filter.Gender);
            }

            //访问IP地址过滤
            if (!string.IsNullOrWhiteSpace(filter.IpAddress))
            {
                whereString.Append("and charindex(IpAddress,@IpAddress) = 1");
                query.Parameter("IpAddress", filter.IpAddress);
            }

            //IP归属地查询
            if (filter.IpLocation > 0)
            {
                whereString.Append("and IpLocation = @IpLocation");
                query.Parameter("IpLocation", filter.IpLocation);
            }

            //年龄大于
            if (filter.MinLimitAge > 0)
            {
                whereString.Append("and Age > @MinLimitAge");
                query.Parameter("MinLimitAge", filter.MinLimitAge - 1);
            }

            //年龄小于
            if (filter.MaxLimitAge > 0)
            {
                whereString.Append("and Age < @MaxLimitAge");
                query.Parameter("MaxLimitAge", filter.MaxLimitAge + 1);
            }

            //年龄小于
            if (!string.IsNullOrWhiteSpace(filter.Screen))
            {
                whereString.Append("and Screen = @Screen");
                query.Parameter("Screen", filter.Screen);
            }

            //搜索关键字
            if (!string.IsNullOrWhiteSpace(filter.SeoKeyword))
            {
                whereString.Append("and SEOKeyWord = @SeoKeyword");
                query.Parameter("SeoKeyword", filter.SeoKeyword);
            }

            //来源域
            if (!string.IsNullOrWhiteSpace(filter.SourceDomain))
            {
                whereString.Append("and SourceDomain = @SourceDomain");
                query.Parameter("SourceDomain", filter.SourceDomain);
            }

            return 0;
        }

        #region 流量统计

        /// <summary>
        /// 时间段内流量访问统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">就是时间</param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public override Pager<MkTrafficStatistics> VisitingRangeTimeStatisticsTotal(DateTime? startTime = null, DateTime? endTime = null, bool onlyTotal = false)
        {
            var result = new Pager<MkTrafficStatistics>();

            using (var _context = Context.UseSharedConnection(true))
            {
                //当只汇总为false是进行当前页记录查询
                if (!onlyTotal)
                {
                    result.Rows =
                        _context.Select<MkTrafficStatistics>("*")
                               .From("MkTrafficStatistics")
                               .Where(
                                   "(nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))")
                                   .Parameter("StartVisitingTime", startTime)
                                   .Parameter("EndVisitingTime", endTime)
                                   .OrderBy("VisitingTime")
                                   .QueryMany();
                }

                //记录汇总
                result.TotalRows = _context.Select<int>("count(SysNo)")
                          .From("MkTrafficStatistics")
                          .Where(
                              "(nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))")
                              .Parameter("StartVisitingTime", startTime)
                              .Parameter("EndVisitingTime", endTime)
                              .OrderBy("VisitingTime")
                              .QuerySingle();

            }
            return result;
        }

        /// <summary>
        /// 时间段内访客访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public override Pager<MkTrafficStatistics> CustomerRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                      DateTime? endTime = null, bool onlyTotal = false)
        {
            var result = new Pager<MkTrafficStatistics>();

            using (var _context = Context.UseSharedConnection(true))
            {
                //当只汇总为false是进行当前页记录查询
                if (!onlyTotal)
                {

                    result.Rows =
                        _context.Select<MkTrafficStatistics>("*")
                                .From(@"
                                    (
                                        select 
                                        VisitorsSign as vs,max(sysno) as sysno,min(visitingtime) as minTime,max(visitingtime) as maxTime
                                        from
                                        MkTrafficStatistics
                                        where
                                        (nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))
                                        group by 
                                        VisitorsSign
                                        ) tempTable
                                        inner join MkTrafficStatistics on MkTrafficStatistics.sysno = tempTable.sysno
                                ")
                                .Parameter("StartVisitingTime", startTime)
                                .Parameter("EndVisitingTime", endTime)
                                .OrderBy("VisitingTime")
                                .QueryMany();  //查询中包含了一个用户的开始访问和结束访问时间
                }

                //记录汇总
                result.TotalRows = _context.Sql(@"
                    select count(tempTable.vs)
                    from 
                    (
                    select 
                    VisitorsSign as vs
                    from
                    MkTrafficStatistics
                    where
                    (nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))
                    group by 
                    VisitorsSign
                    ) tempTable
                    ")
                 .Parameter("StartVisitingTime", startTime)
                .Parameter("EndVisitingTime", endTime).QuerySingle<int>();

            }
            return result;
        }

        /// <summary>
        /// 时间段内IP地址访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public override Pager<MkTrafficStatistics> IpAddressRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                      DateTime? endTime = null, bool onlyTotal = false)
        {
            var result = new Pager<MkTrafficStatistics>();

            using (var _context = Context.UseSharedConnection(true))
            {
                //当只汇总为false是进行当前页记录查询
                if (!onlyTotal)
                {

                    result.Rows =
                        _context.Select<MkTrafficStatistics>("*")
                                .From(@"
                                    (
                                        select 
                                        VisitorsSign as vs,max(sysno) as sysno,min(visitingtime) as minTime,max(visitingtime) as maxTime
                                        from
                                        MkTrafficStatistics
                                        where
                                        (nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))
                                        group by 
                                        VisitorsSign
                                        ) tempTable
                                        inner join MkTrafficStatistics on MkTrafficStatistics.sysno = tempTable.sysno
                                ")
                                .Parameter("StartVisitingTime", startTime)
                                .Parameter("EndVisitingTime", endTime)
                                .OrderBy("VisitingTime")
                                .QueryMany();  //查询中包含了一个用户的开始访问和结束访问时间
                }

                //记录汇总
                result.TotalRows = _context.Sql(@"
                    select count(tempTable.vs)
                    from 
                    (
                    select 
                    VisitorsSign as vs
                    from
                    MkTrafficStatistics
                    where
                    (nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))
                    group by 
                    VisitorsSign
                    ) tempTable
                    ")
                 .Parameter("StartVisitingTime", startTime)
                .Parameter("EndVisitingTime", endTime).QuerySingle<int>();

            }
            return result;
        }

        /// <summary>
        /// 时间段内新访客访问统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="onlyTotal">只汇总</param>
        /// <returns>返回分页信息</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public override Pager<MkTrafficStatistics> NewUserRangeTimeStatisticsTotal(DateTime? startTime = null,
                                                                      DateTime? endTime = null, bool onlyTotal = false)
        {
            var result = new Pager<MkTrafficStatistics>();

            using (var _context = Context.UseSharedConnection(true))
            {
                //当只汇总为false是进行当前页记录查询
                if (!onlyTotal)
                {

                    result.Rows =
                        _context.Select<MkTrafficStatistics>("*")
                                .From(@"
                                   (
                                        select 
                                        visitorssign
                                        from
                                        MkTrafficStatistics
                                        where
                                        (nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))
                                        group by 
                                        visitorssign
                                        ) tempTable 
                                        inner join MkTrafficStatistics mkt on mkt.visitorssign = tempTable.visitorssign
                                ")
                                .Where(
                                    "not exists(select mkt1.visitorssign from MkTrafficStatistics mkt1 where mkt1.VisitingTime <@StartVisitingTime and tempTable.visitorssign = mkt1.visitorssign)")
                                .Parameter("StartVisitingTime", startTime)
                                .Parameter("EndVisitingTime", endTime)
                                .OrderBy("VisitingTime")
                                .QueryMany();  //查询中包含了一个用户的开始访问和结束访问时间
                }

                //记录汇总
                result.TotalRows = _context.Sql(@"
                    select 
                    count(tempTable.visitorssign)
                    from 
                    (
                    select 
                    visitorssign
                    from
                    MkTrafficStatistics
                    where
                    (nvl2(@StartVisitingTime,0,1) = 1 or (VisitingTime > @StartVisitingTime)) and (nvl2(@EndVisitingTime,0,1) = 1 or (VisitingTime < @EndVisitingTime))
                    group by 
                    visitorssign
                    ) tempTable 
                    where 
                    not exists(select mkt1.visitorssign from MkTrafficStatistics mkt1 where mkt1.VisitingTime <@StartVisitingTime and tempTable.visitorssign = mkt1.visitorssign)
                    ")
                 .Parameter("StartVisitingTime", startTime)
                .Parameter("EndVisitingTime", endTime)
                .QuerySingle<int>();

            }
            return result;
        }
        #region 流量汇总报表


        /// <summary>
        /// 月报表PV,IP对比报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public override CBMkTrafficStatisticsPVAndIPMonthReport PvAndIpMothReport(int year, bool isMobilePlatform)
        {
            return Context.Sql(@"
                select  ViewYear as Item
                ,count(m_01) m_01_PV,count(distinct m_01) m_01_IP
                ,count(m_02) m_02_PV,count(distinct m_02) m_02_IP
                ,count(m_03) m_03_PV,count(distinct m_03) m_03_IP
                ,count(m_04) m_04_PV,count(distinct m_04) m_04_IP
                ,count(m_05) m_05_PV,count(distinct m_05) m_05_IP
                ,count(m_06) m_06_PV,count(distinct m_06) m_06_IP
                ,count(m_07) m_07_PV,count(distinct m_07) m_07_IP
                ,count(m_08) m_08_PV,count(distinct m_08) m_08_IP
                ,count(m_09) m_09_PV,count(distinct m_09) m_09_IP
                ,count(m_10) m_10_PV,count(distinct m_10) m_10_IP
                ,count(m_11) M_11_PV,count(distinct m_11) M_11_IP
                ,count(m_12) m_12_PV,count(distinct m_12) m_12_IP
                from
                (
                select Convert(nvarchar(4),VisitingTime,120) as ViewYear,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '01' THEN Visitorssign ELSE null END m_01,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '02' THEN Visitorssign ELSE null END m_02,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '03' THEN Visitorssign ELSE null END m_03,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '04' THEN Visitorssign ELSE null END m_04,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '05' THEN Visitorssign ELSE null END m_05,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '06' THEN Visitorssign ELSE null END m_06,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '07' THEN Visitorssign ELSE null END m_07,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '08' THEN Visitorssign ELSE null END m_08,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '09' THEN Visitorssign ELSE null END m_09,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '10' THEN Visitorssign ELSE null END m_10,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '11' THEN Visitorssign ELSE null END m_11,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),6,2) WHEN '12' THEN Visitorssign ELSE null END m_12
                from  
                 MkTrafficStatistics 
                where VisitingTime > to_date('" + (year - 1) +
                        @"/12/31 23:59:59','yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + year +
                        @"/12/31 23:59:59','yyyy/mm/dd HH24:mi:ss') and (:0 = 0 or IsMobileDevice=1) 
                group by VisitingTime,Visitorssign
                )
                group by ViewYear
            ", (isMobilePlatform ? 1 : 0)).QuerySingle<CBMkTrafficStatisticsPVAndIPMonthReport>();
        }

        /// <summary>
        /// 日表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>一以当前时间为基点进行查询数据进行前推1周数据，好进行环比和同比</remarks>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public override CBMkTrafficStatisticsPVAndIPMonthReport PvAndIp7DayReport(bool isMobilePlatform)
        {
            DateTime firstDate = DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0));

            return Context.Sql(@"
                select  ViewYear as Item
                ,count(m_01) m_01_PV,count(distinct m_01) m_01_IP
                ,count(m_02) m_02_PV,count(distinct m_02) m_02_IP
                ,count(m_03) m_03_PV,count(distinct m_03) m_03_IP
                ,count(m_04) m_04_PV,count(distinct m_04) m_04_IP
                ,count(m_05) m_05_PV,count(distinct m_05) m_05_IP
                ,count(m_06) m_06_PV,count(distinct m_06) m_06_IP
                ,count(m_07) m_07_PV,count(distinct m_07) m_07_IP
                from
                (
                select 'flag' as ViewYear,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.ToString("dd") + @"' THEN Visitorssign ELSE null END m_01,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.AddDays(1).ToString("dd") + @"' THEN Visitorssign ELSE null END m_02,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.AddDays(2).ToString("dd") + @"' THEN Visitorssign ELSE null END m_03,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.AddDays(3).ToString("dd") + @"' THEN Visitorssign ELSE null END m_04,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.AddDays(4).ToString("dd") + @"' THEN Visitorssign ELSE null END m_05,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.AddDays(5).ToString("dd") + @"' THEN Visitorssign ELSE null END m_06,
                CASE Substring(Convert(nvarchar(10),VisitingTime,120),9,2) WHEN '" + firstDate.AddDays(6).ToString("dd") + @"' THEN Visitorssign ELSE null END m_07
                from  
                 MkTrafficStatistics 
                where VisitingTime > to_date('" + firstDate.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyy/MM/dd") +
                      @" 23:59:59','yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") +
                      @" 00:00:00','yyyy/mm/dd HH24:mi:ss') and (:0 = 0 or IsMobileDevice=1) 
                group by VisitingTime,Visitorssign
                )
                group by ViewYear
            ", (isMobilePlatform ? 1 : 0)).QuerySingle<CBMkTrafficStatisticsPVAndIPMonthReport>();
        }

        /// <summary>
        /// 日表PV,IP对比报表
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>一以当前时间为基点进行查询数据进行前推1周数据，好进行环比和同比</remarks>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public override CBMkTrafficStatisticsPVAndIPMonthReport PvAndIp12HourReport(bool isMobilePlatform)
        {
            DateTime befor12Hours = DateTime.Now.Subtract(new TimeSpan(12, 0, 0));

            return Context.Sql(@"
                select  
                  dayNum  as Item
                  ,count(m_01) m_01_PV,count(distinct m_01) m_01_IP
                  ,count(m_02) m_02_PV,count(distinct m_02) m_02_IP
                  ,count(m_03) m_03_PV,count(distinct m_03) m_03_IP
                  ,count(m_04) m_04_PV,count(distinct m_04) m_04_IP
                  ,count(m_05) m_05_PV,count(distinct m_05) m_05_IP
                  ,count(m_06) m_06_PV,count(distinct m_06) m_06_IP
                  ,count(m_07) m_07_PV,count(distinct m_07) m_07_IP
                  ,count(m_08) m_08_PV,count(distinct m_08) m_08_IP
                  ,count(m_09) m_09_PV,count(distinct m_09) m_09_IP
                  ,count(m_10) m_10_PV,count(distinct m_10) m_10_IP
                  ,count(m_11) m_11_PV,count(distinct m_11) m_11_IP
                  ,count(m_12) m_12_PV,count(distinct m_12) m_12_IP
                from
                (
                  select 'flag' as dayNum,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.Hour + @"' THEN Visitorssign ELSE null END m_01,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(1).ToString("HH") + @"' THEN Visitorssign ELSE null END m_02,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(2).ToString("HH") + @"' THEN Visitorssign ELSE null END m_03,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(3).ToString("HH") + @"' THEN Visitorssign ELSE null END m_04,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(4).ToString("HH") + @"' THEN Visitorssign ELSE null END m_05,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(5).ToString("HH") + @"' THEN Visitorssign ELSE null END m_06,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(6).ToString("HH") + @"' THEN Visitorssign ELSE null END m_07,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(7).ToString("HH") + @"' THEN Visitorssign ELSE null END m_08,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(8).ToString("HH") + @"' THEN Visitorssign ELSE null END m_09,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(9).ToString("HH") + @"' THEN Visitorssign ELSE null END m_10,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(10).ToString("HH") + @"' THEN Visitorssign ELSE null END m_11,
                  CASE Substring(Convert(nvarchar(20),VisitingTime,120),12,2) WHEN '" + befor12Hours.AddHours(11).ToString("HH") + @"' THEN Visitorssign ELSE null END m_12
                  from  
                   MkTrafficStatistics 
                  where VisitingTime > to_date('" + befor12Hours.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss')
                    and (:0 = 0 or IsMobileDevice=1) 
                  group by VisitingTime,Visitorssign
                )
                group by dayNum
                ", (isMobilePlatform ? 1 : 0)).QuerySingle<CBMkTrafficStatisticsPVAndIPMonthReport>();
        }

        /// <summary>
        /// 120分组报表PV,IP对比报表（10分钟为一个单位）
        /// </summary>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回统计数组，</returns>
        /// <remarks>一以当前时间为基点进行查询数据进行前推1周数据，好进行环比和同比</remarks>
        /// <remarks>2013-12-10 邵斌 创建</remarks>
        public override CBMkTrafficStatisticsPVAndIPMonthReport PvAndIp10MinuteReport(bool isMobilePlatform)
        {
            DateTime befor120Minutes = DateTime.Now.Subtract(new TimeSpan(0, 120, 0));

            return Context.Sql(@"
                select  
                  dayNum  as Item
                  ,count(m_01) m_01_PV,count(distinct m_01) m_01_IP
                  ,count(m_02) m_02_PV,count(distinct m_02) m_02_IP
                  ,count(m_03) m_03_PV,count(distinct m_03) m_03_IP
                  ,count(m_04) m_04_PV,count(distinct m_04) m_04_IP
                  ,count(m_05) m_05_PV,count(distinct m_05) m_05_IP
                  ,count(m_06) m_06_PV,count(distinct m_06) m_06_IP
                  ,count(m_07) m_07_PV,count(distinct m_07) m_07_IP
                  ,count(m_08) m_08_PV,count(distinct m_08) m_08_IP
                  ,count(m_09) m_09_PV,count(distinct m_09) m_09_IP
                  ,count(m_10) m_10_PV,count(distinct m_10) m_10_IP
                  ,count(m_11) m_11_PV,count(distinct m_11) m_11_IP
                  ,count(m_12) m_12_PV,count(distinct m_12) m_12_IP
                from
                (
                  select 'flag' as dayNum,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),0,Visitorssign,null) m_01,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),1,Visitorssign,null) m_02,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),2,Visitorssign,null) m_03,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),3,Visitorssign,null) m_04,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),4,Visitorssign,null) m_05,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),5,Visitorssign,null) m_06,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),6,Visitorssign,null) m_07,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),7,Visitorssign,null) m_08,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),8,Visitorssign,null) m_09,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),9,Visitorssign,null) m_10,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),10,Visitorssign,null) m_11,
                  decode(floor((VisitingTime-to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss'))*24*60/10),11,Visitorssign,null) m_12
                  from  
                   MkTrafficStatistics 
                  where VisitingTime > to_date('" + befor120Minutes.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd HH24:mi:ss')
                    and (:0 = 0 or IsMobileDevice=1) 
                  group by VisitingTime,Visitorssign
                )
                group by dayNum
                ", (isMobilePlatform ? 1 : 0)).QuerySingle<CBMkTrafficStatisticsPVAndIPMonthReport>();
        }

        #endregion

        #region 简单报表汇总统计对比

        /// <summary>
        /// 指定时间段内数据的简单汇总统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回数据 IP数，PV数，页面访问深度，评价页面停留时间，老用户比例</returns>
        /// <remarks>2013-12-13 邵斌 创建</remarks>
        public override CBMkTrafficStatisticsSampleAnalysis GetSampleAnalysis(DateTime startTime, DateTime endTime)
        {
            #region 测试SQL

            /*
                  
                --查询IP和PV 使用用户表示来作为IP数统计，不能用实际IP字段是因为可能造成同IP用户漏记。
                select count(IP) as IP,sum(pv) as PV from(
                       select Visitorssign as IP,count(Visitorssign) as pv from MkTrafficStatistics where VisitingTime > to_date('2013/12/01 00:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2013/12/02 00:00:00', 'yyyy/mm/dd HH24:mi:ss') group by Visitorssign
                );

                --查询访问深度(一个用户标示访问了多少个页面除以用户数)
                select sum(countNum)/count(Visitorssign)
                from
                (
                  select Visitorssign,sum(countNum) as countNum from(     
                         select distinct urladdress,Visitorssign, 1 as countNum from MkTrafficStatistics where VisitingTime > to_date('2013/12/01 00:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2013/12/02 00:00:00', 'yyyy/mm/dd HH24:mi:ss') order by Visitorssign,urladdress
                  ) tempTable group by Visitorssign order by Visitorssign
                ) tt; 

                --平均停留时间(单位：秒)(总访问时长除以用户数)
                select sum(stayTime)/count(Visitorssign)
                from (select Visitorssign,
                                       decode(lag(Visitorssign) over(order by Visitorssign),
                                              Visitorssign,
                                              ceil((VisitingTime - lag(VisitingTime)
                                                    over(order by Visitorssign)) * 24 * 60 * 60),
                                              0) as stayTime
                                  from MkTrafficStatistics
                                 where VisitingTime >
                                       to_date('2013/12/01 00:00:00', 'yyyy/mm/dd HH24:mi:ss')
                                   and VisitingTime <
                                       to_date('2013/12/02 00:00:00', 'yyyy/mm/dd HH24:mi:ss')                   
                                 group by Visitorssign, VisitingTime
                ) tt
                where tt.stayTime>0 and tt.stayTime<1200   --超过1200秒（20分钟Session默认过期时间）为无效链接
                ;

                --新老访客比
                select (sum(oldIP)/sum(ip))*100 
                from
                (               
                  select 
                  1 as ip,nvl2(tt.Visitorssign,1,0) as oldIP 
                  from 
                  MkTrafficStatistics mts 
                  left join
                  (
                        select Visitorssign
                        from MkTrafficStatistics
                        where VisitingTime <= to_date('2013/12/01 00:00:00', 'yyyy/mm/dd HH24:mi:ss') or VisitingTime >= to_date('2013/12/02 00:00:00', 'yyyy/mm/dd HH24:mi:ss')
                        group by Visitorssign                 
                  ) tt on mts.Visitorssign = tt.Visitorssign
                  where 
                  mts.VisitingTime > to_date('2013/12/01 00:00:00', 'yyyy/mm/dd HH24:mi:ss') and mts.VisitingTime < to_date('2013/12/02 00:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                  group by mts.Visitorssign,tt.Visitorssign
                );       
             */

            #endregion

            CBMkTrafficStatisticsSampleAnalysis model;

            using (var _context = Context.UseSharedConnection(true))
            {
                //查询IP和PV值，并第一次初始化结果Model对象
                model = _context.Sql(@"
                    select count(IP) as IP,sum(pv) as PV from(
                            select Visitorssign as IP,count(Visitorssign) as pv from MkTrafficStatistics where VisitingTime > to_date('" + startTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + endTime.ToString("yyyy/MM/dd HH:mm:ss") + @"', 'yyyy/mm/dd HH24:mi:ss') group by Visitorssign
                    )").QuerySingle<CBMkTrafficStatisticsSampleAnalysis>();

                //如果时间段没没有数据（即Model为空）就没有必要执行其他数据统计
                if (model != null)
                {
                    //有Model数据，多其他统计数据进行查询

                    //查询访问深度(一个用户标示访问了多少个页面除以用户数)
                    model.AverageViewPages = _context.Sql(@"
                   select round(sum(countNum)/count(Visitorssign),2)
                    from
                    (
                      select Visitorssign,sum(countNum) as countNum from(     
                             select distinct urladdress,Visitorssign, 1 as countNum from MkTrafficStatistics where VisitingTime > to_date('" + startTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + endTime.ToString("yyyy/MM/dd HH:mm:ss") + @"', 'yyyy/mm/dd HH24:mi:ss') order by Visitorssign,urladdress
                      ) tempTable group by Visitorssign order by Visitorssign
                    ) tt").QuerySingle<decimal>();

                    //平均停留时间(单位：秒)(总访问时长除以用户数)
                    model.AverageStayTime = _context.Sql(@"
                        select round(sum(stayTime)/count(Visitorssign),2)
                        from (select Visitorssign,
                                               decode(lag(Visitorssign) over(order by Visitorssign),
                                                      Visitorssign,
                                                      ceil((VisitingTime - lag(VisitingTime)
                                                            over(order by Visitorssign)) * 24 * 60 * 60),
                                                      0) as stayTime
                                          from MkTrafficStatistics
                                         where 
                                               VisitingTime > to_date('" + startTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + endTime.ToString("yyyy/MM/dd HH:mm:ss") + @"', 'yyyy/mm/dd HH24:mi:ss')
                                         group by Visitorssign, VisitingTime
                        ) tt
                        where tt.stayTime>0 and tt.stayTime<1200 
                    ").QuerySingle<int>();

                    //新老访客比
                    model.OldCustomerReviewRate = _context.Sql(@"
                        select round((sum(oldIP)/sum(ip))*100,2)
                        from
                        (               
                          select 
                          1 as ip,nvl2(tt.Visitorssign,1,0) as oldIP 
                          from 
                          MkTrafficStatistics mts 
                          left join
                          (
                                select Visitorssign
                                from MkTrafficStatistics
                                where 
                                VisitingTime > to_date('" + startTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" + endTime.ToString("yyyy/MM/dd HH:mm:ss") + @"', 'yyyy/mm/dd HH24:mi:ss')
                                group by Visitorssign                 
                          ) tt on mts.Visitorssign = tt.Visitorssign
                          where 
                                mts.VisitingTime < to_date('" + startTime.ToString("yyyy/MM/dd HH:mm:ss") + "', 'yyyy/mm/dd HH24:mi:ss') or mts.VisitingTime > to_date('" + endTime.ToString("yyyy/MM/dd HH:mm:ss") + @"', 'yyyy/mm/dd HH24:mi:ss')
                          group by mts.Visitorssign,tt.Visitorssign
                        )
                    ").QuerySingle<decimal>();
                }
                else
                {
                    model = new CBMkTrafficStatisticsSampleAnalysis();
                }

            }

            return model;
        }

        #endregion

        #region 7天汇总

        /// <summary>
        /// 7天内:访问来源比例Top10
        /// </summary>
        /// <returns>来源PV比例结果</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> ViewerSevenDayTotalTop10()
        {
            IList<CBMkTrafficStatisticsPagePVAndIPReport> result = new List<CBMkTrafficStatisticsPagePVAndIPReport>();

            #region 测试SQL

            /*
             --PV 统计总数
            select count(sysno) from MkTrafficStatistics where sourcedomain is not null and VisitingTime > to_date('2013/12/09 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2013/12/16 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
            
            --如果 sourcedomain为空表示为直接访问
            select * from 
            (
            --91为统计时间段内有来源PV总数
               select sourcedomain,round((count(sysno)/91)*100,2) as rateNum
                    from MkTrafficStatistics
                   where VisitingTime > to_date('2013/12/09 01:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                         and VisitingTime < to_date('2013/12/16 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                   group by sourcedomain                   
            )
            where rateNum > 0 and rownum < 11
            order by rateNum desc
             * */
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //PV 统计总数
                var totalPV =
                    _context.Sql(
                        "select count(sysno) from MkTrafficStatistics where sourcedomain is not null and VisitingTime > to_date('" +
                        DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                        " 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" +
                        DateTime.Now.ToString("yyyy/MM/dd") +
                        " 23:59:59', 'yyyy/mm/dd HH24:mi:ss')")
                            .QuerySingle<int>();

                if (totalPV == 0)
                    return result;

                result = _context.Sql(@" select * from 
                                        (
                                           select sourcedomain as Item,round((count(sysno)/" + totalPV + @")*100,2) as TotalPV
                                                from MkTrafficStatistics
                                               where VisitingTime > to_date('" +
                                                                    DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                                                                    @" 01:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                                                     and VisitingTime < to_date('" +
                                                                        DateTime.Now.ToString("yyyy/MM/dd") +
                                                                        @" 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                                               group by sourcedomain
                                        )
                                        where TotalPV > 0 and rownum < 11
                                        order by TotalPV desc").QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();

            }

            return result;
        }

        /// <summary>
        /// 7天内:地区来源比例Top10
        /// </summary>
        /// <returns>地区PV比例结果</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> LocationSevenDayTotalTop10()
        {
            IList<CBMkTrafficStatisticsPagePVAndIPReport> result = new List<CBMkTrafficStatisticsPagePVAndIPReport>();

            #region 测试SQL

            /*
             --PV 统计总数
            select count(sysno) from MkTrafficStatistics where IPLOCATION is not null and VisitingTime > to_date('2013/12/09 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2013/12/16 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
            select * from 
            (
                --19为统计时间段内有来源PV总数
               select IPLOCATION,round((count(sysno)/19)*100,2) as rateNum
                    from MkTrafficStatistics
                   where VisitingTime >
                         to_date('2013/12/09 01:00:00', 'yyyy/mm/dd HH24:mi:ss')
                     and VisitingTime <
                         to_date('2013/12/16 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                     and IPLOCATION is not null
                   group by IPLOCATION     
            )
            where rateNum > 0 and rownum < 11
            order by rateNum desc
             * */
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //PV 统计总数
                var totalPV =
                    _context.Sql(
                        "select count(sysno) from MkTrafficStatistics where IPLOCATION is not null and VisitingTime > to_date('" +
                        DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                        " 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" +
                        DateTime.Now.ToString("yyyy/MM/dd") +
                        " 23:59:59', 'yyyy/mm/dd HH24:mi:ss')")
                            .QuerySingle<int>();

                if (totalPV == 0)
                    return result;

                result = _context.Sql(@" select * from 
                                        (
                                           select IPLOCATION as Item,round((count(sysno)/" + totalPV + @")*100,2) as TotalPV
                                            from MkTrafficStatistics
                                               where VisitingTime > to_date('" +
                                                                    DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                                                                    @" 01:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                                                     and VisitingTime < to_date('" +
                                                                        DateTime.Now.ToString("yyyy/MM/dd") +
                                                                        @" 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                                                     and IPLOCATION is not null
                                               group by IPLOCATION                                              
                                        )
                                        where TotalPV > 0 and rownum < 11
                                        order by TotalPV desc").QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();

            }

            return result;
        }

        /// <summary>
        /// 7天内:访问商品信息Top10
        /// </summary>
        /// <returns>产品PV比例结果</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> ProductSevenDayTotalTop10()
        {
            IList<CBMkTrafficStatisticsPagePVAndIPReport> result = new List<CBMkTrafficStatisticsPagePVAndIPReport>();

            #region 测试SQL

            /*
             --PV 统计总数
            select count(sysno) from MkTrafficStatistics where charindex(UrlAddress,'/product/details/') > 0 and VisitingTime > to_date('2013/12/09 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2013/12/16 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
            select * from 
            (
                --67为统计时间段内有来源PV总数
                select UrlAddress,round((count(sysno)/67)*100,2) as rateNum
                    from MkTrafficStatistics
                    where VisitingTime >
                            to_date('2013/12/09 01:00:00', 'yyyy/mm/dd HH24:mi:ss')
                        and VisitingTime <
                            to_date('2013/12/16 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                        and charindex(UrlAddress,'/product/details/') > 0
                    group by UrlAddress     
            )
            where rateNum > 0 and rownum < 11
            order by rateNum desc
             * */
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //PV 统计总数
                var totalPV =
                    _context.Sql(
                        "select count(sysno) from MkTrafficStatistics where charindex(lower(UrlAddress),'/product/details/') > 0 and VisitingTime > to_date('" +
                        DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                        " 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" +
                        DateTime.Now.ToString("yyyy/MM/dd") +
                        " 23:59:59', 'yyyy/mm/dd HH24:mi:ss')")
                            .QuerySingle<int>();

                if (totalPV == 0)
                    return result;

                result = _context.Sql(@" select * from 
                                        (
                                           select UrlAddress as Item,round((count(sysno)/" + totalPV + @")*100,2) as TotalPV
                                            from MkTrafficStatistics
                                               where VisitingTime > to_date('" +
                                                                    DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                                                                    @" 01:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                                                     and VisitingTime < to_date('" +
                                                                        DateTime.Now.ToString("yyyy/MM/dd") +
                                                                        @" 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                                                     and charindex(lower(UrlAddress),'/product/details/') > 0
                                               group by UrlAddress                                              
                                        )
                                        where TotalPV > 0 and rownum < 11
                                        order by TotalPV desc").QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();


                //如果有数据将对数据中的商品进行名称替换
                if (result.Count > 0)
                {
                    //从连接中取商品系统编号
                    //格式化商品名称
                    StringBuilder sb = new StringBuilder();
                    int postion = 0;
                    int productSysno = 0;
                    foreach (var cbMkTrafficStatisticsPagePvAndIpReport in result)
                    {
                        postion = cbMkTrafficStatisticsPagePvAndIpReport.Item.ToLower().IndexOf("/product/details");
                        if (postion > 0)
                        {
                            postion += 17;
                            int.TryParse(cbMkTrafficStatisticsPagePvAndIpReport.Item.ToLower().Substring(postion, cbMkTrafficStatisticsPagePvAndIpReport.Item.Length - postion), out productSysno);
                            if (sb.Length > 0)
                            {
                                sb.Append(",");
                            }
                            sb.Append(productSysno);
                            cbMkTrafficStatisticsPagePvAndIpReport.Item = productSysno.ToString();
                        }
                    }


                    //通过商品系统编号查询商品
                    IList<PdProduct> products = _context.Sql(string.Format("select sysno,easname from pdproduct where sysno in ({0})", sb.ToString())).QueryMany<PdProduct>();
                    //设置商品名称
                    foreach (var cbMkTrafficStatisticsPagePvAndIpReport in result)
                    {
                        var product = products.Where(p => p.SysNo.ToString() == cbMkTrafficStatisticsPagePvAndIpReport.Item).FirstOrDefault();
                        if (product != null)
                        {
                            cbMkTrafficStatisticsPagePvAndIpReport.Item = product.EasName;
                        }
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// 7天内:屏幕分辨率比例Top10
        /// </summary>
        /// <returns>屏幕分辨率比例结果</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> ScreenSevenDayTotalTop10()
        {
            IList<CBMkTrafficStatisticsPagePVAndIPReport> result = new List<CBMkTrafficStatisticsPagePVAndIPReport>();

            #region 测试SQL

            /*
             --PV 统计总数
            select count(sysno) from MkTrafficStatistics where screen is not null and VisitingTime > to_date('2014/01/01 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2014/01/8 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
            select * from 
            (
                --67为统计时间段内有来源PV总数
                select screen,round((count(sysno)/67)*100,2) as rateNum
                    from MkTrafficStatistics
                    where VisitingTime >
                            to_date('2014/01/01 01:00:00', 'yyyy/mm/dd HH24:mi:ss')
                        and VisitingTime <
                            to_date('2014/01/8 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                        and screen is not null 
                    group by screen     
            )
            where rateNum > 0 and rownum < 11
            order by rateNum desc
             * */
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //PV 统计总数
                var totalPV =
                    _context.Sql(
                        "select count(sysno) from MkTrafficStatistics where screen is not null and VisitingTime > to_date('" +
                        DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                        " 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" +
                        DateTime.Now.ToString("yyyy/MM/dd") +
                        " 23:59:59', 'yyyy/mm/dd HH24:mi:ss')")
                            .QuerySingle<int>();

                if (totalPV == 0)
                    return result;

                result = _context.Sql(@" select * from 
                                        (
                                           select screen as Item,round((count(sysno)/" + totalPV + @")*100,2) as TotalPV
                                            from MkTrafficStatistics
                                               where VisitingTime > to_date('" +
                                                                    DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                                                                    @" 01:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                                                     and VisitingTime < to_date('" +
                                                                        DateTime.Now.ToString("yyyy/MM/dd") +
                                                                        @" 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                                                     and screen is not null 
                                               group by screen                                              
                                        )
                                        where TotalPV > 0 and rownum < 11
                                        order by TotalPV desc").QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();

            }

            return result;
        }

        /// <summary>
        /// 7天内:屏幕分辨率比例Top10
        /// </summary>
        /// <returns>屏幕分辨率比例结果</returns>
        /// <remarks>2013-12-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> BrowserSevenDayTotalTop10()
        {
            IList<CBMkTrafficStatisticsPagePVAndIPReport> result = new List<CBMkTrafficStatisticsPagePVAndIPReport>();

            #region 测试SQL

            /*
             --PV 统计总数
            select count(sysno) from MkTrafficStatistics where screen is not null and VisitingTime > to_date('2014/01/01 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('2014/01/8 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
            select * from 
            (
                --67为统计时间段内有来源PV总数
                select BrowserType,round((count(sysno)/67)*100,2) as rateNum
                    from MkTrafficStatistics
                    where VisitingTime >
                            to_date('2014/01/01 01:00:00', 'yyyy/mm/dd HH24:mi:ss')
                        and VisitingTime <
                            to_date('2014/01/8 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                        and BrowserType is not null 
                    group by BrowserType     
            )
            where rateNum > 0 and rownum < 11
            order by rateNum desc
             * */
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //PV 统计总数
                var totalPV =
                    _context.Sql(
                        "select count(sysno) from MkTrafficStatistics where BrowserType is not null and VisitingTime > to_date('" +
                        DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                        " 01:00:00', 'yyyy/mm/dd HH24:mi:ss') and VisitingTime < to_date('" +
                        DateTime.Now.ToString("yyyy/MM/dd") +
                        " 23:59:59', 'yyyy/mm/dd HH24:mi:ss')")
                            .QuerySingle<int>();

                if (totalPV == 0)
                    return result;

                result = _context.Sql(@" select * from 
                                        (
                                           select BrowserType as Item,round((count(sysno)/" + totalPV + @")*100,2) as TotalPV
                                            from MkTrafficStatistics
                                               where VisitingTime > to_date('" +
                                                                    DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).ToString("yyyy/MM/dd") +
                                                                    @" 01:00:00', 'yyyy/mm/dd HH24:mi:ss') 
                                                     and VisitingTime < to_date('" +
                                                                        DateTime.Now.ToString("yyyy/MM/dd") +
                                                                        @" 23:59:59', 'yyyy/mm/dd HH24:mi:ss')
                                                     and BrowserType is not null 
                                               group by BrowserType                                              
                                        )
                                        where TotalPV > 0 and rownum < 11
                                        order by TotalPV desc").QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();

            }

            return result;
        }

        #endregion

        /// <summary>
        /// 指定时间段内PV访问量
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束数据</param>
        /// <param name="spliteNumber">分片段数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public override IList<int> PvForRangeTime(DateTime startTime, DateTime endTime, int spliteNumber = 6)
        {
            var spliteDay = Math.Abs(Math.Round(double.Parse((startTime.Subtract(endTime).Days / spliteNumber).ToString())));

            DateTime tempStartDate = startTime;
            DateTime tempEndDate;
            tempStartDate = startTime;

            IList<int> PVList = new List<int>();

            StringBuilder PV_sql = new StringBuilder();
            using (var _context = Context.UseSharedConnection(true))
            {

                for (int i = 0; i < spliteNumber; i++)
                {

                    if (i > 0)
                    {
                        tempStartDate = tempStartDate.AddDays(spliteDay);
                        tempStartDate = (tempStartDate.CompareTo(endTime) < 0) ? tempStartDate : endTime;
                    }

                    tempEndDate = tempStartDate.AddDays(spliteDay);
                    tempEndDate = (tempEndDate.CompareTo(endTime) < 0) ? tempEndDate : endTime;

                    if (PV_sql.Length > 0)
                    {
                        PV_sql.Append(System.Environment.NewLine);
                        PV_sql.Append(" union all ");
                    }

                    PV_sql.Append(@"
                    select 
                    count(sysno)
                    from MkTrafficStatistics mkt
                    where
                    mkt.VisitingTime > to_date('" + tempStartDate.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd hh24:mi:ss') and mkt.VisitingTime < to_date('" + tempEndDate.ToString("yyyy/MM/dd HH:mm:ss") + "','yyyy/mm/dd hh24:mi:ss') ");
                    PV_sql.Append(System.Environment.NewLine);
                }

                PVList = _context.Sql(PV_sql.ToString()).QueryMany<int>();
            }
            return PVList;
        }

        /// <summary>
        /// 指定时间段内IP访问量
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束数据</param>
        /// <param name="spliteNumber">分片段数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public override IList<int> IpForRangeTime(DateTime startTime, DateTime endTime, int spliteNumber = 6)
        {
            var spliteDay = Math.Abs(Math.Round(double.Parse((startTime.Subtract(endTime).Days / spliteNumber).ToString())));

            DateTime tempStartDate = startTime;
            DateTime tempEndDate;
            tempStartDate = startTime;

            IList<int> PVList = new List<int>();

            StringBuilder PV_sql = new StringBuilder();
            using (var _context = Context.UseSharedConnection(true))
            {

                for (int i = 0; i < spliteNumber; i++)
                {

                    if (i > 0)
                    {
                        tempStartDate = tempStartDate.AddDays(spliteDay);
                        tempStartDate = (tempStartDate.CompareTo(endTime) < 0) ? tempStartDate : endTime;
                    }

                    tempEndDate = tempStartDate.AddDays(spliteDay);
                    tempEndDate = (tempEndDate.CompareTo(endTime) < 0) ? tempEndDate : endTime;

                    if (PV_sql.Length > 0)
                    {
                        PV_sql.Append(System.Environment.NewLine);
                        PV_sql.Append(" union all ");
                    }

                    PV_sql.Append(@"
                    select 
                    count(VisitorsSign)
                    from MkTrafficStatistics mkt
                    where
                    mkt.VisitingTime > to_date('" + tempStartDate.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd hh24:mi:ss') and mkt.VisitingTime < to_date('" + tempEndDate.ToString("yyyy/MM/dd HH:mm:ss") + @"','yyyy/mm/dd hh24:mi:ss') 
                    group by VisitorsSign ");
                    PV_sql.Append(System.Environment.NewLine);
                }

                PVList = _context.Sql(PV_sql.ToString()).QueryMany<int>();
            }
            return PVList;
        }

        /// <summary>
        /// 页面PV，IP统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="recordNumber">记录条数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> PagePvAndIpTotal(DateTime startTime, DateTime endTime, int recordNumber = 10)
        {
            recordNumber++;
            return Context.Sql(@"
                select rownum as id,tempTable.* 
                from
                (
                select URLADDRESS as Item,sum(pv) as TotalPV,sum(ip) as TotalIP from
                (  
                  select 
                    URLADDRESS,count(URLADDRESS) as pv,count(distinct visitorssign) as IP    
                  from
                  (
                    select
                      mkt.URLADDRESS,mkt.visitorssign
                    from 
                      MkTrafficStatistics mkt 
                    where 
                      mkt.VisitingTime > @startTime and mkt.VisitingTime <  @endTime) 
                  ) tempTable  
                  group by URLADDRESS,visitorssign
                ) abc
                group by URLADDRESS
                order by TotalPV desc
                ) tempTable
                where 
                 rownum<@recordNumber
            ")
             .Parameter("startTime", startTime)
             .Parameter("endTime", endTime)
             .Parameter("recordNumber", recordNumber)
             .QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();
        }

        /// <summary>
        /// 页面分类页面PV，IP统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="recordNumber">记录条数</param>
        /// <returns>返回数据 [PV,IP]</returns>
        /// <remarks>2013-11-18 邵斌 创建</remarks>
        public override IList<CBMkTrafficStatisticsPagePVAndIPReport> PageSearchPvAndIpTotal(DateTime startTime, DateTime endTime, int recordNumber = 10)
        {
            recordNumber++;
            return Context.Sql(@"
                select rownum as id,tempTable.* 
                from
                (
                select URLADDRESS as Item,sum(pv) as TotalPV,sum(ip) as TotalIP from
                (  
                  select 
                    URLADDRESS,count(URLADDRESS) as pv,count(distinct visitorssign) as IP    
                  from
                  (
                    select
                      mkt.URLADDRESS,mkt.visitorssign
                    from 
                      MkTrafficStatistics mkt 
                    where 
                      mkt.VisitingTime > @startTime and mkt.VisitingTime <  @endTime) 
                  ) tempTable  
                  group by URLADDRESS,visitorssign
                ) abc
                group by URLADDRESS
                order by TotalPV desc
                ) tempTable
                where 
                 rownum<@recordNumber
            ")
             .Parameter("startTime", startTime)
             .Parameter("endTime", endTime)
             .Parameter("recordNumber", recordNumber)
             .QueryMany<CBMkTrafficStatisticsPagePVAndIPReport>();
        }

        #endregion
    }
}
