using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReturnAddressVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("contact")]
public  		string
  contact { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("zip_code")]
public  		string
  zipCode { get; set; }


         [XmlElement("full_address")]
public  		string
  fullAddress { get; set; }


         [XmlElement("full_area_id")]
public  		string
  fullAreaId { get; set; }


         [XmlElement("address_type")]
public  		int
  addressType { get; set; }


         [XmlElement("create_time")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("modify_time")]
public  		DateTime
  modifyTime { get; set; }


}
}
