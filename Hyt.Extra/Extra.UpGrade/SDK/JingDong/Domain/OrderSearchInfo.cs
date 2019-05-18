using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderSearchInfo : JdObject{


         [XmlElement("orderId")]
public  		string
  orderId { get; set; }


         [XmlElement("venderId")]
public  		string
  venderId { get; set; }


         [XmlElement("orderType")]
public  		string
  orderType { get; set; }


         [XmlElement("payType")]
public  		string
  payType { get; set; }


         [XmlElement("orderTotalPrice")]
public  		string
  orderTotalPrice { get; set; }


         [XmlElement("orderSellerPrice")]
public  		string
  orderSellerPrice { get; set; }


         [XmlElement("orderPayment")]
public  		string
  orderPayment { get; set; }


         [XmlElement("freightPrice")]
public  		string
  freightPrice { get; set; }


         [XmlElement("sellerDiscount")]
public  		string
  sellerDiscount { get; set; }


         [XmlElement("orderState")]
public  		string
  orderState { get; set; }


         [XmlElement("orderStateRemark")]
public  		string
  orderStateRemark { get; set; }


         [XmlElement("deliveryType")]
public  		string
  deliveryType { get; set; }


         [XmlElement("invoiceInfo")]
public  		string
  invoiceInfo { get; set; }


         [XmlElement("invoiceCode")]
public  		string
  invoiceCode { get; set; }


         [XmlElement("orderRemark")]
public  		string
  orderRemark { get; set; }


         [XmlElement("orderStartTime")]
public  		string
  orderStartTime { get; set; }


         [XmlElement("orderEndTime")]
public  		string
  orderEndTime { get; set; }


         [XmlElement("consigneeInfo")]
public  		string
  consigneeInfo { get; set; }


         [XmlElement("itemInfoList")]
public  		List<string>
  itemInfoList { get; set; }


         [XmlElement("couponDetailList")]
public  		List<string>
  couponDetailList { get; set; }


         [XmlElement("venderRemark")]
public  		string
  venderRemark { get; set; }


         [XmlElement("balanceUsed")]
public  		string
  balanceUsed { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("returnOrder")]
public  		string
  returnOrder { get; set; }


         [XmlElement("paymentConfirmTime")]
public  		string
  paymentConfirmTime { get; set; }


         [XmlElement("waybill")]
public  		string
  waybill { get; set; }


         [XmlElement("logisticsId")]
public  		string
  logisticsId { get; set; }


         [XmlElement("vatInfo")]
public  		string
  vatInfo { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


         [XmlElement("directParentOrderId")]
public  		string
  directParentOrderId { get; set; }


         [XmlElement("parentOrderId")]
public  		string
  parentOrderId { get; set; }


         [XmlElement("customs")]
public  		string
  customs { get; set; }


         [XmlElement("orderSource")]
public  		string
  orderSource { get; set; }


         [XmlElement("customsModel")]
public  		string
  customsModel { get; set; }


         [XmlElement("storeOrder")]
public  		string
  storeOrder { get; set; }


         [XmlElement("idSopShipmenttype")]
public  		int
  idSopShipmenttype { get; set; }


         [XmlElement("scDT")]
public  		string
  scDT { get; set; }


         [XmlElement("serviceFee")]
public  		string
  serviceFee { get; set; }


         [XmlElement("pauseBizInfo")]
public  		string
  pauseBizInfo { get; set; }


         [XmlElement("taxFee")]
public  		string
  taxFee { get; set; }


         [XmlElement("tuiHuoWuYou")]
public  		string
  tuiHuoWuYou { get; set; }


         [XmlElement("orderSign")]
public  		string
  orderSign { get; set; }


}
}
