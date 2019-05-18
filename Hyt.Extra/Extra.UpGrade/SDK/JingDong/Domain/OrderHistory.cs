using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderHistory : JdObject{


         [XmlElement("orderCode")]
public  		string
  orderCode { get; set; }


         [XmlElement("orderStatus")]
public  		int
  orderStatus { get; set; }


         [XmlElement("clientId")]
public  		string
  clientId { get; set; }


         [XmlElement("clientBusinessNo")]
public  		string
  clientBusinessNo { get; set; }


         [XmlElement("orderTime")]
public  		string
  orderTime { get; set; }


}
}
