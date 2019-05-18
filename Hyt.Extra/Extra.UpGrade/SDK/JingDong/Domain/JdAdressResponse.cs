using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JdAdressResponse : JdObject{


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("provinceId")]
public  		string
  provinceId { get; set; }


         [XmlElement("provinceName")]
public  		string
  provinceName { get; set; }


         [XmlElement("cityId")]
public  		string
  cityId { get; set; }


         [XmlElement("cityName")]
public  		string
  cityName { get; set; }


         [XmlElement("countryId")]
public  		string
  countryId { get; set; }


         [XmlElement("countryName")]
public  		string
  countryName { get; set; }


         [XmlElement("townId")]
public  		string
  townId { get; set; }


         [XmlElement("townName")]
public  		string
  townName { get; set; }


         [XmlElement("lng")]
public  		string
  lng { get; set; }


         [XmlElement("lat")]
public  		string
  lat { get; set; }


         [XmlElement("reliability")]
public  		string
  reliability { get; set; }


         [XmlElement("shipCodResult")]
public  		string
  shipCodResult { get; set; }


}
}
