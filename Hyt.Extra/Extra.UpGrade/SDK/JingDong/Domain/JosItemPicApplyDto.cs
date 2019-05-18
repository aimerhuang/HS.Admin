using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosItemPicApplyDto : JdObject{


         [XmlElement("single_obj")]
public  		string
  singleObj { get; set; }


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
