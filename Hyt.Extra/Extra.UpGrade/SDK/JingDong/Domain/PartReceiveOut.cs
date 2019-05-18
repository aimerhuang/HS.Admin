using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PartReceiveOut : JdObject{


         [XmlElement("receivePin")]
public  		string
  receivePin { get; set; }


         [XmlElement("receiveName")]
public  		string
  receiveName { get; set; }


         [XmlElement("partReceiveId")]
public  		int
  partReceiveId { get; set; }


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("waybill")]
public  		string
  waybill { get; set; }


         [XmlElement("receiveJudgment")]
public  		int
  receiveJudgment { get; set; }


         [XmlElement("partQuality")]
public  		int
  partQuality { get; set; }


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("wareSn")]
public  		string
  wareSn { get; set; }


         [XmlElement("wareAttachment")]
public  		string
  wareAttachment { get; set; }


         [XmlElement("partAppearance")]
public  		int
  partAppearance { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("createName")]
public  		string
  createName { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


}
}
