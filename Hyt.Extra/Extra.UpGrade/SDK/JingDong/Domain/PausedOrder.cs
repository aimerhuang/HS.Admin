using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PausedOrder : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("skus")]
public  		List<string>
  skus { get; set; }


}
}
