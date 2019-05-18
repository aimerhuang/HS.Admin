using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FullCategoryDto : JdObject{


         [XmlElement("category1")]
public  		string
  category1 { get; set; }


         [XmlElement("category2")]
public  		string
  category2 { get; set; }


         [XmlElement("category3")]
public  		string
  category3 { get; set; }


}
}
