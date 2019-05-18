using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderModeVO : JdObject{


         [XmlElement("promo_id")]
public  		long
  promoId { get; set; }


         [XmlElement("favor_mode")]
public  		int
  favorMode { get; set; }


         [XmlElement("quota")]
public  		string
  quota { get; set; }


         [XmlElement("rate")]
public  		string
  rate { get; set; }


         [XmlElement("plus")]
public  		string
  plus { get; set; }


         [XmlElement("minus")]
public  		string
  minus { get; set; }


         [XmlElement("link")]
public  		string
  link { get; set; }


}
}
