using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceApplyInfoExport : JdObject{


         [XmlElement("expectPickwareType")]
public  		int
  expectPickwareType { get; set; }


         [XmlElement("customerExpect")]
public  		int
  customerExpect { get; set; }


}
}
