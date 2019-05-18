using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ResultInfoDTO : JdObject{


         [XmlElement("rcode")]
public  		int
  rcode { get; set; }


         [XmlElement("rmessage")]
public  		string
  rmessage { get; set; }


         [XmlElement("sourcetSortCenterId")]
public  		int
  sourcetSortCenterId { get; set; }


         [XmlElement("sourcetSortCenterName")]
public  		string
  sourcetSortCenterName { get; set; }


         [XmlElement("originalCrossCode")]
public  		string
  originalCrossCode { get; set; }


         [XmlElement("originalTabletrolleyCode")]
public  		string
  originalTabletrolleyCode { get; set; }


         [XmlElement("targetSortCenterId")]
public  		int
  targetSortCenterId { get; set; }


         [XmlElement("targetSortCenterName")]
public  		string
  targetSortCenterName { get; set; }


         [XmlElement("destinationCrossCode")]
public  		string
  destinationCrossCode { get; set; }


         [XmlElement("destinationTabletrolleyCode")]
public  		string
  destinationTabletrolleyCode { get; set; }


         [XmlElement("siteId")]
public  		int
  siteId { get; set; }


         [XmlElement("siteName")]
public  		string
  siteName { get; set; }


         [XmlElement("road")]
public  		string
  road { get; set; }


         [XmlElement("aging")]
public  		int
  aging { get; set; }


         [XmlElement("agingName")]
public  		string
  agingName { get; set; }


         [XmlElement("smileDelivery")]
public  		string
  smileDelivery { get; set; }


}
}
