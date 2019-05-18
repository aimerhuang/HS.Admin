using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderForJosResultList : JdObject{


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("errorMessage")]
public  		string
  errorMessage { get; set; }


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("resultDtoList")]
public  		List<string>
  resultDtoList { get; set; }


}
}
