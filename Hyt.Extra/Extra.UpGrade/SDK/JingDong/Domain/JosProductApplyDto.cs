using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosProductApplyDto : JdObject{


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("submit_time")]
public  		DateTime
  submitTime { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


}
}
