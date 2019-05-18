using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PageDomain : JdObject{


         [XmlElement("totalRecord")]
public  		int
  totalRecord { get; set; }


         [XmlElement("objectList")]
public  		List<string>
  objectList { get; set; }


}
}
