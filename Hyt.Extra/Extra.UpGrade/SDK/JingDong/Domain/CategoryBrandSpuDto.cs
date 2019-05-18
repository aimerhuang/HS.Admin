using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CategoryBrandSpuDto : JdObject{


         [XmlElement("model")]
public  		string
  model { get; set; }


}
}
