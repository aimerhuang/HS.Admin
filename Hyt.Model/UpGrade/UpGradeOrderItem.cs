using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 商城订单明细
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    public class UpGradeOrderItem
    {
        /// <summary>
        /// 第三方订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 第三方订单SN编号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 第三方订单明细编号
        /// </summary>
        public string MallOrderItemId { get; set; }

        /// <summary>
        /// 第三方商品属性值
        /// </summary>
        public string MallProductAttrId { get; set; }

        /// <summary>
        /// 第三方商品属性名称
        /// </summary>
        public string MallProductAttrs { get; set; }

        /// <summary>
        /// 第三方商品名称
        /// </summary>
        public string MallProductName { get; set; }

        /// <summary>
        /// 第三方商品编号
        /// </summary>
        public string MallProductCode { get; set; }

        /// <summary>
        /// 商城商品编号
        /// </summary>
        public int HytProductSysNo { get; set; }

        /// <summary>
        /// 商城产品ERP编号
        /// </summary>
        public string HytProductErpCode { get; set; }

        /// <summary>
        /// 商城商品名称
        /// </summary>
        public string HytProductName { get; set; }

        /// <summary>
        /// 商城分销商价格
        /// </summary>
        public decimal HytPrice { get; set; }

        /// <summary>
        /// 第三方商品销售价格
        /// </summary>
        public decimal MallPrice { get; set; }

        /// <summary>
        /// 第三方商品销售金额 (数量*第三方商品销售价格)
        /// </summary>
        public decimal MallAmount { get; set; }

        /// <summary>
        /// 第三方商品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 第三方商品明细优惠总金额
        /// </summary>
        public decimal DiscountFee { get; set; }

        /// <summary>
        /// 商品销售类型：普通(10),团购(20),秒杀(30),抢购(40),拍卖(50),组合(60),赠品(90)
        /// </summary>
        public int ProductSalesType { get; set; }
        /// <summary>
        /// 商品类型：保税(1),香港直邮(2),海外直邮(3)
        /// </summary>
        public int ProductType { get; set; }

        /// <summary>
        /// 关联商城订单明细
        /// </summary>
        public SoOrderItem HytOrderItem { get; set; }
    }
}
