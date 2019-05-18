using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderDataNotPayInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("venderId")]
public  		long
  venderId { get; set; }


         [XmlElement("payment")]
public  		int
  payment { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


         [XmlElement("parentId")]
public  		long
  parentId { get; set; }


         [XmlElement("orderCreated")]
public  		string
  orderCreated { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("userName")]
public  		string
  userName { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("sendPay")]
public  		string
  sendPay { get; set; }


         [XmlElement("itemList")]
public  		List<string>
  itemList { get; set; }


}
}
