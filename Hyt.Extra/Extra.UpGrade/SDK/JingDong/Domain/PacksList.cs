using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PacksList : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("imageDomain")]
public  		string
  imageDomain { get; set; }


         [XmlElement("data")]
public  		List<string>
  data { get; set; }


}
}
