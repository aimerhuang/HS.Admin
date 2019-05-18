using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SOrderListResult : JdObject{


         [XmlElement("isSuccess")]
public  		string
  isSuccess { get; set; }


         [XmlElement("msgs")]
public  		string
  msgs { get; set; }


         [XmlElement("orderTotal")]
public  		string
  orderTotal { get; set; }


         [XmlElement("sOrderInfoList")]
public  		List<string>
  sOrderInfoList { get; set; }


}
}
