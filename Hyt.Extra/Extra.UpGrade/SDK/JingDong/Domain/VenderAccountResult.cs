using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderAccountResult : JdObject{


         [XmlElement("child_account_s")]
public  		List<string>
  childAccountS { get; set; }


         [XmlElement("is_success")]
public  		string
  isSuccess { get; set; }


         [XmlElement("error_code")]
public  		string
  errorCode { get; set; }


         [XmlElement("error_msg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("total_count")]
public  		string
  totalCount { get; set; }


}
}
