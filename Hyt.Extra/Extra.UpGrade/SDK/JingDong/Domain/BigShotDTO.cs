using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BigShotDTO : JdObject{


         [XmlElement("waybillCode")]
public  		string
  waybillCode { get; set; }


         [XmlElement("bigShotName")]
public  		string
  bigShotName { get; set; }


         [XmlElement("bigShotCode")]
public  		string
  bigShotCode { get; set; }


         [XmlElement("gatherCenterName")]
public  		string
  gatherCenterName { get; set; }


         [XmlElement("gatherCenterCode")]
public  		string
  gatherCenterCode { get; set; }


         [XmlElement("branchName")]
public  		string
  branchName { get; set; }


         [XmlElement("branchCode")]
public  		string
  branchCode { get; set; }


         [XmlElement("secondSectionCode")]
public  		string
  secondSectionCode { get; set; }


         [XmlElement("thirdSectionCode")]
public  		string
  thirdSectionCode { get; set; }


}
}
