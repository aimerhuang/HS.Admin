using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ExternalVerificationQueryResponse : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


         [XmlElement("data")]
public  		List<string>
  data { get; set; }


}
}
