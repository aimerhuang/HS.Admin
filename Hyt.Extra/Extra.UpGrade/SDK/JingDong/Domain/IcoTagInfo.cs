using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IcoTagInfo : JdObject{


         [XmlElement("tagurl")]
public  		string
  tagurl { get; set; }


}
}
