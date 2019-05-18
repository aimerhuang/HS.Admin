using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PurchaseWarePropertyDto : JdObject{


         [XmlElement("ware_id")]
public  		long
  wareId { get; set; }


         [XmlElement("chest")]
public  		double
  chest { get; set; }


         [XmlElement("waistline")]
public  		double
  waistline { get; set; }


         [XmlElement("hip")]
public  		double
  hip { get; set; }


         [XmlElement("dress_length")]
public  		double
  dressLength { get; set; }


         [XmlElement("height")]
public  		double
  height { get; set; }


         [XmlElement("color")]
public  		string
  color { get; set; }


}
}
