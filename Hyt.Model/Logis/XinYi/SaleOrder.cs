using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Logis.XinYi
{
    /// <summary>
    /// 心怡销售单
    /// </summary>
    /// <remarks>2015-10-14 杨浩 创建</remarks>
    [Serializable]
    public class SaleOrder
    {
        /// <summary>
        /// 货主编号(由心怡统一分配)(必选)
        /// </summary>
        public string OnNumber { get; set; }
        /// <summary>
        /// 仓库编号(由心怡统一分配)(必选)
        /// </summary>
        public string WhNumber { get; set; }
        /// <summary>
        /// 进仓订单单编号(来源编号)(必选)
        /// </summary>
        public string SoNumber { get; set; }
        /// <summary>
        /// 订单日期(格式：yyyy-MM-dd HH:mm:ss)(必选)
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 销售价(可选)
        /// </summary>
        public decimal ProPricesum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Qtysum { get; set; }
        /// <summary>
        /// 订单优先级
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 订单人姓名
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 订单人证件类型(01:身份证、02:护照、03:其他)必选
        /// </summary>
        public string OrderDocType { get; set; }
        /// <summary>
        /// 订单人证件号
        /// </summary>
        public string OrderDocId { get; set; }
        /// <summary>
        /// 订单人电话
        /// </summary>
        public string OrderPhone { get; set; }
        /// <summary>
        /// 进口标识(I:进口；E:出口)
        /// </summary>
        public string IEFlag { get; set; }
        /// <summary>
        /// 运费(必选)
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 税款(必选)
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// 保价费
        /// </summary>
        public decimal ValuationFee { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// 收件人省市区代码此项为空时，以下省市区三项不能为空
        /// 如:120000来源省市区表(必选时，须第三级)即只能填第三级省市区代码
        /// </summary>
        public string RecipientProvincesCode { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string ReceiverProvince { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string ReceiverCity { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string ReceiverArea { get; set; }
        /// <summary>
        /// 收件人国别地区代码兼容代码及中文(必选)
        /// </summary>
        public string RecipientCode { get; set; }
        /// <summary>
        /// 收件人地址(必选)
        /// </summary>
        public string RecipientDetailedAddress { get; set; }
        /// <summary>
        /// 件人电话(必选)
        /// </summary>
        public string RecipientPhone { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string NoticeNo { get; set; }
        /// <summary>
        /// 收件人证件类型(01:身份证、02:护照、03:其他)
        /// </summary>
        public string RecDocType { get; set; }
        /// <summary>
        /// 收件人证件号
        /// </summary>
        public string RecDocId { get; set; }
        /// <summary>
        /// 收件人邮政编码
        /// </summary>
        public string ReceiverZipCode { get; set; }
        /// <summary>
        /// 收件人固定电话
        /// </summary>
        public string ReceiverPhone { get; set; }
        /// <summary>
        /// 支付交易号
        /// </summary>
        public string payTransactionNo { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal payAmount { get; set;}
        /// <summary>
        /// 支付货款
        /// </summary>
        public decimal payGoodsAmount { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public string payTimeStr { get; set; }
        /// <summary>
        /// 支付企业编号
        /// </summary>
        public string PayEntNo { get; set; }
        /// <summary>
        /// 购买人ID(可选)
        /// </summary>
        public string HZpurchaserId { get; set; }
        /// <summary>
        /// 支付企业名称(可选)
        /// </summary>
        public string payEnterpriseName { get; set; }
        /// <summary>
        /// 发卡行(可选)
        /// </summary>
        public string bank { get; set; }
        /// <summary>
        /// 支付ID(可选)
        /// </summary>
        public string payAccount { get; set; }
        /// <summary>
        /// 支付人姓名(可选)
        /// </summary>
        public string payerName { get; set; }
        /// <summary>
        /// 支付人证件类型
        /// </summary>
        public string payerDocumentType { get; set; }
        /// <summary>
        /// 支付人证件
        /// </summary>
        public string payerDocumentNumber { get; set; }
        /// <summary>
        /// 支付人手机号(可选)
        /// </summary>
        public string payerPhoneNumber { get; set; }
        /// <summary>
        /// 银行卡号(可选)
        /// </summary>
        public string assoBankAccount { get; set; }
        /// <summary>
        /// 平台交易单号(可选)
        /// </summary>
        public string orderSourceCode { get; set; }
        /// <summary>
        /// 收货方信息(可选)
        /// </summary>
        public string ReceivingParty { get; set; }
        /// <summary>
        /// 发货人(可选)
        /// </summary>
        public string Consignor { get; set; }
        /// <summary>
        /// 发货地址(可选)
        /// </summary>
        public string DeliveryAddress { get; set; }
        /// <summary>
        /// 快递公司(可选)
        /// </summary>
        public string CourierCompany { get; set; }
        /// <summary>
        /// 发票类型 增值税 普通发票(可选)
        /// </summary>
        public string BillType { get; set; }
        /// <summary>
        /// 发票抬头(可选)
        /// </summary>
        public string BillTitle { get; set; }
        /// <summary>
        /// 发票金额(可选)
        /// </summary>
        public string BillAccount { get; set; }
        /// <summary>
        /// 发票内容(可选)
        /// </summary>
        public string BillContent { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<SaleOrderDetail> SaleOrderDetail{ get; set; }

    }
}
