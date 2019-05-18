using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ExtPropertyResponse : JdObject{


         [XmlElement("giftList")]
public  		List<string>
  giftList { get; set; }


         [XmlElement("details")]
public  		string
  details { get; set; }


}
}
