using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PublicResultObjectUnresolvedTask : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("unresolvedAfsService")]
public  		string
  unresolvedAfsService { get; set; }


}
}
