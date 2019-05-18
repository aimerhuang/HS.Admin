using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderInfo : JdObject{


         [XmlElement("orderNo")]
public  		long
  orderNo { get; set; }


         [XmlElement("orderStatus")]
public  		int
  orderStatus { get; set; }


         [XmlElement("payTime")]
public  		DateTime
  payTime { get; set; }


         [XmlElement("totalAmount")]
public  		string
  totalAmount { get; set; }


         [XmlElement("freight")]
public  		string
  freight { get; set; }


         [XmlElement("carrierCode")]
public  		string
  carrierCode { get; set; }


         [XmlElement("carrierName")]
public  		string
  carrierName { get; set; }


         [XmlElement("deliveryTime")]
public  		DateTime
  deliveryTime { get; set; }


         [XmlElement("sellerId")]
public  		long
  sellerId { get; set; }


         [XmlElement("storeId")]
public  		long
  storeId { get; set; }


         [XmlElement("cashOnDelivery")]
public  		bool
  cashOnDelivery { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("country")]
public  		string
  country { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("address1")]
public  		string
  address1 { get; set; }


         [XmlElement("address2")]
public  		string
  address2 { get; set; }


         [XmlElement("fullAddress")]
public  		string
  fullAddress { get; set; }


         [XmlElement("postCode")]
public  		string
  postCode { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("passportNo")]
public  		string
  passportNo { get; set; }


         [XmlElement("skuList")]
public  		string
  skuList { get; set; }


}
}
