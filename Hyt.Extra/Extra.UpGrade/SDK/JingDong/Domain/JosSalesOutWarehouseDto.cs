using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosSalesOutWarehouseDto : JdObject{


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("serialNo")]
public  		string
  serialNo { get; set; }


         [XmlElement("ckBusId")]
public  		string
  ckBusId { get; set; }


         [XmlElement("saleOrdTm")]
public  		DateTime
  saleOrdTm { get; set; }


         [XmlElement("ckTime")]
public  		DateTime
  ckTime { get; set; }


         [XmlElement("userPayablePayAmount")]
public  		string
  userPayablePayAmount { get; set; }


}
}
