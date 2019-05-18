using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsMessageOut : JdObject{


         [XmlElement("afsMessageId")]
public  		int
  afsMessageId { get; set; }


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("context")]
public  		string
  context { get; set; }


}
}
