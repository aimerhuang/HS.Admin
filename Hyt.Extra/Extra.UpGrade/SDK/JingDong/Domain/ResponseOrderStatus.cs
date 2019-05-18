using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ResponseOrderStatus : JdObject{


         [XmlElement("receipt_no")]
public  		string
  receiptNo { get; set; }


         [XmlElement("order_status_details")]
public  		List<string>
  orderStatusDetails { get; set; }


         [XmlElement("order_package_details")]
public  		List<string>
  orderPackageDetails { get; set; }


}
}
