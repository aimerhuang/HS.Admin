using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class TraceDTO : JdObject{


         [XmlElement("opeTitle")]
public  		string
  opeTitle { get; set; }


         [XmlElement("opeRemark")]
public  		string
  opeRemark { get; set; }


         [XmlElement("opeName")]
public  		string
  opeName { get; set; }


         [XmlElement("opeTime")]
public  		string
  opeTime { get; set; }


         [XmlElement("waybillCode")]
public  		string
  waybillCode { get; set; }


}
}
