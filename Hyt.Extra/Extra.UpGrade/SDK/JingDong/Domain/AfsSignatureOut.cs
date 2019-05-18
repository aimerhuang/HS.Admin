using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsSignatureOut : JdObject{


         [XmlElement("opName")]
public  		string
  opName { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("opTime")]
public  		DateTime
  opTime { get; set; }


}
}
