using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 二次销售详情
    /// </summary>
    /// <remarks>2014-9-23 朱成果 创建</remarks>
    public  class CBTwoSaleDetail
    {
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Moblie { get; set; }

        /// <summary>
        /// ERP编码
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 优惠
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal SaleAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal RealSaleAmount { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public string DeliveryType { get; set; }

        /// <summary>
        /// 结算状态
        /// </summary>
        public string SettlementState { get; set; }
   
    }
}
