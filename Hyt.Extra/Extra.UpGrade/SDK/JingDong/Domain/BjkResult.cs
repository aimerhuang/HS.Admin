using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BjkResult : JdObject{


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("totalRecord")]
public  		string
  totalRecord { get; set; }


         [XmlElement("serviceInfoList")]
public  		List<string>
  serviceInfoList { get; set; }


}
}
