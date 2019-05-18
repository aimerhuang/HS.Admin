using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 优惠卡类型关联扩展
    /// </summary>
    /// <remarks>
    /// 2014-01-09 朱成果 创建
    /// </remarks>
   public  class CBSpCouponCardAssociate : SpCouponCardAssociate
   {
       /// <summary>
       /// 优惠券代码
       /// </summary>
       public string CouponCode { get; set; }

       /// <summary>
       /// 优惠券金额
       /// </summary>
       public decimal CouponAmount { get; set; }

       /// <summary>
       /// 优惠券描述
       /// </summary>
       public string Description { get; set; }
    }
}
