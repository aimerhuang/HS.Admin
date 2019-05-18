using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Order
{
    /// <summary>
    /// 利嘉订单同步模型
    /// </summary>
    /// <remarks>2017-5-18 罗勤尧 创建</remarks>
   public  class LiJiaOrderModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
       public int SourceOrderNo { get; set; }
       /// <summary>
       ///会员ID
       /// </summary>
       public int MemberId { get; set; }
        /// <summary>
       ///会员名称
        /// </summary>
       public string MemberName { get; set; }
        /// <summary>
       /// 收货人
        /// </summary>
       public string ReceiptName { get; set; }
        /// <summary>
       /// 联系电话
        /// </summary>
       public string ReceiptPhoneNumber { get; set; }
        /// <summary>
       /// 身份证
        /// </summary>
       public string IDNumber { get; set; }
        /// <summary>
       /// 收货地址
        /// </summary>
       public string ReceiptAddress { get; set; }
        /// <summary>
       /// 备注
        /// </summary>
       public string Memo { get; set; }
        /// <summary>
       /// 销售日期(格式 2017-05-16 10:55:11)
        /// </summary>
       public string OrderCreateTime { get; set; }
        /// <summary>
       /// 运费(人民币)
        /// </summary>
       public decimal ShippingTotalAmount { get; set; }
        /// <summary>
       /// 订单总金额(人民币)
        /// </summary>
       public decimal OrderTotalAmount { get; set; }
        /// <summary>
       /// 商品总金额(人民币)
        /// </summary>
       public decimal GoodsTotalAmount { get; set; }
        /// <summary>
       /// 付款总金额(人民币)
        /// </summary>
       public decimal OrderPaymentAmount { get; set; }

       /// <summary>
       /// 付款时间
       /// </summary>
       public string OrderPaymentTime { get; set; }
       /// <summary>
       /// 支付方式
       /// </summary>
       public string OrderPaymentType { get; set; }
       /// <summary>
       /// 支付流水号
       /// </summary>
       public string OrderPaymentNo { get; set; }
       /// <summary>
       /// 订单商品明细数据集
       /// </summary>
       public List<LiJiaOrderItem> OrderItem { get; set; }
       /// <summary>
       /// 外币运费
       /// </summary>
       public decimal ForeignShippingPrice { get; set; }
       /// <summary>
       /// 外币币种CNY=人民币,USD=美元,EUR=欧元,GBP=英镑,HKD=港元,JPY=日元,NZD=纽币,AUD=澳币填写币种英文标识,如:USD
       /// </summary>
       public string ForeignCurency { get; set; }
       /// <summary>
       /// 汇率(外币对人民币汇率)
       /// </summary>
       public float ForeignRate { get; set; }
       /// <summary>
       /// 外币总金额
       /// </summary>
       public decimal ForeignTotalAmount { get; set; }
    }
}
