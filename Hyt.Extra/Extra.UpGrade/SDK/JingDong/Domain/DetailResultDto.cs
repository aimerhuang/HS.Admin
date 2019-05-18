using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DetailResultDto : JdObject{


         [XmlElement("returnId")]
public  		long
  returnId { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("providerName")]
public  		string
  providerName { get; set; }


         [XmlElement("fromDeliverCenterName")]
public  		string
  fromDeliverCenterName { get; set; }


         [XmlElement("toDeliverCenterName")]
public  		string
  toDeliverCenterName { get; set; }


         [XmlElement("totalNum")]
public  		int
  totalNum { get; set; }


         [XmlElement("totalPrice")]
public  		string
  totalPrice { get; set; }


         [XmlElement("wareVariety")]
public  		int
  wareVariety { get; set; }


         [XmlElement("bookingDate")]
public  		DateTime
  bookingDate { get; set; }


         [XmlElement("deliverTime")]
public  		DateTime
  deliverTime { get; set; }


         [XmlElement("balanceState")]
public  		int
  balanceState { get; set; }


         [XmlElement("balanceStateName")]
public  		string
  balanceStateName { get; set; }


         [XmlElement("balanceDate")]
public  		DateTime
  balanceDate { get; set; }


         [XmlElement("opinion")]
public  		string
  opinion { get; set; }


         [XmlElement("outStoreRoomDate")]
public  		DateTime
  outStoreRoomDate { get; set; }


         [XmlElement("detailDtoList")]
public  		List<string>
  detailDtoList { get; set; }


}
}
