using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ResponseDTO : JdObject{


         [XmlElement("statusCode")]
public  		int
  statusCode { get; set; }


         [XmlElement("statusMessage")]
public  		string
  statusMessage { get; set; }


         [XmlElement("data")]
public  		List<string>
  data { get; set; }


}
}
