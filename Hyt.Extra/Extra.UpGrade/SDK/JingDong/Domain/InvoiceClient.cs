using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class InvoiceClient : JdObject{


         [XmlElement("invoiceType")]
public  		int
  invoiceType { get; set; }


         [XmlElement("billingType")]
public  		int
  billingType { get; set; }


}
}
