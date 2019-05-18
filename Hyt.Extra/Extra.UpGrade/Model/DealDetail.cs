using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 拍拍订单详情
    /// </summary>
    /// <remarks>2013-11-10 陶辉 创建</remarks>
    public class DealDetail
    {
        /// <summary>
        /// 子订单id
        /// </summary>
        public string dealSubCode { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode { get; set; }

        /// <summary>
        /// 订单的商品快照编码
        /// </summary>
        public string itemCodeHistory { get; set; }

        /// <summary>
        /// 商家编码
        /// </summary>
        public string itemLocalCode { get; set; }

        /// <summary>
        /// 商品的库存唯一标识码,由拍拍平台生成
        /// </summary>
        public string skuId { get; set; }

        /// <summary>
        /// 商品库存编码
        /// </summary>
        public string stockLocalCode { get; set; }

        /// <summary>
        /// 买家下单时选择的库存属性
        /// </summary>
        public string stockAttr { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName { get; set; }

        /// <summary>
        /// 买家下单时的商品价格
        /// </summary>
        public decimal itemDealPrice { get; set; }

        /// <summary>
        /// 订单的调整价格:正数为订单加价,负数为订单减价
        /// </summary>
        public decimal itemAdjustPrice { get; set; }

        /// <summary>
        /// 购买商品的红包值、折扣优惠价
        /// </summary>
        public decimal itemDiscountFee { get; set; }

        /// <summary>
        /// 购买的数量
        /// </summary>
        public int itemDealCount { get; set; }

        /// <summary>
        /// 网购现金券金额
        /// </summary>
        public string wanggouQuanAmt { get; set; }
    }
}
