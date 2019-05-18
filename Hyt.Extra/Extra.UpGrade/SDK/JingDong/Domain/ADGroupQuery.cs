using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ADGroupQuery : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("campaignId")]
public  		long
  campaignId { get; set; }


         [XmlElement("feeStr")]
public  		string
  feeStr { get; set; }


         [XmlElement("outerFeeStr")]
public  		string
  outerFeeStr { get; set; }


         [XmlElement("inSearchFee")]
public  		long
  inSearchFee { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("area")]
public  		string
  area { get; set; }


         [XmlElement("newAreaId")]
public  		string
  newAreaId { get; set; }


         [XmlElement("groupDirection")]
public  		string
  groupDirection { get; set; }


         [XmlElement("position")]
public  		string
  position { get; set; }


         [XmlElement("billingType")]
public  		int
  billingType { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


}
}
