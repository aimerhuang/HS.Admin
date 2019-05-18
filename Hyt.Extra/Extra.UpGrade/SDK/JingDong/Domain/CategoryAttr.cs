using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CategoryAttr : JdObject{


         [XmlElement("categoryAttrId")]
public  		long
  categoryAttrId { get; set; }


         [XmlElement("categoryId")]
public  		long
  categoryId { get; set; }


         [XmlElement("attName")]
public  		string
  attName { get; set; }


         [XmlElement("attrIndexId")]
public  		int
  attrIndexId { get; set; }


         [XmlElement("inputType")]
public  		int
  inputType { get; set; }


         [XmlElement("attributeType")]
public  		int
  attributeType { get; set; }


         [XmlElement("attrFeatures")]
public  		string
  attrFeatures { get; set; }


         [XmlElement("categoryAttrGroup")]
public  		string
  categoryAttrGroup { get; set; }


         [XmlElement("attrValueList")]
public  		List<string>
  attrValueList { get; set; }


}
}
