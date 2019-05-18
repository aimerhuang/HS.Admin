using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 配送方式支付方式关联查询参数类
    /// </summary>
    /// <remarks>
    /// 2013-08-02 郑荣华 创建
    /// </remarks>
    public class ParaBsDeliveryPaymentFilter
    {
        /// <summary>
        /// 配送方式系统编号
        /// </summary>
        public int? DeliverySysNo { get; set; }

        /// <summary>
        /// 配送类型，即父级系统编号
        /// </summary>
        public int? ParentSysNo { get; set; }

        /// <summary>
        /// 支付方式系统编号
        /// </summary>
        public int? PaymentSysNo { get; set; }

        /// <summary>
        /// 支付类型：预付（10）、到付（20） 
        /// </summary>
        public int? PaymentType { get; set; }
    }
}
