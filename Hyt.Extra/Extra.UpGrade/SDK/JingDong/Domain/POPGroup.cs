using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class POPGroup : JdObject{


         [XmlElement("shopName")]
public  		string
  shopName { get; set; }


         [XmlElement("shopUrl")]
public  		string
  shopUrl { get; set; }


         [XmlElement("waiterCount")]
public  		string
  waiterCount { get; set; }


         [XmlElement("waiterList")]
public  		List<string>
  waiterList { get; set; }


}
}
