using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 优惠卡及优惠卡类型、优惠卷信息
    /// </summary>
    /// <remarks>2014-01-08 朱家宏 创建</remarks>
   [DataContract]
    public class CBCouponCard
    {
        /// <summary>
        /// 优惠卡
        /// </summary>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
       [DataMember] 
       public SpCouponCard CouponCard { get; set; }

        /// <summary>
        /// 优惠卡类型
        /// </summary>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        [DataMember]
       public SpCouponCardType CouponCardType { get; set; }

        /// <summary>
        /// 优惠卷
        /// </summary>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        [DataMember]
       public IList<SpCoupon> Coupons { get; set; }

        /// <summary>
        /// 优惠卡关联
        /// </summary>
        /// <remarks>2014-01-08 朱家宏 创建</remarks>
        [DataMember]
       public IList<SpCouponCardAssociate> Associations { get; set; }
    }
}
