using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 优惠券扩展
    /// </summary>
    /// <remarks>2013-08-27 黄志勇 创建</remarks>
    [Serializable]
    public class CBSpCoupon: SpCoupon
    {
        /// <summary>
        /// 优惠券
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 促销名称
        /// </summary>
        public string PromotionName { get; set; }

        /// <summary>
        /// 优惠卡编号
        /// </summary>
        public string CouponCardNo { get; set; }
    }
}
