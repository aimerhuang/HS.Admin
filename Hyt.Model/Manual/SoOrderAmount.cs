using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
  /// <summary>
  /// 订单金额相关字段（方便随时刷新订单价格信息)
  /// </summary>
    /// <remarks>2013-08-30 朱成果 创建</remarks>
   public class SoOrderAmount
   {
       /// <summary>
       /// 商品销售价合计
       /// 商品销售价合计(明细销售单价*数量,之和.优惠前金额)
       /// </summary>
       public decimal ProductAmount { get; set; }
       /// <summary>
       /// 商品折扣金额
       /// 商品折扣金额(明细折扣金额之和)
       /// </summary>
       public decimal ProductDiscountAmount { get; set; }
       /// <summary>
       /// 商品调价金额合计
       /// 商品调价金额合计(明细调价金额之和)
       /// </summary>
       public decimal ProductChangeAmount { get; set; }
       /// <summary>
       /// 优惠券金额
       /// 优惠券金额(多张优惠券合计)
       /// </summary>
       public decimal CouponAmount { get; set; }
       /// <summary>
       /// 运费折扣金额
       /// 运费折扣金额
       /// </summary>
       public decimal FreightDiscountAmount { get; set; }
       /// <summary>
       /// 运费调价金额
       /// 运费调价金额(正负值)
       /// </summary>
       public decimal FreightChangeAmount { get; set; }
       /// <summary>
       /// 订单折扣金额
       /// OrderDiscountAmount
       /// </summary>
       public decimal OrderDiscountAmount { get; set; }
       /// <summary>
       /// 销售单总金额
       /// 销售单总金额(商品销售价合计+运费-商品折扣金额-运费折扣金额-订单折扣金额-优惠券金额)
       /// </summary>
       public decimal OrderAmount { get; set; }
       /// <summary>
       /// 运费金额
       /// 运费金额(折扣前金额)
       /// </summary>
       public decimal FreightAmount { get; set; }
       /// <summary>
       /// 总金额中现金支付部分
       /// 总金额中现金支付部分
       /// </summary>
       public decimal CashPay { get; set; }
       /// <summary>
       /// 总金额中惠源币支付部分
       /// 总金额中惠源币支付部分
       /// </summary>
       public decimal CoinPay { get; set; }
       /// <summary>
       /// 系统编号
       /// 
       /// </summary>
       public int SysNo { get; set; }
       /// <summary>
       /// 时间戳
       /// </summary>
       public DateTime Stamp { get; set; }
   }
}
