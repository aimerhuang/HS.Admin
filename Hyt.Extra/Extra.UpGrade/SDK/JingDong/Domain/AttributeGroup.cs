using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AttributeGroup : JdObject{


         [XmlElement("groupId")]
public  		int
  groupId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("cid")]
public  		int
  cid { get; set; }


}
}
