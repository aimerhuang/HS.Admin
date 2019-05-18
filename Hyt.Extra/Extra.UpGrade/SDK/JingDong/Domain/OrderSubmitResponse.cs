using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderSubmitResponse : JdObject{


         [XmlElement("orderCode")]
public  		string
  orderCode { get; set; }


         [XmlElement("fxOrderCode")]
public  		string
  fxOrderCode { get; set; }


         [XmlElement("jdOrderCode")]
public  		string
  jdOrderCode { get; set; }


         [XmlElement("skus")]
public  		List<string>
  skus { get; set; }


         [XmlElement("errorSkus")]
public  		List<string>
  errorSkus { get; set; }


         [XmlElement("freightFee")]
public  		string
  freightFee { get; set; }


         [XmlElement("rebate")]
public  		string
  rebate { get; set; }


         [XmlElement("shouldPay")]
public  		string
  shouldPay { get; set; }


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


}
}
