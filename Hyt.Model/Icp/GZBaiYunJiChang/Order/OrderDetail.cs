using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Order
{
    /// <summary>
    /// 电子订单详情信息
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        /// 企业电子订单编号
        /// </summary>
        /// 
        [XmlElement(ElementName = "EntOrderNo")]
        public string EntOrderNo { get; set; }

        /// <summary>
        /// 电子订单状态
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderStatus")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        /// 
        [XmlElement(ElementName = "PayStatus")]
        public string PayStatus { get; set; }

        /// <summary>
        /// 订单商品总额
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderGoodTotal")]
        public string OrderGoodTotal { get; set; }

        /// <summary>
        /// 订单商品总额币制
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderGoodTotalCurr")]
        public string OrderGoodTotalCurr { get; set; }

        /// <summary>
        /// 订单运费
        /// </summary>
        /// 
        [XmlElement(ElementName = "Freight")]
        public string Freight { get; set; }

        /// <summary>
        /// 运费币制
        /// </summary>
        /// 
        [XmlElement(ElementName = "FreightCurr")]
        public string FreightCurr { get; set; }

        /// <summary>
        /// 税款
        /// </summary>
        /// 
        [XmlElement(ElementName = "Tax")]
        public string Tax { get; set; }

        /// <summary>
        /// 税款币制
        /// </summary>
        /// 
        [XmlElement(ElementName = "TaxCurr")]
        public string TaxCurr { get; set; }

        /// <summary>
        /// 抵付金额
        /// </summary>
        /// 
        [XmlElement(ElementName = "OtherPayment")]
        public string OtherPayment { get; set; }

        /// <summary>
        /// 币制代码
        /// </summary>
        /// 
        [XmlElement(ElementName = "OtherPayCurr")]
        public string OtherPayCurr { get; set; }

        /// <summary>
        /// 抵付说明
        /// </summary>
        /// 
        [XmlElement(ElementName = "OtherPayNotes")]
        public string OtherPayNotes { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        /// 
        [XmlElement(ElementName = "OtherCharges")]
        public string OtherCharges { get; set; }

        /// <summary>
        /// 币制代码
        /// </summary>
        /// 
        [XmlElement(ElementName = "OtherChargesCurr")]
        public string OtherChargesCurr { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        /// 
        [XmlElement(ElementName = "ActualAmountPaid")]
        public string ActualAmountPaid { get; set; }

        /// <summary>
        /// 实际支付币制
        /// </summary>
        /// 
        [XmlElement(ElementName = "ActualCurr")]
        public string ActualCurr { get; set; }

        /// <summary>
        /// 收货人名称
        /// </summary>
        /// 
        [XmlElement(ElementName = "RecipientName")]
        public string RecipientName { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        /// 
        [XmlElement(ElementName = "RecipientAddr")]
        public string RecipientAddr { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        /// 
        [XmlElement(ElementName = "RecipientTel")]
        public string RecipientTel { get; set; }

        /// <summary>
        /// 收货人所在国
        /// </summary>
        /// 
        [XmlElement(ElementName = "RecipientCountry")]
        public string RecipientCountry { get; set; }

        /// <summary>
        /// 下单人账户   可空
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderDocAcount")]
        public string OrderDocAcount { get; set; }

        /// <summary>
        /// 下单人姓名
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderDocName")]
        public string OrderDocName { get; set; }

        /// <summary>
        /// 下单人证件类型  可空
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderDocType")]
        public string OrderDocType { get; set; }

        /// <summary>
        /// 下单人证件号  可空
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderDocId")]
        public string OrderDocId { get; set; }

        /// <summary>
        ///下单人电话
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderDocTel")]
        public string OrderDocTel { get; set; }

        /// <summary>
        ///订单日期
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrderDate")]
        public string OrderDate { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        /// 
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }

        /// <summary>
        ///订单商品明细
        /// </summary>
        /// 
        [XmlElement(ElementName = "GoodsList")]
        public GoodsList GoodsList { get; set; }
    }
}
