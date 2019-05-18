using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 退货单明细实体
    /// </summary>
    /// <remarks>2013-8-29 陶辉 创建</remarks>
    public class UpGradeRmaItem
    {
        /// <summary>
        /// 第三方订单明细编号
        /// </summary>
        public string MallOrderItemId { get; set; }

        /// <summary>
        /// 第三方商品编号
        /// </summary>
        public string MallProductCode { get; set; }

        /// <summary>
        /// 第三方商品属性
        /// </summary>
        public string MallProductAttrs { get; set; }

        /// <summary>
        /// 商城商品编号
        /// </summary>
        public string HytProductCode { get; set; }

        /// <summary>
        /// 商城商品编号
        /// </summary>
        public string HytProductErpCode { get; set; }

        /// <summary>
        /// 第三方商品名称
        /// </summary>
        public string MallProductName { get; set; }

        /// <summary>
        /// 第三方商品退款金额
        /// </summary>
        public decimal MallRefundFee { get; set; }

        /// <summary>
        /// 退货商品数量
        /// </summary>
        public int MallQuantity { get; set; }

        /// <summary>
        /// 商品明细优惠总金额
        /// </summary>
        public decimal DiscountFee { get; set; }

        /// <summary>
        /// 商城退款金额
        /// </summary>
        public decimal HytRmaAmount { get; set; }

        /// <summary>
        /// 商城商品名称
        /// </summary>
        public string HytProductName { get; set; }

        /// <summary>
        /// 商城商品单价
        /// </summary>
        public decimal HytProductPrice { get; set; }

        /// <summary>
        /// 商城出库商品数量
        /// </summary>
        public int ProductQuantity { get; set; }

        /// <summary>
        /// 商城出库单明细编号
        /// </summary>
        public int StockOutItemSysNo { get; set; }
    }
}
