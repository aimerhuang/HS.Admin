using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceDetail : JdObject{


         [XmlElement("afsServiceDetailId")]
public  		int
  afsServiceDetailId { get; set; }


         [XmlElement("afsServiceId")]
public  		int
  afsServiceId { get; set; }


         [XmlElement("wareId")]
public  		int
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("wareDescribe")]
public  		string
  wareDescribe { get; set; }


         [XmlElement("wareBrand")]
public  		string
  wareBrand { get; set; }


         [XmlElement("wareCid1")]
public  		int
  wareCid1 { get; set; }


         [XmlElement("wareCid2")]
public  		int
  wareCid2 { get; set; }


         [XmlElement("wareCid3")]
public  		int
  wareCid3 { get; set; }


         [XmlElement("createName")]
public  		string
  createName { get; set; }


         [XmlElement("updateName")]
public  		string
  updateName { get; set; }


         [XmlElement("createDate")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("updateDate")]
public  		DateTime
  updateDate { get; set; }


}
}
