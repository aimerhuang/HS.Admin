using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DangerGoodsDto : JdObject{


         [XmlElement("key")]
public  		string
  key { get; set; }


         [XmlElement("val")]
public  		int
  val { get; set; }


}
}
