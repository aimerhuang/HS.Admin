using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWarehouseOutAddResultJosBatchDto : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("resultMessage")]
public  		string
  resultMessage { get; set; }


         [XmlElement("vcWareHouseOutInfoJosDtos")]
public  		List<string>
  vcWareHouseOutInfoJosDtos { get; set; }


}
}
