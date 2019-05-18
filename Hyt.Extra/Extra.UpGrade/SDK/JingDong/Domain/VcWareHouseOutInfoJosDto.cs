using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWareHouseOutInfoJosDto : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


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


         [XmlElement("companyName")]
public  		string
  companyName { get; set; }


         [XmlElement("distribCenterName")]
public  		string
  distribCenterName { get; set; }


         [XmlElement("warehouseName")]
public  		string
  warehouseName { get; set; }


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


         [XmlElement("returnCode")]
public  		string
  returnCode { get; set; }


         [XmlElement("remarkForOutBound")]
public  		string
  remarkForOutBound { get; set; }


         [XmlElement("vcWareHouseOutSpareCodeJosDtoList")]
public  		List<string>
  vcWareHouseOutSpareCodeJosDtoList { get; set; }


}
}
