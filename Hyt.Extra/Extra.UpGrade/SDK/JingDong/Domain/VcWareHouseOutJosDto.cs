using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWareHouseOutJosDto : JdObject{


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("stockOutNo")]
public  		string
  stockOutNo { get; set; }


         [XmlElement("companyCode")]
public  		string
  companyCode { get; set; }


         [XmlElement("distribCenterCode")]
public  		string
  distribCenterCode { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("stockOutStatus")]
public  		int
  stockOutStatus { get; set; }


         [XmlElement("stockOutStatusName")]
public  		string
  stockOutStatusName { get; set; }


         [XmlElement("stockOutType")]
public  		string
  stockOutType { get; set; }


         [XmlElement("stockOutTypeName")]
public  		string
  stockOutTypeName { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("checkTime")]
public  		DateTime
  checkTime { get; set; }


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
