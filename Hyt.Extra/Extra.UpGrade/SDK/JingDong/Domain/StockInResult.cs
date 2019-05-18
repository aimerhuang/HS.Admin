using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockInResult : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("error_code")]
public  		string
  errorCode { get; set; }


         [XmlElement("error_msg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("stock_in_bill_id")]
public  		long
  stockInBillId { get; set; }


         [XmlElement("stock_in_time")]
public  		DateTime
  stockInTime { get; set; }


         [XmlElement("skuinfo_list")]
public  		List<string>
  skuinfoList { get; set; }


}
}
