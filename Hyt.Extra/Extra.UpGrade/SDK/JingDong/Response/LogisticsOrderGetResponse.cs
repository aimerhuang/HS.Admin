using System;
using System.Xml.Serialization;
using System.Collections.Generic;

						using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsOrderGetResponse : JdResponse{


         [XmlElement("receipt_no")]
public  		string
  receiptNo { get; set; }


         [XmlElement("order_status_details")]
public  		string
  orderStatusDetails { get; set; }


         [XmlElement("order_package_details")]
public  		string
  orderPackageDetails { get; set; }


}
}
