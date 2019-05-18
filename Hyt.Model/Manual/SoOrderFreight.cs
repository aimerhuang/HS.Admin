using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 快递及运费
    /// </summary>
    /// <remarks> 2013-12-05 吴文强 创建</remarks>
    public partial class SoOrderFreight
    {
        /// <summary>
        /// 快递编号
        /// </summary>
        public int FreightSysNo { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public string FreightName { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 运费折扣
        /// </summary>
        public decimal FreightDiscountAmount { get; set; }

        /// <summary>
        /// 运费调价
        /// </summary>
        public decimal FreightChangeAmount { get; set; }

        /// <summary>
        /// 惠源币抵扣
        /// </summary>
        public decimal CoinDeduction { get; set; }

        /// <summary>
        /// 实际运费金额
        /// </summary>
        public decimal RealFreightAmount { get; set; }
    }
}