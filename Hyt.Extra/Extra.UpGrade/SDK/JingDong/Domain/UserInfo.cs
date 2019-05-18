using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UserInfo : JdObject{


         [XmlElement("fullname")]
public  		string
  fullname { get; set; }


         [XmlElement("telephone")]
public  		string
  telephone { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


         [XmlElement("fullAddress")]
public  		string
  fullAddress { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("county")]
public  		string
  county { get; set; }


}
}
