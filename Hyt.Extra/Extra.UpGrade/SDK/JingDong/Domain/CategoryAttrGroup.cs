using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CategoryAttrGroup : JdObject{


         [XmlElement("groupId")]
public  		long
  groupId { get; set; }


         [XmlElement("groupName")]
public  		string
  groupName { get; set; }


         [XmlElement("attrGroupIndexId")]
public  		int
  attrGroupIndexId { get; set; }


         [XmlElement("attrGroupfeatures")]
public  		string
  attrGroupfeatures { get; set; }


}
}
