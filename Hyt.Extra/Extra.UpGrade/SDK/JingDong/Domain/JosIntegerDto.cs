using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosIntegerDto : JdObject{


         [XmlElement("single_obj")]
public  		int
  singleObj { get; set; }


         [XmlElement("count")]
public  		int
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
