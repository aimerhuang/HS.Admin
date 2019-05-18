using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class MachiningHeader : JdObject{


         [XmlElement("machiningNo")]
public  		string
  machiningNo { get; set; }


         [XmlElement("machiningType")]
public  		string
  machiningType { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("machiningSrcDetailList")]
public  		List<string>
  machiningSrcDetailList { get; set; }


         [XmlElement("machiningDestDetailList")]
public  		List<string>
  machiningDestDetailList { get; set; }


}
}
