using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GoodsSerial : JdObject{


         [XmlElement("businessNo")]
public  		string
  businessNo { get; set; }


         [XmlElement("businessType")]
public  		string
  businessType { get; set; }


         [XmlElement("departmentNo")]
public  		string
  departmentNo { get; set; }


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("serialNumber")]
public  		string
  serialNumber { get; set; }


}
}
