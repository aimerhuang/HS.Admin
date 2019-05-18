using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosModelDto : JdObject{


         [XmlElement("custom")]
public  		bool
  custom { get; set; }


         [XmlElement("models")]
public  		List<string>
  models { get; set; }


}
}
