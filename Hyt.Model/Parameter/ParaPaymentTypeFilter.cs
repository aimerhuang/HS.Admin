using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 支付方式查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-08-20 郑荣华 创建
    /// </remarks>
    public class ParaPaymentTypeFilter
    {
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }
        /// <summary>
        /// 是否网上支付：是（1）、否（0）
        /// </summary>
        public int? IsOnlinePay { get; set; }

        /// <summary>
        /// 前台是否可见：可见（1）、不可见（0）
        /// </summary>
        public int? IsOnlineVisible { get; set; }

        /// <summary>
        /// 支付类型：预付（10）、到付（20）
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// 是否需要卡号：是(1),否(0)
        /// </summary>
        public int? RequiredCardNumber { get; set; }

        /// <summary>
        /// 状态：有效（1）、无效（0）
        /// </summary>
        public int? Status { get; set; }
    }
}
