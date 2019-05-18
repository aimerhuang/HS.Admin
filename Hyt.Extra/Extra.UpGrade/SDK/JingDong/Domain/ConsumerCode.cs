using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ConsumerCode : JdObject{


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("code_num")]
public  		string
  codeNum { get; set; }


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("effective_date")]
public  		DateTime
  effectiveDate { get; set; }


         [XmlElement("send_count")]
public  		int
  sendCount { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("consumer_status")]
public  		int
  consumerStatus { get; set; }


         [XmlElement("consumer_time")]
public  		DateTime
  consumerTime { get; set; }


         [XmlElement("card_number")]
public  		string
  cardNumber { get; set; }


         [XmlElement("pwd_number")]
public  		string
  pwdNumber { get; set; }


}
}
