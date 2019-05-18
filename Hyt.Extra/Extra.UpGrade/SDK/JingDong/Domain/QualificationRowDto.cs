using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QualificationRowDto : JdObject{


         [XmlElement("apply_id")]
public  		string
  applyId { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("modified_time")]
public  		string
  modifiedTime { get; set; }


}
}
