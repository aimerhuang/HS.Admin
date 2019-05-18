using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QuerySingleOrderForJosResultDto : JdObject{


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("resultSingleDto")]
public  		string
  resultSingleDto { get; set; }


}
}
