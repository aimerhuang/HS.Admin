using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RoDto : JdObject{


         [XmlElement("returnId")]
public  		long
  returnId { get; set; }


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("providerName")]
public  		string
  providerName { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("fromDeliverCenterName")]
public  		string
  fromDeliverCenterName { get; set; }


         [XmlElement("toDeliverCenterName")]
public  		string
  toDeliverCenterName { get; set; }


         [XmlElement("returnStateName")]
public  		string
  returnStateName { get; set; }


         [XmlElement("totalNum")]
public  		int
  totalNum { get; set; }


         [XmlElement("totalPrice")]
public  		string
  totalPrice { get; set; }


         [XmlElement("stockName")]
public  		string
  stockName { get; set; }


         [XmlElement("wareHouseAddress")]
public  		string
  wareHouseAddress { get; set; }


         [XmlElement("wareHouseCell")]
public  		string
  wareHouseCell { get; set; }


         [XmlElement("wareHouseContact")]
public  		string
  wareHouseContact { get; set; }


         [XmlElement("outStoreRoomDate")]
public  		DateTime
  outStoreRoomDate { get; set; }


         [XmlElement("wareVariety")]
public  		int
  wareVariety { get; set; }


         [XmlElement("balanceStateName")]
public  		string
  balanceStateName { get; set; }


         [XmlElement("balanceDate")]
public  		DateTime
  balanceDate { get; set; }


}
}
