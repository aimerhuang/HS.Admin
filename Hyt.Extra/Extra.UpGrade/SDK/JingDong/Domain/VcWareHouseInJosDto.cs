using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWareHouseInJosDto : JdObject{


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("docNo")]
public  		string
  docNo { get; set; }


         [XmlElement("companyCode")]
public  		string
  companyCode { get; set; }


         [XmlElement("distribCenterCode")]
public  		string
  distribCenterCode { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("stockInType")]
public  		string
  stockInType { get; set; }


         [XmlElement("stockInTypeName")]
public  		string
  stockInTypeName { get; set; }


         [XmlElement("stockOutStatusName")]
public  		string
  stockOutStatusName { get; set; }


         [XmlElement("docStatus")]
public  		string
  docStatus { get; set; }


         [XmlElement("docStatusName")]
public  		string
  docStatusName { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("pickwareId")]
public  		string
  pickwareId { get; set; }


         [XmlElement("unpackingTime")]
public  		DateTime
  unpackingTime { get; set; }


         [XmlElement("remark1")]
public  		string
  remark1 { get; set; }


         [XmlElement("remark2")]
public  		string
  remark2 { get; set; }


         [XmlElement("remark3")]
public  		string
  remark3 { get; set; }


         [XmlElement("remark4")]
public  		string
  remark4 { get; set; }


         [XmlElement("remark5")]
public  		string
  remark5 { get; set; }


}
}
