using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceInfo : JdObject{


         [XmlElement("serviceId")]
public  		long
  serviceId { get; set; }


         [XmlElement("sourceId")]
public  		string
  sourceId { get; set; }


         [XmlElement("isvPin")]
public  		string
  isvPin { get; set; }


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("shopNo")]
public  		string
  shopNo { get; set; }


         [XmlElement("eclpOrderId")]
public  		string
  eclpOrderId { get; set; }


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("wareType")]
public  		string
  wareType { get; set; }


         [XmlElement("questionDesc")]
public  		string
  questionDesc { get; set; }


         [XmlElement("applyReason")]
public  		string
  applyReason { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
