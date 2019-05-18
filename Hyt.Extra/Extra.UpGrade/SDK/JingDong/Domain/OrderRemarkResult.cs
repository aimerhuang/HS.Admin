using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderRemarkResult : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("totleNum")]
public  		long
  totleNum { get; set; }


         [XmlElement("remarkList")]
public  		List<string>
  remarkList { get; set; }


}
}
