using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SerialTracDetailDto : JdObject{


         [XmlElement("serialTracDetailList")]
public  		List<string>
  serialTracDetailList { get; set; }


}
}
