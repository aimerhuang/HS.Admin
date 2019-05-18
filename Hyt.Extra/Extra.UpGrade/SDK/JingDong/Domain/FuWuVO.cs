using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FuWuVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("service_code")]
public  		string
  serviceCode { get; set; }


         [XmlElement("service_name")]
public  		string
  serviceName { get; set; }


         [XmlElement("service_status")]
public  		int
  serviceStatus { get; set; }


         [XmlElement("service_logo")]
public  		string
  serviceLogo { get; set; }


         [XmlElement("fws_pin")]
public  		string
  fwsPin { get; set; }


         [XmlElement("fws_id")]
public  		int
  fwsId { get; set; }


         [XmlElement("cid")]
public  		int
  cid { get; set; }


         [XmlElement("service_type")]
public  		int
  serviceType { get; set; }


         [XmlElement("chare_mode")]
public  		int
  chareMode { get; set; }


         [XmlElement("app_key")]
public  		string
  appKey { get; set; }


         [XmlElement("has_success_case")]
public  		int
  hasSuccessCase { get; set; }


         [XmlElement("customer_number")]
public  		int
  customerNumber { get; set; }


         [XmlElement("introduce")]
public  		string
  introduce { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("service_ext")]
public  		string
  serviceExt { get; set; }


}
}
