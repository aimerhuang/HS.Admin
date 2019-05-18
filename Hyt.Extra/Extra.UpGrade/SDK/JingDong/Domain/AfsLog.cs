using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsLog : JdObject{


         [XmlElement("afsLogId")]
public  		int
  afsLogId { get; set; }


         [XmlElement("relationId")]
public  		int
  relationId { get; set; }


         [XmlElement("operateRemark")]
public  		string
  operateRemark { get; set; }


         [XmlElement("operatePin")]
public  		string
  operatePin { get; set; }


         [XmlElement("operateName")]
public  		string
  operateName { get; set; }


         [XmlElement("operateDate")]
public  		DateTime
  operateDate { get; set; }


}
}
