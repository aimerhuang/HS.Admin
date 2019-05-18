using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UserAuthDto : JdObject{


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("userId")]
public  		string
  userId { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


         [XmlElement("origin")]
public  		string
  origin { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("isManager")]
public  		int
  isManager { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


}
}
