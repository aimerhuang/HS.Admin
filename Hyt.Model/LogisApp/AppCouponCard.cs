using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 优惠卡及优惠卡类型、优惠卷信息
    /// </summary>
    /// <remarks>2014-03-10 周唐炬 创建</remarks>
    public class AppCouponCard : BaseEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 优惠券卡类型系统编号
        /// </summary>
        public int CardTypeSysNo { get; set; }
        /// <summary>
        /// 优惠卡号码
        /// </summary>
        public string CouponCardNo { get; set; }
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ActivationTime { get; set; }
        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime TerminationTime { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 优惠卷
        /// </summary>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        public List<AppSpCoupon> Coupons { get; set; }

        /// <summary>
        /// 优惠卡类型
        /// </summary>
        public AppSpCouponCardType CouponCardType { get; set; }
    }

    /// <summary>
    /// 优惠卡及优惠卡类型、优惠卷信息
    /// </summary>
    /// <remarks>2014-03-10 周唐炬 创建</remarks>
    public class AppSpCouponCardType : BaseEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类型描述
        /// </summary>
        public string TypeDescription { get; set; }
        /// <summary>
        /// 有效时间起
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 有效时间止
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        public int Status { get; set; }
    }
}
