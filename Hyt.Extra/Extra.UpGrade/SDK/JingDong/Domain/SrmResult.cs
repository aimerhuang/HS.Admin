using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SrmResult : JdObject{


         [XmlElement("code")]
public  		int
  code { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("lasHairQueryQtyResults")]
public  		List<string>
  lasHairQueryQtyResults { get; set; }


}
}
