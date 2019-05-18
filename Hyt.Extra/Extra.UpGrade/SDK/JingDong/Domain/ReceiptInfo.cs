using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReceiptInfo : JdObject{


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


}
}
