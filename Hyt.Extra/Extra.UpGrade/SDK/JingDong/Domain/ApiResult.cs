using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ApiResult : JdObject{


         [XmlElement("isSuccess")]
public  		string
  isSuccess { get; set; }


         [XmlElement("englishErrCode")]
public  		string
  englishErrCode { get; set; }


         [XmlElement("chineseErrCode")]
public  		string
  chineseErrCode { get; set; }


         [XmlElement("numberCode")]
public  		string
  numberCode { get; set; }


}
}
