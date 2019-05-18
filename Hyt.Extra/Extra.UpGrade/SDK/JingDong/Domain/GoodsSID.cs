using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GoodsSID : JdObject{


         [XmlElement("ckDateStr")]
public  		string
  ckDateStr { get; set; }


         [XmlElement("rkDateStr")]
public  		string
  rkDateStr { get; set; }


         [XmlElement("isOutStr")]
public  		string
  isOutStr { get; set; }


         [XmlElement("rkBusTypeStr")]
public  		string
  rkBusTypeStr { get; set; }


         [XmlElement("ckBusTypeStr")]
public  		string
  ckBusTypeStr { get; set; }


         [XmlElement("ckStoreName")]
public  		string
  ckStoreName { get; set; }


         [XmlElement("rkStoreName")]
public  		string
  rkStoreName { get; set; }


         [XmlElement("wareId")]
public  		string
  wareId { get; set; }


         [XmlElement("serial")]
public  		string
  serial { get; set; }


         [XmlElement("rkBusId")]
public  		string
  rkBusId { get; set; }


         [XmlElement("ckBusId")]
public  		string
  ckBusId { get; set; }


         [XmlElement("rkStoreId")]
public  		string
  rkStoreId { get; set; }


         [XmlElement("ckStoreId")]
public  		string
  ckStoreId { get; set; }


         [XmlElement("rkBusType")]
public  		string
  rkBusType { get; set; }


         [XmlElement("rkTime")]
public  		DateTime
  rkTime { get; set; }


         [XmlElement("ckBusType")]
public  		string
  ckBusType { get; set; }


         [XmlElement("ckTime")]
public  		DateTime
  ckTime { get; set; }


}
}
