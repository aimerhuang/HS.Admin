using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("service_id")]
public  		long
  serviceId { get; set; }


         [XmlElement("service_code")]
public  		string
  serviceCode { get; set; }


         [XmlElement("fws_pin")]
public  		string
  fwsPin { get; set; }


         [XmlElement("fws_id")]
public  		int
  fwsId { get; set; }


         [XmlElement("item_code")]
public  		string
  itemCode { get; set; }


         [XmlElement("item_name")]
public  		string
  itemName { get; set; }


         [XmlElement("item_type")]
public  		int
  itemType { get; set; }


         [XmlElement("item_version")]
public  		int
  itemVersion { get; set; }


         [XmlElement("item_status")]
public  		int
  itemStatus { get; set; }


         [XmlElement("item_desc")]
public  		string
  itemDesc { get; set; }


         [XmlElement("charge_type")]
public  		int
  chargeType { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
