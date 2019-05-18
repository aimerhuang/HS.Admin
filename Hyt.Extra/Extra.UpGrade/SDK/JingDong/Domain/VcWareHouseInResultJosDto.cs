using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWareHouseInResultJosDto : JdObject{


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("vcWareHouseInJosDtos")]
public  		List<string>
  vcWareHouseInJosDtos { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


}
}
