using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class LogisticsCompany : JdObject{


         [XmlElement("logistics_id")]
public  		long
  logisticsId { get; set; }


         [XmlElement("logistics_name")]
public  		string
  logisticsName { get; set; }


         [XmlElement("logistics_remark")]
public  		string
  logisticsRemark { get; set; }


         [XmlElement("sequence")]
public  		string
  sequence { get; set; }


         [XmlElement("agree_flag")]
public  		string
  agreeFlag { get; set; }


         [XmlElement("isCod")]
public  		string
  isCod { get; set; }


}
}
