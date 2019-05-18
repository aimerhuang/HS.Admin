using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class TraceApiDto : JdObject{


         [XmlElement("ope_title")]
public  		string
  opeTitle { get; set; }


         [XmlElement("ope_remark")]
public  		string
  opeRemark { get; set; }


         [XmlElement("ope_name")]
public  		string
  opeName { get; set; }


         [XmlElement("ope_time")]
public  		string
  opeTime { get; set; }


}
}
