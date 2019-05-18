using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IncomeExpenseVO : JdObject{


         [XmlElement("swift_number")]
public  		long
  swiftNumber { get; set; }


         [XmlElement("creat_time")]
public  		string
  creatTime { get; set; }


         [XmlElement("amount")]
public  		long
  amount { get; set; }


         [XmlElement("in_out_type")]
public  		long
  inOutType { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("show_date")]
public  		string
  showDate { get; set; }


}
}
