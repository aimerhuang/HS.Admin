using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderListResult : JdObject{


         [XmlElement("apiResult")]
public  		string
  apiResult { get; set; }


         [XmlElement("orderTotal")]
public  		string
  orderTotal { get; set; }


         [XmlElement("orderInfoList")]
public  		List<string>
  orderInfoList { get; set; }


}
}
