using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 门店会员消费报表查询
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBReportShopCustomerConsume
    {
        /// <summary>
        /// 门店编号
        /// </summary>
        public int 门店编号 { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string 门店名称 { get; set; }
        /// <summary>
        /// 会员消费笔数
        /// </summary>
        public int 会员消费笔数 { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string 日期 { get; set; }
    }
}
