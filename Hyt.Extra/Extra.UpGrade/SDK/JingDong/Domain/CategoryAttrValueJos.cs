using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CategoryAttrValueJos : JdObject{


         [XmlElement("attributeId")]
public  		long
  attributeId { get; set; }


         [XmlElement("categoryId")]
public  		long
  categoryId { get; set; }


         [XmlElement("features")]
public  		string
  features { get; set; }


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("indexId")]
public  		int
  indexId { get; set; }


         [XmlElement("value")]
public  		string
  value { get; set; }


}
}
