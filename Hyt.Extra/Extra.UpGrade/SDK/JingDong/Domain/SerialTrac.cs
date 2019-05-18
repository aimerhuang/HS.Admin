using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SerialTrac : JdObject{


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("serialNo")]
public  		string
  serialNo { get; set; }


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


}
}
