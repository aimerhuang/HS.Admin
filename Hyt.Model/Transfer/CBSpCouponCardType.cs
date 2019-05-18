using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 优惠卡类型扩展
    /// </summary>
    /// <remarks>
    /// 2014-01-09 朱成果 创建
    /// </remarks>
  public class CBSpCouponCardType : SpCouponCardType
  {
      /// <summary>
      /// 关联的优惠券
      /// </summary>
      public List<CBSpCouponCardAssociate> AssociateItem { get; set; }
  }
}
