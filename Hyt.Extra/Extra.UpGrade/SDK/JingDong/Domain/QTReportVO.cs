using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QTReportVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("qt_code")]
public  		string
  qtCode { get; set; }


         [XmlElement("qt_name")]
public  		string
  qtName { get; set; }


         [XmlElement("qt_type")]
public  		int
  qtType { get; set; }


         [XmlElement("qt_standard")]
public  		string
  qtStandard { get; set; }


         [XmlElement("is_passed")]
public  		int
  isPassed { get; set; }


         [XmlElement("sp_name")]
public  		string
  spName { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("submit_time")]
public  		string
  submitTime { get; set; }


         [XmlElement("report_time")]
public  		string
  reportTime { get; set; }


         [XmlElement("expiry_time")]
public  		string
  expiryTime { get; set; }


         [XmlElement("item_url")]
public  		string
  itemUrl { get; set; }


         [XmlElement("item_desc")]
public  		string
  itemDesc { get; set; }


         [XmlElement("report_url")]
public  		string
  reportUrl { get; set; }


         [XmlElement("ext_attr")]
public  		string
  extAttr { get; set; }


         [XmlElement("num_iid")]
public  		int
  numIid { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


}
}
