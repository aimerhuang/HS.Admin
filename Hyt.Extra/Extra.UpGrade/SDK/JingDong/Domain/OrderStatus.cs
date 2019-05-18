using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderStatus : JdObject{


         [XmlElement("soStatusCode")]
public  		string
  soStatusCode { get; set; }


         [XmlElement("soStatusName")]
public  		string
  soStatusName { get; set; }


         [XmlElement("operateTime")]
public  		string
  operateTime { get; set; }


         [XmlElement("operateUser")]
public  		string
  operateUser { get; set; }


}
}
