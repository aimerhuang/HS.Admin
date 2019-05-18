using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaybillAddress : JdObject{


         [XmlElement("provinceId")]
public  		int
  provinceId { get; set; }


         [XmlElement("provinceName")]
public  		string
  provinceName { get; set; }


         [XmlElement("cityId")]
public  		int
  cityId { get; set; }


         [XmlElement("cityName")]
public  		string
  cityName { get; set; }


         [XmlElement("countryId")]
public  		int
  countryId { get; set; }


         [XmlElement("countryName")]
public  		string
  countryName { get; set; }


         [XmlElement("countrysideId")]
public  		int
  countrysideId { get; set; }


         [XmlElement("countrysideName")]
public  		string
  countrysideName { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("contact")]
public  		string
  contact { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


}
}
