using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderinfoProductResponse : JdObject{


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


         [XmlElement("outer_sku_id")]
public  		string
  outerSkuId { get; set; }


         [XmlElement("sku_name")]
public  		string
  skuName { get; set; }


         [XmlElement("jd_price")]
public  		double
  jdPrice { get; set; }


         [XmlElement("gift_point")]
public  		int
  giftPoint { get; set; }


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("item_total")]
public  		int
  itemTotal { get; set; }


         [XmlElement("stock_owner")]
public  		string
  stockOwner { get; set; }


}
}
