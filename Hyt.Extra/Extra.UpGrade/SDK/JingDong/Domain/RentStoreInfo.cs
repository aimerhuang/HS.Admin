using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RentStoreInfo : JdObject{


         [XmlElement("com_id")]
public  		long
  comId { get; set; }


         [XmlElement("com_name")]
public  		string
  comName { get; set; }


         [XmlElement("org_id")]
public  		long
  orgId { get; set; }


         [XmlElement("wh_id")]
public  		long
  whId { get; set; }


         [XmlElement("org_name")]
public  		string
  orgName { get; set; }


         [XmlElement("wh_name")]
public  		string
  whName { get; set; }


         [XmlElement("custom_name")]
public  		string
  customName { get; set; }


         [XmlElement("areaRent")]
public  		string
  areaRent { get; set; }


         [XmlElement("apply_time")]
public  		DateTime
  applyTime { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("contract")]
public  		string
  contract { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("zip_code")]
public  		string
  zipCode { get; set; }


         [XmlElement("back_name")]
public  		string
  backName { get; set; }


         [XmlElement("back_phone")]
public  		string
  backPhone { get; set; }


}
}
