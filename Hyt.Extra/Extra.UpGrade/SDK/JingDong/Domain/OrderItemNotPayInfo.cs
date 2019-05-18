using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderItemNotPayInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("venderId")]
public  		long
  venderId { get; set; }


         [XmlElement("num")]
public  		int
  num { get; set; }


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
