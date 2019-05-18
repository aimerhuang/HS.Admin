using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP支付方式
    /// </summary>
    /// <remarks>2013-08-01 周唐炬 创建</remarks>
    public class AppBsPaymentType : BaseEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 支付类型：预付（10）、到付（20）
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// 是否需要卡号：是(1),否(0)
        /// </summary>
        public int RequiredCardNumber { get; set; }
    }
}
