using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OpReason : JdObject{


         [XmlElement("note")]
public  		string
  note { get; set; }


}
}
