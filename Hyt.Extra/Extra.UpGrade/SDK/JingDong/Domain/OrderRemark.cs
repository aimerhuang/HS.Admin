using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderRemark : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("flag")]
public  		int
  flag { get; set; }


}
}
