using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ArticleVO : JdObject{


         [XmlElement("fws_pin")]
public  		string
  fwsPin { get; set; }


         [XmlElement("service_code")]
public  		string
  serviceCode { get; set; }


         [XmlElement("item_code")]
public  		string
  itemCode { get; set; }


         [XmlElement("start_date")]
public  		DateTime
  startDate { get; set; }


         [XmlElement("end_date")]
public  		DateTime
  endDate { get; set; }


         [XmlElement("order_type")]
public  		int
  orderType { get; set; }


         [XmlElement("order_cycle")]
public  		int
  orderCycle { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
