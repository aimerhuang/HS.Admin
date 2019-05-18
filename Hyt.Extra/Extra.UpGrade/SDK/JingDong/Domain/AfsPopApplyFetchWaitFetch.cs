using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsPopApplyFetchWaitFetch : JdObject{


         [XmlElement("afsApplyId")]
public  		int
  afsApplyId { get; set; }


         [XmlElement("afsCategoryId")]
public  		int
  afsCategoryId { get; set; }


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("afsServiceList")]
public  		List<string>
  afsServiceList { get; set; }


         [XmlElement("afsApply")]
public  		string
  afsApply { get; set; }


}
}
