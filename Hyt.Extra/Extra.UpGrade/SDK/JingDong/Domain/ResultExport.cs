using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ResultExport : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("data")]
public  		string
  data { get; set; }


         [XmlElement("errMsg")]
public  		string
  errMsg { get; set; }


}
}
