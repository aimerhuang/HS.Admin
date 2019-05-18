using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PublicResultListAfsServiceHistory : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("resultErrorMsg")]
public  		string
  resultErrorMsg { get; set; }


         [XmlElement("afsServiceHistoryList")]
public  		List<string>
  afsServiceHistoryList { get; set; }


}
}
