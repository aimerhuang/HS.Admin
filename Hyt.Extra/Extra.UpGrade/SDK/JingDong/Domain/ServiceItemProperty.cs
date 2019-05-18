using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceItemProperty : JdObject{


         [XmlElement("service_item_code")]
public  		string
  serviceItemCode { get; set; }


         [XmlElement("service_item_name")]
public  		string
  serviceItemName { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("service_code")]
public  		string
  serviceCode { get; set; }


         [XmlElement("service_name")]
public  		string
  serviceName { get; set; }


         [XmlElement("service_item_description")]
public  		string
  serviceItemDescription { get; set; }


         [XmlElement("base_price")]
public  		string
  basePrice { get; set; }


         [XmlElement("qt_ativity_type")]
public  		int
  qtAtivityType { get; set; }


}
}
