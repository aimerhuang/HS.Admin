using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AttributeValue : JdObject{


         [XmlElement("valueId")]
public  		int
  valueId { get; set; }


         [XmlElement("valueName")]
public  		string
  valueName { get; set; }


         [XmlElement("attId")]
public  		int
  attId { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


}
}
