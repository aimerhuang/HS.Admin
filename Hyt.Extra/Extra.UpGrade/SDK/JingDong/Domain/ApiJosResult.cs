using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ApiJosResult : JdObject{


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


         [XmlElement("result_describe")]
public  		string
  resultDescribe { get; set; }


}
}
