using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ADReportVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("sumDisplay")]
public  		long
  sumDisplay { get; set; }


         [XmlElement("sumClick")]
public  		long
  sumClick { get; set; }


         [XmlElement("sumCost")]
public  		double
  sumCost { get; set; }


}
}
