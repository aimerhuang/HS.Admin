using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ADQueryVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("url")]
public  		string
  url { get; set; }


         [XmlElement("adGroupId")]
public  		long
  adGroupId { get; set; }


         [XmlElement("width")]
public  		int
  width { get; set; }


         [XmlElement("height")]
public  		int
  height { get; set; }


         [XmlElement("imgUrl")]
public  		string
  imgUrl { get; set; }


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("auditInfo")]
public  		string
  auditInfo { get; set; }


         [XmlElement("putType")]
public  		int
  putType { get; set; }


         [XmlElement("auditTime")]
public  		DateTime
  auditTime { get; set; }


         [XmlElement("auditAdvice")]
public  		string
  auditAdvice { get; set; }


         [XmlElement("outerAuditStatus")]
public  		string
  outerAuditStatus { get; set; }


}
}
