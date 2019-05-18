using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOutsideMainResult : JdObject{


         [XmlElement("outsideStatus")]
public  		string
  outsideStatus { get; set; }


         [XmlElement("isvOutsideNo")]
public  		string
  isvOutsideNo { get; set; }


         [XmlElement("warehouseIdOut")]
public  		long
  warehouseIdOut { get; set; }


         [XmlElement("warehouseIdIn")]
public  		long
  warehouseIdIn { get; set; }


         [XmlElement("boxes")]
public  		string
  boxes { get; set; }


}
}
