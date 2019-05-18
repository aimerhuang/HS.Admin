using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsRefundDto : JdObject{


         [XmlElement("customer_order_id")]
public  		string
  customerOrderId { get; set; }


         [XmlElement("typeId")]
public  		int
  typeId { get; set; }


         [XmlElement("itemName")]
public  		string
  itemName { get; set; }


         [XmlElement("itemMoney")]
public  		string
  itemMoney { get; set; }


}
}
