using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceVO : JdObject{


         [XmlElement("serviceCode")]
public  		string
  serviceCode { get; set; }


         [XmlElement("serviceName")]
public  		string
  serviceName { get; set; }


         [XmlElement("service_status")]
public  		string
  serviceStatus { get; set; }


         [XmlElement("serviceDetailUrl")]
public  		string
  serviceDetailUrl { get; set; }


         [XmlElement("serviceLogo")]
public  		string
  serviceLogo { get; set; }


         [XmlElement("fwsPin")]
public  		string
  fwsPin { get; set; }


         [XmlElement("fwsId")]
public  		int
  fwsId { get; set; }


         [XmlElement("cid")]
public  		int
  cid { get; set; }


         [XmlElement("service_type")]
public  		int
  serviceType { get; set; }


         [XmlElement("appKey")]
public  		string
  appKey { get; set; }


         [XmlElement("chargeVO")]
public  		string
  chargeVO { get; set; }


}
}
