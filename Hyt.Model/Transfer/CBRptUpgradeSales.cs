using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 升舱销量报表
    /// </summary>
    /// <remarks>2014-04-16 朱家宏 创建</remarks>
    public class CBRptUpgradeSales
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime StatsDate { get; set; }

        /// <summary>
        /// 分销商
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 商城
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 百城达升舱金额
        /// </summary>
        public decimal BcdSum { get; set; }

        /// <summary>
        /// 第三方升舱金额
        /// </summary>
        public decimal DsfSum { get; set; }

        /// <summary>
        /// 百城达订单数
        /// </summary>
        public int BcdCount { get; set; }

        /// <summary>
        /// 第三方订单数
        /// </summary>
        public decimal DsfCount { get; set; }

    }

}
