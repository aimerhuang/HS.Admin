using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 用于App接口的购物车对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class LogisShoppingCart
    {
        /// <summary>
        /// 商品销售合计金额(优惠前的金额)
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 商品优惠金额(优惠的金额)
        /// </summary>
        public decimal ProductDiscountAmount { get; set; }

        /// <summary>
        /// 运费金额(优惠前的金额)
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 运费优惠金额(运费的优惠金额)
        /// </summary>
        public decimal FreightDiscountAmount { get; set; }

        /// <summary>
        /// 结算优惠金额(订单总金额的优惠金额，不包括商品,运费优惠金额)
        /// </summary>
        public decimal SettlementDiscountAmount { get; set; }

        /// <summary>
        /// 优惠券代码
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 结算金额(=商品销售合计金额+运费金额-商品优惠金额-运费优惠金额-订单优惠金额)
        /// SettlementAmount = ProductAmount+FreightAmount-ProductDiscountAmount-FreightDiscountAmount-SettlementDiscountAmount-CouponAmount
        /// </summary>
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// 购物车组
        /// </summary>
        public IList<CrShoppingCartGroup> ShoppingCartGroups { get; set; }

        /// <summary>
        /// 购物车促销信息
        /// </summary>
        public IList<CrShoppingCartGroupPromotion> GroupPromotions { get; set; }
    }
}
