using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReturnOrderPreForJosResultDto : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("customOrderId")]
public  		long
  customOrderId { get; set; }


         [XmlElement("roApplyFee")]
public  		string
  roApplyFee { get; set; }


         [XmlElement("roApplyDate")]
public  		DateTime
  roApplyDate { get; set; }


         [XmlElement("orderCreateDate")]
public  		DateTime
  orderCreateDate { get; set; }


         [XmlElement("approvalState")]
public  		int
  approvalState { get; set; }


         [XmlElement("orderState")]
public  		int
  orderState { get; set; }


         [XmlElement("roPreNo")]
public  		long
  roPreNo { get; set; }


         [XmlElement("roAccount")]
public  		string
  roAccount { get; set; }


         [XmlElement("roReason")]
public  		string
  roReason { get; set; }


         [XmlElement("approvalSuggestion")]
public  		string
  approvalSuggestion { get; set; }


         [XmlElement("orderDetailList")]
public  		List<string>
  orderDetailList { get; set; }


}
}
