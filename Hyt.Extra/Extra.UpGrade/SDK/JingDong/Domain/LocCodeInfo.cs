using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class LocCodeInfo : JdObject{


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("code_status")]
public  		int
  codeStatus { get; set; }


         [XmlElement("order_create_time")]
public  		string
  orderCreateTime { get; set; }


         [XmlElement("status_modified_time")]
public  		string
  statusModifiedTime { get; set; }


         [XmlElement("effective_date_start")]
public  		string
  effectiveDateStart { get; set; }


         [XmlElement("effective_date_end")]
public  		string
  effectiveDateEnd { get; set; }


         [XmlElement("send_count")]
public  		int
  sendCount { get; set; }


         [XmlElement("consume_shop_id")]
public  		string
  consumeShopId { get; set; }


         [XmlElement("consume_shop_name")]
public  		string
  consumeShopName { get; set; }


         [XmlElement("order_shop_id")]
public  		string
  orderShopId { get; set; }


         [XmlElement("order_shop_name")]
public  		string
  orderShopName { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("phone_num")]
public  		string
  phoneNum { get; set; }


         [XmlElement("code_consumed_time")]
public  		string
  codeConsumedTime { get; set; }


         [XmlElement("card_number")]
public  		string
  cardNumber { get; set; }


         [XmlElement("pwd_number")]
public  		string
  pwdNumber { get; set; }


}
}
