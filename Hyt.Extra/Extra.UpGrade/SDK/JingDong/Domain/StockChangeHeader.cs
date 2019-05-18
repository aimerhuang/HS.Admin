using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockChangeHeader : JdObject{


         [XmlElement("changeNo")]
public  		string
  changeNo { get; set; }


         [XmlElement("changeType")]
public  		string
  changeType { get; set; }


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


         [XmlElement("stockChangeDetailList")]
public  		List<string>
  stockChangeDetailList { get; set; }


}
}
