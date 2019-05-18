using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderForJosResult : JdObject{


         [XmlElement("customOrderId")]
public  		long
  customOrderId { get; set; }


         [XmlElement("pay")]
public  		string
  pay { get; set; }


         [XmlElement("operatorState")]
public  		int
  operatorState { get; set; }


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


         [XmlElement("orderTime")]
public  		DateTime
  orderTime { get; set; }


         [XmlElement("orderRemark")]
public  		string
  orderRemark { get; set; }


         [XmlElement("orderCreateDate")]
public  		DateTime
  orderCreateDate { get; set; }


         [XmlElement("isNotice")]
public  		int
  isNotice { get; set; }


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


         [XmlElement("memoByVendor")]
public  		string
  memoByVendor { get; set; }


         [XmlElement("refundSourceFlag")]
public  		string
  refundSourceFlag { get; set; }


         [XmlElement("provinceName")]
public  		string
  provinceName { get; set; }


         [XmlElement("cityName")]
public  		string
  cityName { get; set; }


         [XmlElement("countyName")]
public  		string
  countyName { get; set; }


         [XmlElement("townName")]
public  		string
  townName { get; set; }


         [XmlElement("parentOrderId")]
public  		long
  parentOrderId { get; set; }


         [XmlElement("orderDetailList")]
public  		List<string>
  orderDetailList { get; set; }


}
}
