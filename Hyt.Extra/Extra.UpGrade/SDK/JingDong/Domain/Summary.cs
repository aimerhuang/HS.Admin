using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Summary : JdObject{


         [XmlElement("ResultCount")]
public  		string
  ResultCount { get; set; }


}
}
