using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderInfoResultPauseBizInfo : JdObject{


         [XmlElement("pauseBizStatusList")]
public  		List<string>
  pauseBizStatusList { get; set; }


         [XmlElement("pauseBizDataYy")]
public  		string
  pauseBizDataYy { get; set; }


}
}
