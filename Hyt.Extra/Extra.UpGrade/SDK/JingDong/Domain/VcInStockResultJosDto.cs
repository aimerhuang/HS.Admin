using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcInStockResultJosDto : JdObject{


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("vcInStockSkuDtos")]
public  		List<string>
  vcInStockSkuDtos { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


}
}
