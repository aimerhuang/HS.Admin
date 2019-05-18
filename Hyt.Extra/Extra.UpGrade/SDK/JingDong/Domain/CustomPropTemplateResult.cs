using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CustomPropTemplateResult : JdObject{


         [XmlElement("totalCount")]
public  		int
  totalCount { get; set; }


         [XmlElement("currentPage")]
public  		int
  currentPage { get; set; }


         [XmlElement("tplList")]
public  		List<string>
  tplList { get; set; }


         [XmlElement("messegeCode")]
public  		string
  messegeCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


}
}
