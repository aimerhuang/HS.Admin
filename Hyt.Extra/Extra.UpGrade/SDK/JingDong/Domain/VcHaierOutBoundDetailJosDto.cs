using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcHaierOutBoundDetailJosDto : JdObject{


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


         [XmlElement("returnPrice")]
public  		string
  returnPrice { get; set; }


         [XmlElement("returnNum")]
public  		int
  returnNum { get; set; }


         [XmlElement("erpCode")]
public  		string
  erpCode { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("settlementCode")]
public  		string
  settlementCode { get; set; }


         [XmlElement("wareSku")]
public  		long
  wareSku { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


}
}
