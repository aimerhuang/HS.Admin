using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderExtResult : JdObject{


         [XmlElement("isvUUID")]
public  		string
  isvUUID { get; set; }


         [XmlElement("eclpSoNo")]
public  		string
  eclpSoNo { get; set; }


         [XmlElement("wayBill")]
public  		string
  wayBill { get; set; }


         [XmlElement("mainStatus")]
public  		int
  mainStatus { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


}
}
