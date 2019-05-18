using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceStatusInfo : JdObject{


         [XmlElement("serviceId")]
public  		long
  serviceId { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


}
}
