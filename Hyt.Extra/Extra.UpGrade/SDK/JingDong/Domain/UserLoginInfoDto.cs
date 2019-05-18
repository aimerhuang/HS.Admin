using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UserLoginInfoDto : JdObject{


         [XmlElement("userId")]
public  		string
  userId { get; set; }


         [XmlElement("username")]
public  		string
  username { get; set; }


         [XmlElement("appLoginUrl")]
public  		string
  appLoginUrl { get; set; }


         [XmlElement("appLoginUrlParameter")]
public  		string
  appLoginUrlParameter { get; set; }


         [XmlElement("loginUrl")]
public  		string
  loginUrl { get; set; }


         [XmlElement("loginUrlParameter")]
public  		string
  loginUrlParameter { get; set; }


         [XmlElement("origin")]
public  		string
  origin { get; set; }


         [XmlElement("alias")]
public  		string
  alias { get; set; }


}
}
