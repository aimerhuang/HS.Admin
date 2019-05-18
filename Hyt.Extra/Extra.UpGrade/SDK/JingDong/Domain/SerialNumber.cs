using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SerialNumber : JdObject{


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("serialNumber")]
public  		string
  serialNumber { get; set; }


         [XmlElement("bizType")]
public  		string
  bizType { get; set; }


         [XmlElement("bizTypeName")]
public  		string
  bizTypeName { get; set; }


         [XmlElement("bizNo")]
public  		string
  bizNo { get; set; }


         [XmlElement("createTimeStr")]
public  		string
  createTimeStr { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("warehouseName")]
public  		string
  warehouseName { get; set; }


}
}
