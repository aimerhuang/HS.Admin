using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PropertyVO : JdObject{


         [XmlElement("catId")]
public  		int
  catId { get; set; }


         [XmlElement("propertyId")]
public  		int
  propertyId { get; set; }


         [XmlElement("propertyName")]
public  		string
  propertyName { get; set; }


         [XmlElement("propertyNameEn")]
public  		string
  propertyNameEn { get; set; }


         [XmlElement("propertyType")]
public  		int
  propertyType { get; set; }


         [XmlElement("required")]
public  		int
  required { get; set; }


         [XmlElement("inputType")]
public  		int
  inputType { get; set; }


         [XmlElement("nav")]
public  		int
  nav { get; set; }


}
}
