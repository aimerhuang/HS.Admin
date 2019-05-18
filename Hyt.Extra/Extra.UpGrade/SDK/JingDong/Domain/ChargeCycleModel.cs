using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ChargeCycleModel : JdObject{


         [XmlElement("service_code")]
public  		string
  serviceCode { get; set; }


         [XmlElement("service_id")]
public  		long
  serviceId { get; set; }


         [XmlElement("item_id")]
public  		long
  itemId { get; set; }


         [XmlElement("item_code")]
public  		string
  itemCode { get; set; }


         [XmlElement("charge_days")]
public  		int
  chargeDays { get; set; }


         [XmlElement("price")]
public  		long
  price { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


         [XmlElement("page_display")]
public  		int
  pageDisplay { get; set; }


}
}
