using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OuterOrderStatusDto : JdObject{


         [XmlElement("orderStatus")]
public  		int
  orderStatus { get; set; }


         [XmlElement("orderStatusName")]
public  		string
  orderStatusName { get; set; }


         [XmlElement("operateUser")]
public  		string
  operateUser { get; set; }


         [XmlElement("operateTime")]
public  		DateTime
  operateTime { get; set; }


}
}
