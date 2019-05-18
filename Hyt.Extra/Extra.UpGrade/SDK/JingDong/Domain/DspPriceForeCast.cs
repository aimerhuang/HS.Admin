using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DspPriceForeCast : JdObject{


         [XmlElement("hourHigh")]
public  		List<string>
  hourHigh { get; set; }


         [XmlElement("hourMiddle")]
public  		List<string>
  hourMiddle { get; set; }


         [XmlElement("hourLow")]
public  		List<string>
  hourLow { get; set; }


         [XmlElement("dayHigh")]
public  		List<string>
  dayHigh { get; set; }


         [XmlElement("dayMiddle")]
public  		List<string>
  dayMiddle { get; set; }


         [XmlElement("dayLow")]
public  		List<string>
  dayLow { get; set; }


}
}
