using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DspWeekForeCast : JdObject{


         [XmlElement("day")]
public  		int
  day { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


}
}
