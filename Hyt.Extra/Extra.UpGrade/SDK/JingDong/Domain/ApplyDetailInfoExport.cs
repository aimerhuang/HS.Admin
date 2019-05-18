using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ApplyDetailInfoExport : JdObject{


         [XmlElement("afsApplyDetailId")]
public  		int
  afsApplyDetailId { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("afsDetailType")]
public  		int
  afsDetailType { get; set; }


         [XmlElement("wareDescribe")]
public  		string
  wareDescribe { get; set; }


}
}
