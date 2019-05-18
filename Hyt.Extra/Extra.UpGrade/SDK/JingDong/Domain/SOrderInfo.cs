using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SOrderInfo : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("venderId")]
public  		long
  venderId { get; set; }


         [XmlElement("orderType")]
public  		string
  orderType { get; set; }


         [XmlElement("orderCreateTime")]
public  		string
  orderCreateTime { get; set; }


         [XmlElement("orderStatus")]
public  		string
  orderStatus { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


         [XmlElement("orderPayment")]
public  		string
  orderPayment { get; set; }


         [XmlElement("freightPrice")]
public  		string
  freightPrice { get; set; }


         [XmlElement("orderPrice")]
public  		string
  orderPrice { get; set; }


         [XmlElement("payPrice")]
public  		string
  payPrice { get; set; }


         [XmlElement("userPin")]
public  		string
  userPin { get; set; }


         [XmlElement("couponPrice")]
public  		string
  couponPrice { get; set; }


         [XmlElement("clubId")]
public  		long
  clubId { get; set; }


         [XmlElement("memStatus")]
public  		string
  memStatus { get; set; }


         [XmlElement("memModified")]
public  		string
  memModified { get; set; }


         [XmlElement("member")]
public  		string
  member { get; set; }


         [XmlElement("skuList")]
public  		List<string>
  skuList { get; set; }


}
}
