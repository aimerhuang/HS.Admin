using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PublicResultObjectAfsService : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("afsServiceOut")]
public  		string
  afsServiceOut { get; set; }


}
}
