using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PauseBizDataYy : JdObject{


         [XmlElement("codDT")]
public  		string
  codDT { get; set; }


         [XmlElement("dbDT")]
public  		string
  dbDT { get; set; }


         [XmlElement("ljDT")]
public  		string
  ljDT { get; set; }


}
}
