using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsRefundOut : JdObject{


         [XmlElement("afsRefundId")]
public  		int
  afsRefundId { get; set; }


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("suggestAmount")]
public  		string
  suggestAmount { get; set; }


         [XmlElement("mark")]
public  		string
  mark { get; set; }


         [XmlElement("reason")]
public  		string
  reason { get; set; }


         [XmlElement("payInfo")]
public  		string
  payInfo { get; set; }


}
}
