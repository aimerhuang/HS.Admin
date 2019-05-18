using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryStockOutResult : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("error_code")]
public  		string
  errorCode { get; set; }


         [XmlElement("error_msg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("vender_id")]
public  		long
  venderId { get; set; }


         [XmlElement("stockbillinfo_list")]
public  		List<string>
  stockbillinfoList { get; set; }


}
}
