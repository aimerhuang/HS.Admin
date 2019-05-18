using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosMyProductInfoDto : JdObject{


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("sale_state")]
public  		int
  saleState { get; set; }


         [XmlElement("modify_time")]
public  		DateTime
  modifyTime { get; set; }


         [XmlElement("is_primary")]
public  		int
  isPrimary { get; set; }


         [XmlElement("is_gaea")]
public  		int
  isGaea { get; set; }


}
}
