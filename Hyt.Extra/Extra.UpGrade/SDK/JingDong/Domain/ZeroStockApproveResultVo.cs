using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ZeroStockApproveResultVo : JdObject{


         [XmlElement("afs_no")]
public  		string
  afsNo { get; set; }


         [XmlElement("ord_no")]
public  		string
  ordNo { get; set; }


         [XmlElement("ser_fin_rs")]
public  		string
  serFinRs { get; set; }


         [XmlElement("ser_fin_t")]
public  		string
  serFinT { get; set; }


         [XmlElement("ref_t")]
public  		string
  refT { get; set; }


}
}
