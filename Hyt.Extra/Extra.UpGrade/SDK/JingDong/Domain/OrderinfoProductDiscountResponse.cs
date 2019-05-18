using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderinfoProductDiscountResponse : JdObject{


         [XmlElement("coupon_price")]
public  		double
  couponPrice { get; set; }


         [XmlElement("coupon_type")]
public  		string
  couponType { get; set; }


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


}
}
