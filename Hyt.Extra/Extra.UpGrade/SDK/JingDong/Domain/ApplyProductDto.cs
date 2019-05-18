using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ApplyProductDto : JdObject{


         [XmlElement("apply_id")]
public  		string
  applyId { get; set; }


         [XmlElement("wareId")]
public  		string
  wareId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("submit_time")]
public  		string
  submitTime { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


}
}
