using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 购物车对象
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ShoppingCartApp
    {

        /// <summary>
        /// 商品销售合计金额(优惠前的金额)
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 结算金额(=商品销售合计金额+运费金额-商品优惠金额-运费优惠金额-订单优惠金额)
        /// OrderSettlementAmount = ProductAmount+FreightAmount-ProductDiscountAmount-FreightDiscountAmount-SettlementDiscountAmount
        /// </summary>
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>
        /// 购物车组
        /// </summary>
        public IList<CrShoppingCartGroup> ShoppingCartGroups { get; set; }

        /// <summary>
        /// 购物车促销信息
        /// </summary>
        public IList<CrShoppingCartGroupPromotion> GroupPromotions { get; set; }
       
    }

    /// <summary>
    /// 购物车计算明细金额
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class ShoppingAmount
    {
        /// <summary>
        /// 商品销售合计金额(优惠前的金额)
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 结算金额(=商品销售合计金额+运费金额-商品优惠金额-运费优惠金额-订单优惠金额)
        /// OrderSettlementAmount = ProductAmount+FreightAmount-ProductDiscountAmount-FreightDiscountAmount-SettlementDiscountAmount
        /// </summary>
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// 运费金额(优惠前的金额)
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 总优惠金额
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }
    }

    /// <summary>
    /// 优惠劵
    /// </summary>
    /// <remarks>2013-8-30 杨浩 添加</remarks>
    public class Coupon
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 促销系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }
        /// <summary>
        /// 优惠券代码
        /// </summary>
        public string CouponCode { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 所需消费金额
        /// </summary>
        public decimal RequirementAmount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 文本和背景颜色值
        /// </summary>
        public string Color { get; set; }
    }
}
