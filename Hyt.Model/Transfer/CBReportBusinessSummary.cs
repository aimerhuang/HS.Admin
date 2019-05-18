using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 运营综述
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public  class CBReportBusinessSummary
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Description("日期")]
        public DateTime 日期 { get; set; }
        /// <summary>
        /// 流量
        /// </summary>
        [Description("流量")]
        public Int64 流量 { get; set; }
        /// <summary>
        /// 访客
        /// </summary>
        [Description("访客")]
        public Int64 访客 { get; set; }
        /// <summary>
        /// 下单数
        /// </summary>
        [Description("下单数")]
        public Int64 下单数 { get; set; }
        /// <summary>
        /// 销售额
        /// </summary>
        [Description("销售额")]
        public decimal 销售额 { get; set; }
        /// <summary>
        /// 退款总额
        /// </summary>
        [Description("退款总额")]
        public decimal 退款总额 { get; set; }
        /// <summary>
        /// 净销售额
        /// </summary>
        [Description("净销售额")]
        public decimal 净销售额 { get; set; }
        /// <summary>
        /// 客单价
        /// </summary>
        [Description("客单价")]
        public decimal 客单价 { get; set; }
        /// <summary>
        /// 转换率
        /// </summary>
        [Description("转换率")]
        public decimal 转换率 { get; set; }
    }

    /// <summary>
    /// 运营综述列表
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBReportBusinessSummaryMap
    {
        /// <summary>
        /// 图表横坐标
        /// </summary>
        public string[] xValues { get; set; }

        /// <summary>
        /// 图表纵坐标
        /// </summary>
        public object[] yValues { get; set; }

        /// <summary>
        /// 系列名称
        /// </summary>
        public string SerieName { get; set; }
    }
}
