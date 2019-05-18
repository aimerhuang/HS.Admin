using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 优惠券卡号管理
    /// </summary>
    /// <remarks>2013-08-27 余勇 创建</remarks>
    [Serializable]
    public class CBSpCouponCard
    {
        /// <summary>
        /// 优惠券卡系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 优惠券卡号
        /// </summary>
        public string CouponCardNo { get; set; }

        /// <summary>
        /// 优惠券卡类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ActivationTime { get; set; }

        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime TerminationTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
