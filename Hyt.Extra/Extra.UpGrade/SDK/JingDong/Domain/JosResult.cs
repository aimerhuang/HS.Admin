using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("data")]
public  		List<string>
  data { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("pageOffset")]
public  		int
  pageOffset { get; set; }


}
}
