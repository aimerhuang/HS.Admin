using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PauseBizStatus : JdObject{


         [XmlElement("bizType")]
public  		int
  bizType { get; set; }


         [XmlElement("bizStatus")]
public  		int
  bizStatus { get; set; }


}
}
