using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PromoPropVO : JdObject{


         [XmlElement("promo_id")]
public  		long
  promoId { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("num")]
public  		int
  num { get; set; }


         [XmlElement("used_way")]
public  		int
  usedWay { get; set; }


}
}
