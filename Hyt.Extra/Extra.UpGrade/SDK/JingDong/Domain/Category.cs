using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Category : JdObject{


         [XmlElement("categoryNo")]
public  		string
  categoryNo { get; set; }


         [XmlElement("categoryName")]
public  		string
  categoryName { get; set; }


}
}
