using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Prop : JdObject{


         [XmlElement("attrId")]
public  		string
  attrId { get; set; }


         [XmlElement("attrValueAlias")]
public  		string
  attrValueAlias { get; set; }


         [XmlElement("attrValues")]
public  		string
  attrValues { get; set; }


}
}
