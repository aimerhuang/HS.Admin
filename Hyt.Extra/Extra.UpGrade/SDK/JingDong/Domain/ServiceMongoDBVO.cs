using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceMongoDBVO : JdObject{


         [XmlElement("service_id")]
public  		long
  serviceId { get; set; }


         [XmlElement("service_code")]
public  		string
  serviceCode { get; set; }


         [XmlElement("success_case")]
public  		string
  successCase { get; set; }


         [XmlElement("service_tutorial")]
public  		string
  serviceTutorial { get; set; }


         [XmlElement("service_desc")]
public  		string
  serviceDesc { get; set; }


         [XmlElement("service_detail_url")]
public  		string
  serviceDetailUrl { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


}
}
