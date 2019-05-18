using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SerialTracDto : JdObject{


         [XmlElement("serialTracList")]
public  		List<string>
  serialTracList { get; set; }


}
}
