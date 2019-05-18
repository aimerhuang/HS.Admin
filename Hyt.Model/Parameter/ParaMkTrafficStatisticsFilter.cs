using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 流量分析查询过滤条件
    /// </summary>
    /// <remarks>2013-11-13 邵斌 创建</remarks>
    public class ParaMkTrafficStatisticsFilter
    {
        /// <summary>
        /// 页面地址
        /// </summary>
        public string UrlAddress { get; set; }

        /// <summary>
        /// 访问开始时间
        /// </summary>
        public DateTime StartVisitingTime { get; set; }

        /// <summary>
        /// 访问结束时间
        /// </summary>
        public DateTime EndVisitingTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        /// <remarks>可以做IP段查询</remarks>
        public string IpAddress { get; set; }

        /// <summary>
        /// IP地址归属地
        /// </summary>
        public int IpLocation { get; set; }

        /// <summary>
        /// 来源域
        /// </summary>
        public string SourceDomain { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SeoKeyword { get; set; }

        /// <summary>
        /// 流量器类型
        /// </summary>
        public string BrowserType { get; set; }

        /// <summary>
        /// 屏幕类型
        /// </summary>
        public string Screen { get; set; }

        /// <summary>
        /// 加载时间时间段  开始时间
        /// </summary>
        public int StartLoadTime { get; set; }

        /// <summary>
        /// 加载时间时间段  结束时间
        /// </summary>
        public int EndLoadTime { get; set; }

        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        /// <remarks>性别：不限（0）、男（1）、女（2）</remarks>
        public int Gender { get; set; }

        /// <summary>
        /// 年龄上限
        /// </summary>
        public int MaxLimitAge { get; set; }

        /// <summary>
        /// 年龄下限
        /// </summary>
        public int MinLimitAge { get; set; }
    }
}
