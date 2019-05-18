using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 订单支付日志
    /// </summary>
    /// <remarks>2017-04-02 杨浩 创建</remarks>
    public class SoOrderPayLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 提交订单号
        /// </summary>
        public string SubmitOrderNumber { get; set; }
        /// <summary>
        /// 支付类型系统编号
        /// </summary>
        public int PaymentTypeSysNo { get; set;}
        /// <summary>
        /// 支付状态 未支付（10）、已支付（20）、支付异常（30）
        /// </summary>
        public int Status { get; set; }
    }
}
