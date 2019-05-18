using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QualificationInfoDto : JdObject{


         [XmlElement("wareId")]
public  		string
  wareId { get; set; }


         [XmlElement("qualifications")]
public  		List<string>
  qualifications { get; set; }


}
}
