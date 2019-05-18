using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosSalesReturnResultDto : JdObject{


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("salesReturnList")]
public  		List<string>
  salesReturnList { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


}
}
