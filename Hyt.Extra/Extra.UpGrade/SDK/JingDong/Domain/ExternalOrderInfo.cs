using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ExternalOrderInfo : JdObject{


         [XmlElement("waybillCode")]
public  		string
  waybillCode { get; set; }


         [XmlElement("courierId")]
public  		int
  courierId { get; set; }


         [XmlElement("courierName")]
public  		string
  courierName { get; set; }


         [XmlElement("courierTel")]
public  		string
  courierTel { get; set; }


         [XmlElement("photo")]
public  		string
  photo { get; set; }


         [XmlElement("siteCode")]
public  		int
  siteCode { get; set; }


         [XmlElement("siteName")]
public  		string
  siteName { get; set; }


         [XmlElement("siteGPS")]
public  		string
  siteGPS { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


}
}
