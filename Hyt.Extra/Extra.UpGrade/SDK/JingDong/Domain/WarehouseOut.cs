using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WarehouseOut : JdObject{


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("warehouseName")]
public  		string
  warehouseName { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("contacts")]
public  		string
  contacts { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("county")]
public  		string
  county { get; set; }


         [XmlElement("town")]
public  		string
  town { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("reserve1")]
public  		string
  reserve1 { get; set; }


         [XmlElement("reserve2")]
public  		string
  reserve2 { get; set; }


         [XmlElement("reserve3")]
public  		string
  reserve3 { get; set; }


         [XmlElement("reserve4")]
public  		string
  reserve4 { get; set; }


         [XmlElement("reserve5")]
public  		string
  reserve5 { get; set; }


}
}
