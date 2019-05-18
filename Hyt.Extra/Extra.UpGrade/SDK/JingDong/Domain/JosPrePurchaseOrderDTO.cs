using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosPrePurchaseOrderDTO : JdObject{


         [XmlElement("forecastPurchaseOrderCode")]
public  		string
  forecastPurchaseOrderCode { get; set; }


         [XmlElement("prePurchaseOrderCode")]
public  		string
  prePurchaseOrderCode { get; set; }


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("orgCode")]
public  		string
  orgCode { get; set; }


         [XmlElement("orgName")]
public  		string
  orgName { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
