using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ChargeCycleVO : JdObject{


         [XmlElement("chargeDays")]
public  		int
  chargeDays { get; set; }


         [XmlElement("pageDisplay")]
public  		int
  pageDisplay { get; set; }


         [XmlElement("price")]
public  		long
  price { get; set; }


}
}
