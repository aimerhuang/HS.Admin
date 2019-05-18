using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ResultStatus : JdObject{


         [XmlElement("code")]
public  		int
  code { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("plan_id")]
public  		long
  planId { get; set; }


}
}
