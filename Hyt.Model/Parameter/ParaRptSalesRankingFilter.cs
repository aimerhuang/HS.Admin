
using System;
using System.Collections.Generic;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 报表-销售排行查询参数
    /// </summary>
    /// <remarks>2013-10-22 朱家宏 创建</remarks>
    public struct ParaRptSalesRankingFilter
    {
        public DateTime? _endDate;

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //结束+1
                return _endDate == null ? (DateTime?) null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }
        /// <summary>
        /// 商品类别
        /// </summary>
        public IList<int> ProductCategories { get; set; }
        /// <summary>
        /// 日期范围
        /// </summary>
        public int? DateRange { get; set; }
        /// <summary>
        /// 取数大小
        /// </summary>
        public int TakingCount { get; set; }
        /// <summary>
        /// 商品销售的形式
        /// </summary>
        public string ProductSaleType { get; set; }
    }

    /// <summary>
    /// 日期查询范围
    /// </summary>
    /// <remarks>2013-10-22 朱家宏 创建</remarks>
    public enum ParaDateRange
    {
        /// <summary>
        /// 今天
        /// </summary>
        今天 = 0,
        /// <summary>
        /// 本周
        /// </summary>
        本周 = 1,
        /// <summary>
        /// 本月
        /// </summary>
        本月 = 2
    }

}
