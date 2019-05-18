using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RtwResult : JdObject{


         [XmlElement("eclpRtwNo")]
public  		string
  eclpRtwNo { get; set; }


         [XmlElement("isvRtwNum")]
public  		string
  isvRtwNum { get; set; }


         [XmlElement("eclpSoNo")]
public  		string
  eclpSoNo { get; set; }


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("source")]
public  		string
  source { get; set; }


         [XmlElement("reason")]
public  		string
  reason { get; set; }


         [XmlElement("createTime")]
public  		string
  createTime { get; set; }


         [XmlElement("createUser")]
public  		string
  createUser { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("msg")]
public  		string
  msg { get; set; }


}
}
