using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosApplyProductDto : JdObject{


         [XmlElement("result")]
public  		List<string>
  result { get; set; }


         [XmlElement("count")]
public  		string
  count { get; set; }


         [XmlElement("is_success")]
public  		string
  isSuccess { get; set; }


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("return_message")]
public  		string
  returnMessage { get; set; }


}
}
