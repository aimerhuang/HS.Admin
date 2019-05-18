using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CategoryVo : JdObject{


         [XmlElement("catId")]
public  		int
  catId { get; set; }


         [XmlElement("parentId")]
public  		int
  parentId { get; set; }


         [XmlElement("catName")]
public  		string
  catName { get; set; }


         [XmlElement("catNameEn")]
public  		string
  catNameEn { get; set; }


         [XmlElement("catLevel")]
public  		int
  catLevel { get; set; }


         [XmlElement("sortOrder")]
public  		int
  sortOrder { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


}
}
