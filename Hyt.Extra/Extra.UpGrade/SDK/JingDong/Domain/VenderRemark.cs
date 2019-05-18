using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderRemark : JdObject{


         [XmlElement("order_id")]
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
public  		string
  flag { get; set; }


}
}
