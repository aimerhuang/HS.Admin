using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AddResult : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("r_code")]
public  		string
  rCode { get; set; }


         [XmlElement("r_msg")]
public  		string
  rMsg { get; set; }


}
}
