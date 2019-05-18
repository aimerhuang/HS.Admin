using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class InnerProtocolResult : JdObject{


         [XmlElement("errcode")]
public  		int
  errcode { get; set; }


         [XmlElement("openlink")]
public  		string
  openlink { get; set; }


}
}
