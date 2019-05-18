using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 经销商支付类型
    /// </summary>
    /// <remarks>2016-5-18 杨浩 创建</remarks>
    [Serializable]
    public partial class DsDealerPayType
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 分销商系统编号
        /// </summary>
        [Description("分销商系统编号")]
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 支付方式系统编号
        /// </summary>
        [Description("支付方式系统编号")]
        public int PaymentTypeSysNo { get; set; }

        /// <summary>
        /// 支付方的商户编号或者主要key
        /// </summary>
        [Description("支付方的商户编号或者主要key")]
        public string AppKey { get; set; }
        
        /// <summary>
        /// 支付方的密钥
        /// </summary>
        [Description("支付方的密钥")]
        public string AppSecret { get; set; }
        
    }
}
