using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosAreaListBeanVO : JdObject{


         [XmlElement("province_id")]
public  		long
  provinceId { get; set; }


         [XmlElement("province_name")]
public  		string
  provinceName { get; set; }


         [XmlElement("city_id")]
public  		long
  cityId { get; set; }


         [XmlElement("city_name")]
public  		string
  cityName { get; set; }


         [XmlElement("county_id")]
public  		long
  countyId { get; set; }


         [XmlElement("county_name")]
public  		string
  countyName { get; set; }


         [XmlElement("town_id")]
public  		long
  townId { get; set; }


         [XmlElement("town_name")]
public  		string
  townName { get; set; }


         [XmlElement("cod")]
public  		bool
  cod { get; set; }


}
}
