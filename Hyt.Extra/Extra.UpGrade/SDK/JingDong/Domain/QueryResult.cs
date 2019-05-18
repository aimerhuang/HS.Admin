using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryResult : JdObject{


         [XmlElement("totalCount")]
public  		long
  totalCount { get; set; }


         [XmlElement("result")]
public  		List<string>
  result { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


}
}
