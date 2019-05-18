using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryAllOrdersForJosResult : JdObject{


         [XmlElement("customOrderId")]
public  		long
  customOrderId { get; set; }


         [XmlElement("pay")]
public  		string
  pay { get; set; }


         [XmlElement("operatorState")]
public  		int
  operatorState { get; set; }


         [XmlElement("orderState")]
public  		int
  orderState { get; set; }


         [XmlElement("consigneeName")]
public  		string
  consigneeName { get; set; }


         [XmlElement("postcode")]
public  		string
  postcode { get; set; }


         [XmlElement("expectedDeliveryTime")]
public  		DateTime
  expectedDeliveryTime { get; set; }


         [XmlElement("telephone")]
public  		string
  telephone { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("orderRemark")]
public  		string
  orderRemark { get; set; }


         [XmlElement("orderCreateDate")]
public  		DateTime
  orderCreateDate { get; set; }


         [XmlElement("isNotNotice")]
public  		int
  isNotNotice { get; set; }


         [XmlElement("sendPay")]
public  		string
  sendPay { get; set; }


         [XmlElement("paymentCategory")]
public  		string
  paymentCategory { get; set; }


         [XmlElement("paymentCategoryDispName")]
public  		string
  paymentCategoryDispName { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("refundSourceFlag")]
public  		int
  refundSourceFlag { get; set; }


         [XmlElement("provinceId")]
public  		int
  provinceId { get; set; }


         [XmlElement("provinceName")]
public  		string
  provinceName { get; set; }


         [XmlElement("cityId")]
public  		int
  cityId { get; set; }


         [XmlElement("cityName")]
public  		string
  cityName { get; set; }


         [XmlElement("countyId")]
public  		int
  countyId { get; set; }


         [XmlElement("countyName")]
public  		string
  countyName { get; set; }


         [XmlElement("townId")]
public  		int
  townId { get; set; }


         [XmlElement("townName")]
public  		string
  townName { get; set; }


         [XmlElement("memoByVendor")]
public  		string
  memoByVendor { get; set; }


         [XmlElement("parentOrderId")]
public  		long
  parentOrderId { get; set; }


         [XmlElement("sku")]
public  		string
  sku { get; set; }


         [XmlElement("commodityName")]
public  		string
  commodityName { get; set; }


         [XmlElement("commodityNum")]
public  		int
  commodityNum { get; set; }


         [XmlElement("jdPrice")]
public  		string
  jdPrice { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("cost")]
public  		string
  cost { get; set; }


}
}
