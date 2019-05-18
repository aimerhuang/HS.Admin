using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosQualificationApplyDto : JdObject{


         [XmlElement("single_obj")]
public  		string
  singleObj { get; set; }


         [XmlElement("count")]
public  		string
  count { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("return_message")]
public  		string
  returnMessage { get; set; }


}
}
