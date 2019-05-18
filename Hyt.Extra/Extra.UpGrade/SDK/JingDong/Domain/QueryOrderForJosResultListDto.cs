using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderForJosResultListDto : JdObject{


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("resultDtoList")]
public  		List<string>
  resultDtoList { get; set; }


}
}
