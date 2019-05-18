using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 支付方式查询显示结果
    /// </summary>
    /// <remarks>
    /// 2013-08-08 郑荣华 创建
    /// </remarks>
    public class CBBsPaymentType : BsPaymentType
    {
        /// <summary>
        /// 支付类型名称 预付、到付
        /// </summary>        
        public string PaymentTypeName { set; get; }
    }
}
