using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImagePathDto : JdObject{


         [XmlElement("is_primary")]
public  		int
  isPrimary { get; set; }


         [XmlElement("order_sort")]
public  		int
  orderSort { get; set; }


         [XmlElement("path")]
public  		string
  path { get; set; }


}
}
